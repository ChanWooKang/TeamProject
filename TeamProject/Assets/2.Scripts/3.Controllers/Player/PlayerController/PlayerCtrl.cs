using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DefineDatas;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Player Component")]
    public PlayerStat _stat;
    public PlayerMoveCtrl _move;
    public PlayerInputCtrl _input;
    public PlayerAnimCtrl _anim;
    public PlayerRenderCtrl _render;
    public PlayerEquipCtrl _equip;
    public PlayerColliderCtrl _collider;
    PetController m_recalledPet;    
    //Player Components
    //Animator _animator;
    PlayerInput _playerInput;
    PlayerAssetsInputs _assetInput;
    CharacterController _control;

    [SerializeField] Transform CameraRoot;
    GameObject _recogObject;
    
    Coroutine _damagedCoroutine;
    public bool isDead;
    public bool StopMove = false;

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
    public GameObject RecognizeObject { get { return _recogObject; } }
    public PetController RecalledPet { get { return m_recalledPet; } }
    private void Start()
    {
        Init();
        StartCoroutine(RegenerationStat());
    }

    private void Update()
    {
        _move.OnUpdate();
        _input.OnUpdate();
    }

    private void LateUpdate()
    {
        _input.CamRotate();
    }

    void Init()
    {        
        _playerInput = GetComponent<PlayerInput>();
        _assetInput = GetComponent<PlayerAssetsInputs>();
        _control = GetComponent<CharacterController>();

        InitControls();

        _recogObject = null;
        _damagedCoroutine = null;
        isDead = false;

        //게임매니저보다 먼저 실행되야 하는 스탯이 로드 됬으니 실행 
        // 차후 변경 ( 스탯을 게임매니저에서 받아오도록 변경
        GameManagerEx._inst.GameMangerStart();
    }

    void InitControls()
    {
        _stat = GetComponent<PlayerStat>();
        _move = GetComponent<PlayerMoveCtrl>();
        _input = GetComponent<PlayerInputCtrl>();
        _anim = GetComponent<PlayerAnimCtrl>();
        _render = GetComponent<PlayerRenderCtrl>();
        _equip = GetComponent<PlayerEquipCtrl>();
        _collider = GetComponent<PlayerColliderCtrl>();

        LoadStat();
        _input.Init(this, _assetInput);
        _move.Init(this, _assetInput, _control);
        _anim.Init(this);
        _render.Init(this);
        _equip.Init(this, _assetInput);
        _collider.Init(this);
    }

    void LoadStat()
    {
        _stat.Init();        
    }

    public void SetRecognizeObject(GameObject go = null)
    {
        _recogObject = go;
    }
   public void SetCorrentRecallPet(PetController pet)
    {
        m_recalledPet = pet;
    }
    public void OnDamage(float damage, Transform attackerT = null)
    {
        if (isDead && gameObject.activeSelf)
            return;

        isDead = _stat.GetHit(damage);
        if(m_recalledPet != null)
        m_recalledPet.SetTarget(attackerT);
        if (_damagedCoroutine != null)
            StopCoroutine(_damagedCoroutine);
        _damagedCoroutine = StartCoroutine(OnDamageEvent());
    }

    IEnumerator OnDamageEvent()
    {
        if (isDead)
        {
            _render.ChangeColor(Color.gray);
            yield break;
        }

        _render.ChangeColor(Color.red);
        yield return new WaitForSeconds(0.3f);
        _render.ReturnColor();
    }    

    IEnumerator RegenerationStat()
    {
        while(!isDead)
        {
            if(_stat.HP < _stat.MaxHP)
            {
                _stat.HP = Mathf.Min(_stat.HP + 5, _stat.MaxHP);
            }

            yield return new WaitForSeconds(1);
        }
    }
}
