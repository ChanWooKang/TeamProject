using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;
using DefineDatas;

public class PlayerInputCtrl : MonoBehaviour
{
    [Header("LayerMask")]
    [SerializeField] float _recogMaskRange;
    [SerializeField] LayerMask _recogLayer;

    [Header("Other Component")]
    public Image _hairImage;

    [Header("Cameras")]
    public CinemachineVirtualCamera AimCam;
    public GameObject CamTarget;
    public float TopClamp = 30.0f;
    public float BottomClamp = -10.0f;
    public float CamAngleOverride = 0;
    public bool LockCamPos = false;
    public float SpanMinY = -10.0f;
    public float SpanMaxY = 10.0f;

    //나중에 변경 해야함
    [Header("Craft")]
    [SerializeField] GameObject m_UICraftingPrefab;
    UI_Craft m_UICrafting;

    float _camTargetYaw;
    float _camTargetPitch;

    //Player Components
    PlayerCtrl _manager;
    PlayerAssetsInputs _input;

    const float _threshold = 0.01f;

    public void Init(PlayerCtrl manager, PlayerAssetsInputs input)
    {
        _manager = manager;
        _input = input;

        _camTargetYaw = CamTarget.transform.rotation.eulerAngles.y;
        ChangeAlpha(0.3f);
    }

    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        float baseAngle = 360.0f;
        if (lfAngle < -baseAngle) lfAngle += baseAngle;
        if (lfAngle > baseAngle) lfAngle -= baseAngle;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void OnUpdate()
    {
        CheckCrossHair();

        if (GameManagerEx._inst.CheckIsMoveAble() && GameManagerEx._inst.isOnBuild == false)
        {
            AimAction();

            CraftAction();

            InteractAction();

            FireAction();

            ThrowAction();

            ReloadAction();
        }

        InventoryAction();
    }

    public void CamRotate()
    {
        if (_input.look.sqrMagnitude >= _threshold && !LockCamPos)
        {
            float times = _manager.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
            _camTargetYaw += _input.look.x * times;
            _camTargetPitch += _input.look.y * times;
        }

        _camTargetYaw = ClampAngle(_camTargetYaw, float.MinValue, float.MaxValue);
        _camTargetPitch = ClampAngle(_camTargetPitch, BottomClamp, TopClamp);

        CamTarget.transform.rotation = Quaternion.Euler(_camTargetPitch + CamAngleOverride, _camTargetYaw, 0);
    }
    void CheckCrossHair()
    {
        Transform target = Camera.main.transform;

        if (Physics.Raycast(target.position, target.forward, out RaycastHit rhit, Mathf.Infinity, _recogLayer))
        {
            float dist = Vector3.SqrMagnitude(rhit.transform.position - transform.position);

            if (dist < _recogMaskRange * _recogMaskRange)
            {
                //Recognize Object Setting
                if (_manager.RecognizeObject != rhit.transform.gameObject)
                {
                    Debug.Log("Recoge Item");
                    _manager.SetRecognizeObject(rhit.transform.gameObject);
                }
                    
            }
        }
        else
        {
            //Recognize Object Reset
            if (_manager.RecognizeObject != null)
                _manager.SetRecognizeObject();
        }
    }

    void ChangeAlpha(float value)
    {
        Color color = _hairImage.color;
        color.a = value;
        _hairImage.color = color;
    }

    //Right Mouse Button
    void AimAction()
    {
        if (_input.aim)
        {
            if (_input.fire == false)
            {
                if (AimCam.gameObject.activeSelf == false)
                {
                    AimCam.gameObject.SetActive(true);
                    ChangeAlpha(1.0f);
                }
            }
        }
        else
        {
            if (AimCam.gameObject.activeSelf == true)
            {
                AimCam.gameObject.SetActive(false);
                ChangeAlpha(0.3f);
            }
        }

        //애니메이션 SetAnimation Bool Aim
        _manager._anim.SetAnimation(ePlayerAnimParams.Aim, _input.aim);
    }

    //Left Mouse Button
    void FireAction()
    {        
        if(_manager._equip.CheckAttackAble())
        {
            _manager._anim.SetAnimation(ePlayerAnimParams.Fire, _input.fire);
        }
        else
        {
            _manager._anim.SetAnimation(ePlayerAnimParams.Fire, false);
        }
    }

    void ThrowAction()
    {
        if (_input.throws)
        {
            _manager._equip.ReadyToAnimAction(false);
            if (AimCam.gameObject.activeSelf == false)
            {
                AimCam.gameObject.SetActive(true);
            }
        }
        else
        {            
            if (AimCam.gameObject.activeSelf == true)
            {
                AimCam.gameObject.SetActive(false);
            }

            if (_manager._equip.PetBallModel.activeSelf)
                _manager._equip.ThrowEnd();
        }
        _manager._anim.SetAnimation(ePlayerAnimParams.Throw, _input.throws);
    }

    //KeyBoard F Key
    void InteractAction()
    {
        if (_input.interact)
        {
            if (_manager.RecognizeObject != null)
            {
                GameObject go = _manager.RecognizeObject;
                if (go.TryGetComponent(out ObjectData data))
                {
                    if (data.isNPC)
                    {
                        TalkManager._inst.ShowText(go, data.objID, data.name);
                    }
                    else
                    {
                        if (go.TryGetComponent(out ItemCtrl item))
                        {
                            _manager._anim.SetAnimation(ePlayerAnimParams.Root);                            
                        }                        
                    }
                }
            }           
        }
        
    }

    void ReloadAction()
    {
        if (_input.reload)
        {
            if(_manager._anim.GetAnimation(ePlayerAnimParams.AcivateAnimation) == false)
                _manager._anim.SetAnimation(ePlayerAnimParams.Reload);
            _input.reload = false;
        }
    }

    //KeyBoard I Key => 나중에 탭키로 변환
    void InventoryAction()
    {
        if (_input.inventory)
        {
            InventoryManager._inst.invenUI.TryOpenInventory();
            _input.inventory = false;
        }
    }

    //KeyBoard B Key
    void CraftAction()
    {
        if (_input.craft)
        {
            if (m_UICrafting == null)
            {
                GameObject ui = Instantiate(m_UICraftingPrefab);
                Canvas canvas = ui.GetComponent<Canvas>();
                canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                m_UICrafting = ui.GetComponent<UI_Craft>();
                ui.SetActive(false);
                m_UICrafting.OpenUI(TechnologyManager._inst.TechLevel);
            }
            else
                m_UICrafting.OpenUI(TechnologyManager._inst.TechLevel);

            _input.craft = false;
        }
    }
}
