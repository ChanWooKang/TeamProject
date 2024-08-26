using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DefineDatas;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerInputController))]
[RequireComponent(typeof(PlayerAnimController))]
[RequireComponent(typeof(PlayerEquipController))]
public class PlayerManager : MonoBehaviour
{       
    [Header("Components")]
    public PlayerStat Stat;
    public PlayerMovementController Movement;
    public PlayerInputController InputCtrl;
    public PlayerAnimController AnimCtrl;
    public PlayerEquipController EquipCtrl;

    [Header("Audios")]
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    Animator _animator;
    PlayerInput _playerInput;
    PlayerAssetsInputs _input;
    CharacterController _controller;    
    GameObject _mainCamera;

    //Add Component
    Renderer[] _renders;
    GameObject _recognizeObject;

    public bool _hasAnimator;
    

    public bool IsCurrentDeviceMouse
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
    public GameObject RecognizeObject { get { return _recognizeObject; } }

    void Start()
    {
        InitComponent();       
    }    

    void Update()
    {
        //_hasAnimator = TryGetComponent(out _animator);

        
        //이동 및 점프
        Movement.OnUpdate();
        //마우스 회전, 줌 인, 키보드 액션
        InputCtrl.OnUpdate();
    }

    void LateUpdate()
    {
        //마우스 delta값으로 카메라 회전
        InputCtrl.CameraRotate();
        
    }

    void InitComponent()
    {
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerAssetsInputs>();
        _playerInput = GetComponent<PlayerInput>();
        _mainCamera = Camera.main.gameObject;

        Movement = GetComponent<PlayerMovementController>();
        InputCtrl = GetComponent<PlayerInputController>();
        AnimCtrl = GetComponent<PlayerAnimController>();
        EquipCtrl = GetComponent<PlayerEquipController>();
        
        LoadStat();
        InputCtrl.Init(this, _input);
        Movement.Init(this, _controller, _input, _mainCamera);
        AnimCtrl.Init(this, _animator);
        EquipCtrl.Init(this);
    }
    

    public void LoadStat()
    {
        Stat.Init();        
        InventoryManager._inst.MaxItemWeights = Stat.CarryWeight;
    }

    void ChangeColor(Color color)
    {
        foreach (Renderer render in _renders)
        {
            render.material.color = color;
        }
    }

    public void SetRecognizeObject(GameObject go = null)
    {
        _recognizeObject = go;
    }

    #region [ Animation Parameter Setting ]
    
    #endregion [ Animation Parameter Setting ]

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }

    #region [ Trigger Evenet ] 
    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Interact"))
        //{
        //    _nearObject = other.gameObject;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Interact"))
        //{
        //    _nearObject = null;
        //    if (TalkManager._talk.talkUI.mainObject.activeSelf)
        //    {
        //        //대화 UI가 켜져있을경우 끄기
        //        TalkManager._talk.ResetData();
        //    }
        //}
    }
    #endregion [ Trigger Event ] 
}
