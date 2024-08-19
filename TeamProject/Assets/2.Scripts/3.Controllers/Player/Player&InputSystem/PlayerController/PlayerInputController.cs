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
        //화면 중앙에 레이 쏘아 타겟 인식
        CrossHairCheck();

        //True 값 : 움직임 제어할 UI 켜져있지 않음
        if (GameManagerEx._inst.CheckIsMoveAble() && GameManagerEx._inst.isOnBuild == false)
        {
            //에임 조준 ( UI On일때에는 정지)
            SetAimCamera();
            //제작 UI UI온일때 작동 X
            CraftAction();
            //상호작용 UI on일때 작동 X
            InteractAction();
            //공격 UI on일때 작동 X
            FireAction();
        }     
        
        //인벤토리 열고 닫기 (UI on일때도 작동 )
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
        //카메라 Root 로 확인 밑을 바라보면 인식이 안됨
        //target = CinemachineCameraTarget.transform;
        //플레이어 자체로 밑을 바라봐도 인식이 되지만 플레이어가 뒤돌아 있으면 인식 안됨
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
