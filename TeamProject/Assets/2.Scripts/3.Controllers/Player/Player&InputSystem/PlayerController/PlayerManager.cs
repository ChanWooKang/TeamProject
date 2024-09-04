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
    public PlayerAttackController AttackCtrl;

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
    [SerializeField] Transform _bodyMesh;
    [SerializeField] Color _baseColor;
    Renderer[] _renders;    
    GameObject _recognizeObject;    


    public bool _hasAnimator;
    public bool isDead = false;

    Coroutine DamageCoroutne = null;
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
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Managers._scene.CurrentScene.SceneLoad(eScene.GameScene);
        }
        //�̵� �� ����
        Movement.OnUpdate();
        //���콺 ȸ��, �� ��, Ű���� �׼�
        InputCtrl.OnUpdate();
    }

    void LateUpdate()
    {
        //���콺 delta������ ī�޶� ȸ��
        InputCtrl.CameraRotate();
        
    }

    void InitComponent()
    {
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerAssetsInputs>();
        _playerInput = GetComponent<PlayerInput>();
        _mainCamera = Camera.main.gameObject;
        _renders = _bodyMesh.GetComponentsInChildren<Renderer>();
        Stat = GetComponent<PlayerStat>();
        Movement = GetComponent<PlayerMovementController>();
        InputCtrl = GetComponent<PlayerInputController>();
        AnimCtrl = GetComponent<PlayerAnimController>();
        EquipCtrl = GetComponent<PlayerEquipController>();
        AttackCtrl = GetComponent<PlayerAttackController>();


        LoadStat();
        InputCtrl.Init(this, _input);
        Movement.Init(this, _controller, _input, _mainCamera);
        AnimCtrl.Init(this, _animator);
        EquipCtrl.Init(this);
        AttackCtrl.Init(this);
        
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
            if(render.materials.Length > 1)
            {
                for (int i = 0; i < render.materials.Length; i++)
                {                    
                    render.materials[i].color = color;
                    
                }
            }
            else
            {
                render.material.color = color;
            }
            
        }
    }

    public void SetRecognizeObject(GameObject go = null)
    {
        _recognizeObject = go;
    }

    public void OnDamage()
    {
        if (DamageCoroutne != null)
            StopCoroutine(DamageCoroutne);
        DamageCoroutne = StartCoroutine(OnDamageEvent());
    }

    public void OnDamage(float damage, MonsterController mc = null)
    {
        if (isDead)
            return;
        //�� ���� �� (Ÿ��)

        isDead = Stat.GetHit(damage);        

        OnDamage();
    }

    IEnumerator OnDamageEvent()
    {
        if (isDead)
        {
            ChangeColor(Color.gray);
            yield break;
        }

        ChangeColor(Color.red);
        yield return new WaitForSeconds(0.3f);        
        ChangeColor(_baseColor);
    }

    void Attack()
    {
        //������ Ÿ�� ������
    }

    #region [ Animation Parameter Setting ]
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
    #endregion [ Animation Parameter Setting ]

    #region [ Trigger Evenet ] 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slash"))
        {
            if(other.TryGetComponent(out LeafSlash slash))
            {
                OnDamage(slash.Damage);
                slash.gameObject.DestroyAPS();
            }
        }

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
        //        //��ȭ UI�� ����������� ����
        //        TalkManager._talk.ResetData();
        //    }
        //}
    }
    #endregion [ Trigger Event ] 
}
