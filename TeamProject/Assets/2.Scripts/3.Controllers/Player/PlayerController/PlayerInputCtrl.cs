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
    public bool isRecall = false;
    //���߿� ���� �ؾ���
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

        if (GameManagerEx._inst.CheckIsMoveAble() && GameManagerEx._inst.isOnBuild == false && _manager.StopMove == false)
        {
            AimAction();



            InteractAction();

            FireAction();

            ThrowAction();

            InputAction();

            ReCallAction();

            ReloadAction();
        }
        CraftAction();
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
                }
                ChangeAlpha(1.0f);
            }
        }
        else
        {
            if (AimCam.gameObject.activeSelf == true)
            {
                AimCam.gameObject.SetActive(false);
            }
            ChangeAlpha(0.3f);
        }

        //�ִϸ��̼� SetAnimation Bool Aim
        _manager._anim.SetAnimation(ePlayerAnimParams.Aim, _input.aim);
    }

    //Left Mouse Button
    void FireAction()
    {
        if (_manager._equip.CheckAttackAble())
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
        if (_input.recall || InventoryManager._inst.GetItemCount(InventoryManager._inst.weaponUI.BallIndex) == 0)
            return;
        if (_input.throws)
        {
            _manager._equip.ReadyToAnimAction(false);
            if (AimCam.gameObject.activeSelf == false)
            {
                AimCam.gameObject.SetActive(true);
            }
            isRecall = false;
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
    void ReCallAction()
    {
        if (PetEntryManager._inst.m_listPetEntryCtrl.Count == 0 || _input.throws || _manager._anim.GetCurrentAnimationStateInfo(ePlayerAnimLayers.OneHandLayer, ePlayerAnimParams.Putin) || UIManager._inst.UIPetEntry.RecalledPet != null)
            return;
        if (_input.recall)
        {
            _manager._equip.ReadyToAnimAction(false);
            if (AimCam.gameObject.activeSelf == false)
            {
                AimCam.gameObject.SetActive(true);
            }
            isRecall = true;
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
        _manager._anim.SetAnimation(ePlayerAnimParams.Recall, _input.recall);
    }
    void InputAction()
    {
        if (PetEntryManager._inst.m_listPetEntryCtrl.Count == 0 || _input.throws || _input.recall)
            return;

        if (_input.putin)
        {
            if (UIManager._inst.UIPetEntry.RecalledPet != null)
            {
                if (UIManager._inst.UIPetEntry.RecalledPet.activeSelf)
                {
                    isRecall = UIManager._inst.UIPetEntry.RecallOrPutIn();
                    _manager._anim.SetAnimation(ePlayerAnimParams.Putin);

                }
            }
            _input.putin = false;
        }
    }
    //KeyBoard E Key
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
                        TalkManager._inst.ShowText(go, data.index);
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

            _input.interact = false;
        }

    }

    void ReloadAction()
    {
        if (_input.reload)
        {
            if (_manager._anim.GetAnimation(ePlayerAnimParams.AcivateAnimation) == false)
                _manager._anim.SetAnimation(ePlayerAnimParams.Reload);
            _input.reload = false;
        }
    }

    //KeyBoard I Key => ���߿� ��Ű�� ��ȯ
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
                m_UICrafting.InteractionUI(TechnologyManager._inst.TechLevel);
            }
            else
            {
                m_UICrafting.InteractionUI(TechnologyManager._inst.TechLevel);
            }

            _input.craft = false;
        }
    }
}
