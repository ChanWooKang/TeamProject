using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DefineDatas;

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

    public float SpanMinY = -10f;
    public float SpanMaxY = 10f;    
    //Animator _animator;

    PlayerManager _manager;
    PlayerAssetsInputs _input;

    //Cinemachine
    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;
    

    const float _threshold = 0.01f;

    public void Init(PlayerManager main,PlayerAssetsInputs input)
    {
        _manager = main;
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
  
    void CrossHairCheck()
    {        
        Transform target;
        //ī�޶� Root �� Ȯ�� ���� �ٶ󺸸� �ν��� �ȵ�
        //target = CinemachineCameraTarget.transform;
        target = Camera.main.transform;
        //�÷��̾� ��ü�� ���� �ٶ���� �ν��� ������ �÷��̾ �ڵ��� ������ �ν� �ȵ�
        //target = transform;
        if (Physics.Raycast(target.position, target.forward, out RaycastHit rhit,
                            Mathf.Infinity, _layerMask))
        {
            Vector3 distance = (rhit.point - transform.position);
            float dist = distance.sqrMagnitude;
            if (dist * dist <= Mathf.Pow(_recognizeMaskDistance, 2))
            {
                if (_manager.RecognizeObject != rhit.transform.gameObject)
                    _manager.SetRecognizeObject(rhit.transform.gameObject);
            }            
        }
        else
        {
            if(_manager.RecognizeObject != null)
                _manager.SetRecognizeObject();
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
            float deltaTimeMultipler = _manager.IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

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
            //���콺 �ٿ�
            if (_manager.AnimCtrl.GetAnimations(ePlayerAnimParams.AttackEnd) == true)
            {
                //����
                _manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Fire);                                
            }
            _manager.AnimCtrl.SetAnimations(ePlayerAnimParams.AttackEnd, false);
        }
        else
        {
            //���콺 ��
            _manager.AnimCtrl.SetAnimations(ePlayerAnimParams.AttackEnd, true);            
        }
    }

    void SetAimCamera()
    {
        if (_input.aim && _manager.Movement.Grounded) 
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

        _manager.AnimCtrl.SetAnimations(DefineDatas.ePlayerAnimParams.Aim, _input.aim);
    }    

    void InteractAction()
    {
        if (_input.interact)
        {
            if (_manager.RecognizeObject != null)
            {
                GameObject obj = _manager.RecognizeObject;
                if (obj.TryGetComponent(out ObjectData data))
                {
                    if (data.isNPC)
                    {
                        TalkManager._inst.ShowText(obj, data.objID, data.name);
                    }

                    else
                    {
                        if (obj.TryGetComponent(out ItemCtrl item))
                        {
                            if (item.Root())
                                _manager.SetRecognizeObject();
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
