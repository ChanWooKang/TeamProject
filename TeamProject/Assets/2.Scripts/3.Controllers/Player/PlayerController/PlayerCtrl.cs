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

    //Player Components
    //Animator _animator;
    PlayerInput _playerInput;
    PlayerAssetsInputs _assetInput;
    CharacterController _control;

    [SerializeField] Transform CameraRoot;
    GameObject _recogObject;
    
    Coroutine _damagedCoroutine;
    public bool isDead;

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

    private void Start()
    {
        Init();
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
        InventoryManager._inst.MaxItemWeights = _stat.CarryWeight;
    }

    public void SetRecognizeObject(GameObject go = null)
    {
        _recogObject = go;
    }
   
    public void OnDamage(float damage)
    {
        if (isDead && gameObject.activeSelf)
            return;

        isDead = _stat.GetHit(damage);

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
}
