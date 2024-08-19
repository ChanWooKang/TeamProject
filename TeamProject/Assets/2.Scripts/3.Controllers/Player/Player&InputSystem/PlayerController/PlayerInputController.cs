using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour
{

    [Header("Craft")]
    [SerializeField] GameObject m_UICraftingPrefab;
    UI_Craft m_UICrafting;

    [Header("CrossHair Image")]
    [SerializeField] Image _crossHair;

    [Header("LayerMasks")]
    [SerializeField] float _recognizeMaskDistance;
    [SerializeField] LayerMask _layerMask;

    [Header("Cameras")]
    public CinemachineVirtualCamera _aimCam;
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

    PlayerManager manager;
    PlayerAssetsInputs _input;

    //Cinemachine
    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;
    

    const float _threshold = 0.01f;

    public void Init(PlayerManager main,PlayerAssetsInputs input)
    {
        manager = main;
        _input = input;

        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }


    public void OnUpdate()
    {
        //ȭ�� �߾ӿ� ���� ��� Ÿ�� �ν�
        CrossHairCheck();

        //True �� : ������ ������ UI �������� ����
        if (GameManagerEx._inst.CheckIsMoveAble() && GameManagerEx._inst.isOnBuild == false)
        {
            //���� ���� ( UI On�϶����� ����)
            SetAimCamera();
            //���� UI UI���϶� �۵� X
            CraftAction();
            //��ȣ�ۿ� UI on�϶� �۵� X
            InteractAction();
            //���� UI on�϶� �۵� X
            FireAction();
        }     
        
        //�κ��丮 ���� �ݱ� (UI on�϶��� �۵� )
        InventoryAction();
    }

    static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(CinemachineCameraTarget.transform.position, CinemachineCameraTarget.transform.forward * _recognizeMaskDistance);
    }

    void CrossHairCheck()
    {        
        Transform target;
        //ī�޶� Root �� Ȯ�� ���� �ٶ󺸸� �ν��� �ȵ�
        //target = CinemachineCameraTarget.transform;
        //�÷��̾� ��ü�� ���� �ٶ���� �ν��� ������ �÷��̾ �ڵ��� ������ �ν� �ȵ�
        target = transform;
        if (Physics.Raycast(target.position, target.forward, out RaycastHit rhit,
                            _recognizeMaskDistance, _layerMask))
        {            
            if(manager.RecognizeObject != rhit.transform.gameObject)
                manager.SetRecognizeObject(rhit.transform.gameObject);
        }
        else
        {
            if(manager.RecognizeObject != null)
                manager.SetRecognizeObject();
        }
            
    }

    void CrossHairAlphaCtrl(float alpha)
    {
        Color color = _crossHair.color;
        color.a = alpha;
        _crossHair.color = color;
    }

    public void CameraRotate()
    {       
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            float deltaTimeMultipler = manager.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultipler;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultipler;
        }

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    void FireAction()
    {
        if (_input.fire)
        {            
            manager.AnimCtrl.SetAnimations(DefineDatas.ePlayerAnimParams.Attack, true);
        }
        else
        {
            manager.AnimCtrl.SetAnimations(DefineDatas.ePlayerAnimParams.Attack, false);
        }
    }

    void SetAimCamera()
    {
        if (_input.aim && manager.Movement.Grounded) 
        {
            if(!_aimCam.gameObject.activeSelf)
            {
                _aimCam.gameObject.SetActive(true);
                CrossHairAlphaCtrl(1.0f);
            }
                
            
        }
        else
        {
            if (_aimCam.gameObject.activeSelf)
            {
                _aimCam.gameObject.SetActive(false);
                CrossHairAlphaCtrl(0.3f);
            }
        }
        
            
    }    

    void InteractAction()
    {
        if (_input.interact)
        {
            if (manager.RecognizeObject != null)
            {
                GameObject obj = manager.RecognizeObject;
                if (obj.TryGetComponent(out ObjectData data))
                {
                    if (data.isNPC)
                    {
                        TalkManager._talk.ShowText(obj, data.objID, data.name);
                    }

                    else
                    {
                        if (obj.TryGetComponent(out BaseItems item))
                        {
                            if (item.Root())
                                manager.SetRecognizeObject();
                        }
                    }
                }
            }
            _input.interact = false;           
        }
    }

    void InventoryAction()
    {
        if (_input.inventory)
        {                        
            InventoryManager._inst.invenUI.TryOpenInventory();
            _input.inventory = false;
        }
    }

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
                m_UICrafting.OpenUI();
            }
            else
                m_UICrafting.OpenUI();

            _input.craft = false;
        }
    }

    
}
