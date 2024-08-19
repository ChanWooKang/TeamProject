using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif


[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif 
public class PlayerController : MonoBehaviour
{
    [Header("Player Stat")]
    [Tooltip(" MoveSpeed in m/s")]
    public float MoveSpeed;
    [Tooltip(" SprintSpeed in m/s")]
    public float SprintSpeed;
    [Tooltip("Accel & Decel")]
    public float SpeedChangeRate = 10.0f;
    [Tooltip("Smooth Rotate SpeedTime")]
    [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
    [Tooltip("Check Player State is Dead")]
    public bool isDead;
    
    [Header("Jump")]
    [Tooltip("Jump Height")]
    public float JumpHeight = 1.2f;
    [Tooltip("Gravity")]
    public float Gravity = -15.0f;
    [Tooltip("Jump Cooltime")]
    public float JumpTimeOut = 0.5f;
    [Tooltip("Fall State Required Time")]
    public float FallTimeOut = 0.15f;
    
    [Header("Player Grounded")]
    [Tooltip("Player Grounded Check")]
    public bool Grounded = true;
    [Tooltip("Userful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("Radius of Grounded Check")]
    public float GroundedRadius = 0.28f;
    [Tooltip("Layers Ground")]
    public LayerMask GroundLayers;
    
    [Tooltip("Follow Target Set in Cinemachine VirtualCamer Follow root")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("Camera up Able Degress")]
    public float TopClamp = 70.0f;
    [Tooltip("Camera down Able Degress")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;
    
    [Header("Audios")]
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Header("Components")]
    public GameObject MainCamera;    
    public PlayerStat Stat;


    //Cinemachine
    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;

    //Player
    float _speed;
    float _animationBlend;
    float _targetRotation = 0.0f;
    float _rotationVelocity;
    float _verticalVelocity;
    float _terminalVelocity = 53.0f;

    //Timeout deltaTime
    float _jumpTimeOutDelta;
    float _fallTimeOutDelta;

    //Animation IDs
    int _animIDSpeed;
    int _animIDGrounded;
    int _animIDJump;
    int _animIDFreeFall;
    int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM
    PlayerInput _playerInput;
#endif
    Animator _animator;
    CharacterController _controller;
    PlayerAssetsInputs _input;
    GameObject _mainCamera;

    //Add Component
    Renderer[] _renders;
    GameObject _nearObject;

    //Unitask
    CancellationTokenSource _source = new CancellationTokenSource();

    const float _threshold = 0.01f;

    bool _hasAnimator;

    bool IsCurrentDeviceMouse
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return _playerInput.currentControlScheme == "KeyBoardMouse";
#else
            return false;
#endif
        }
    }
    TalkManager talkManager { get { return TalkManager._talk; } }


    private void Start()
    {
        InitComponent();        
    }

    private void Update()
    {
        //_hasAnimator = TryGetComponent(out _animator);

        JumpAndGravity();
        GroundedCheck();
        Move();
        KeyBoardEvent();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    #region [ Sub Method ]
    void InitComponent()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#else
        Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
        //AddComponents
        _renders = GetComponentsInChildren<Renderer>();
        _nearObject = null;

        //Aimation ID Assign
        if (_hasAnimator)
            AssignAnimationIDs();

        // reset out timeouts on start
        _jumpTimeOutDelta = JumpTimeOut;
        _fallTimeOutDelta = FallTimeOut;

        //Component Setting
        _mainCamera = MainCamera;
        LoadStat();
    }

    void LoadStat()
    {
        Stat.Init();

        MoveSpeed = Stat.MoveSpeed;
        SprintSpeed = Stat.RunSpeed;

        InventoryManager._inst.MaxItemWeights = Stat.CarryWeight;

    }

    void ChangeColor(Color color)
    {
        foreach(Renderer render in _renders)
        {
            render.material.color = color;
        }
    }

    #endregion [ Sub Method ]

    #region [ Animations ]
    void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }
    #endregion [ Animations ]

    #region [ Move & Jump] 


    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    void CameraRotation()
    {        
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultipler = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultipler;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultipler;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (_hasAnimator)
            _animator.SetBool(_animIDGrounded, Grounded);
    }

    void JumpAndGravity()
    {
        if (Grounded)
        {
            _fallTimeOutDelta = FallTimeOut;

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump,false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            if (_verticalVelocity < 0.0f)
                _verticalVelocity = -2.0f;

            //JumpAction
            if (_input.jump && _jumpTimeOutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);

                if (_hasAnimator)                
                    _animator.SetBool(_animIDJump, true);
                
            }

            //Jump timeout
            if (_jumpTimeOutDelta >= 0.0f)
                _jumpTimeOutDelta -= Time.deltaTime;
        }
        else
        {
            _jumpTimeOutDelta = JumpTimeOut;

            if (_fallTimeOutDelta >= 0.0f)
            {
                _fallTimeOutDelta -= Time.deltaTime;
            }                
            else
            {
                if (_hasAnimator)
                    _animator.SetBool(_animIDFreeFall, true);
            }

            _input.jump = false;
        }

        if (_verticalVelocity < _terminalVelocity)
            _verticalVelocity += Gravity * Time.deltaTime;
    }

    void Move()
    {
        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

        Debug.Log(_input.move);
        
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
            
        }
        else
        {
            _speed = targetSpeed;
            
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0.0f;

        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        if(_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                            new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    #endregion [ Move & Jump & Animtaions ] 

    #region [ KeyBoardEvent ] 

    void KeyBoardEvent()
    {
        InteractionEvent();
        InventoryEvent();
    }

    void InteractionEvent()
    {
        if (_input.interact)
        {
            if (_nearObject.TryGetComponent(out ObjectData objData))
            {
                if (objData.isNPC)
                {
                    talkManager.ShowText(_nearObject, objData.objID, objData.name);
                }
                else
                {
                    if (_nearObject.TryGetComponent(out BaseItems item))
                    {
                        if (item.Root())
                        {
                            _nearObject = null;
                        }

                    }
                }
               
            }
            _input.interact = false;
        }            
    }

    void InventoryEvent()
    {
        if (_input.inventory)
        {
            InventoryManager._inst.invenUI.TryOpenInventory();
            _input.inventory = false;
        }
    }
    #endregion [ KeyBoardEvent ] 

    #region [ OnDamaged ] 
    public void OnDamage(float damage)
    {
        if(isDead)
            return;

        isDead = Stat.GetHit(damage);
        //UnitaskVoid 사용시 .Forget()
        if (_source != null)
            _source = new CancellationTokenSource();

        OnDamageEvent().Forget();
    }

    async UniTaskVoid OnDamageEvent()
    {
        //When Dead

        ChangeColor(Color.gray);
        await UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken : _source.Token);
        ChangeColor(Color.red);
    }

    #endregion [ OnDamaged ]

    #region [ Animation Event ]


    #endregion [ Animation Event ]

    #region [ Audio Event]
    void OnFootStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if(FootstepAudioClips.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }

    void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }
    #endregion [ Audio Event]

    #region [ Trigger Evenet ] 
    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            _nearObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            _nearObject = null;
            if (talkManager.talkUI.mainObject.activeSelf)
            {
                //대화 UI가 켜져있을경우 끄기
                talkManager.ResetData();
            }
        }
    }
    #endregion [ Trigger Event ] 

    #region [ OnEnable & OnDisable ] 

    private void OnEnable()
    {
        if (_source != null)
            _source = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        _source.Cancel();
    }
    #endregion [ OnEnable & OnDisable ] 
}
