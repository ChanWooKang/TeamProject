using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerCtrl : MonoBehaviour
{
    #region [ Component ]
    public PlayerStat Stat;
    [SerializeField] Transform playerBody;
    [SerializeField] Transform cameraArm;
    [SerializeField] WeaponCtrl EquipWeapon;
    [SerializeField] Renderer render;
    #endregion [ Component ]

    #region [ �������� ]
    public bool isDead = false;
    GameObject nearObject = null;
    float moveSpeed;    
    //bool ContinueAttack = false;
    #endregion [ �������� ]

    #region [ Property ]
    TalkManager talkManager { get { return TalkManager._talk; } }
    #endregion [ Property ]

    void Start()
    {
        Managers._input.KeyAction -= OnKeyBoardEvent;
        Managers._input.KeyAction += OnKeyBoardEvent;
        Managers._input.LeftMouseAction -= OnLeftMouseEvent;
        Managers._input.LeftMouseAction += OnLeftMouseEvent;
        Managers._input.RightMouseAction -= OnRightMouseEvent;
        Managers._input.RightMouseAction += OnRightMouseEvent;

        //Test
        Init();
    }

    void Update()
    {
       if(UI_Inventory.ActiveInventory == false)
            LookAround();
    }

    //���� ���� �� ���� �Ŵ������� ȣ��
    public void Init()
    {
        //���߿��� ������ �ε� 
        Stat.Init();        
    }

    void ChangeColor(Color color)
    {
        render.material.color = color;
    }

    //���콺 �ν��Ͽ� ī�޶� ȸ�� �� �÷��̾� ȸ��
    void LookAround()
    {        
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;
        if(x < 180)        
            x = Mathf.Clamp(x, -1, 70);        
        else        
            x = Mathf.Clamp(x, 335, 361);
                
        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    #region [ Key Event ]
    void OnKeyBoardEvent()
    {
        OnMoveEvent(Input.GetKey(KeyCode.LeftShift),Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        OnInteractEvent(Input.GetKeyDown(KeyCode.F));
        OnInventoryEvent(Input.GetKeyDown(KeyCode.I));        
    }

    void OnMoveEvent(bool btnDown,float horizontal, float vertical)
    {        
        Vector2 moveInput = new Vector2(horizontal, vertical);
        bool isMove = moveInput.sqrMagnitude != 0;
        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;            
            playerBody.forward = moveDir;

            moveSpeed = btnDown ? Stat.RunSpeed : Stat.MoveSpeed;
            if (btnDown)
            {
                //�޸��� �ִϸ��̼�
            }
            else
            {
                //�ȱ� �ִϸ��̼�
            }
            transform.position += moveSpeed * Time.deltaTime * moveDir;
        }        
    }

    void OnInteractEvent(bool btnDown)
    {
        if (btnDown == false)
            return;

        if (nearObject != null)
        {
            if (nearObject.TryGetComponent(out ObjectData objData))
            {
                if (objData.isNPC)
                {
                    talkManager.ShowText(nearObject, objData.objID, objData.name);
                }
                else
                {
                    if (nearObject.TryGetComponent(out BaseItem item))
                    {
                        if (item.Root())
                        {
                            nearObject = null;
                        }

                    }
                }

            }
        }
    }

    void OnInventoryEvent(bool btnDown)
    {
        if (btnDown == false)
            return;

        InventoryManager._inst.invenUI.TryOpenInventory();
    }
    #endregion [ Key Event ]


    #region [ Mouse Event ]
    //�̳��� ���� �۾�
    void OnLeftMouseEvent(MouseEvent evt)
    {
        switch (evt)
        {
            case MouseEvent.PointerDown:
                //ContinueAttack = true;
                EquipWeapon.OnWeaponUse(Stat.Damage);
                break;
            case MouseEvent.Press:

                break;
            case MouseEvent.PointerUp:
                //ContinueAttack = false;
                break;
        }
    }

    //�� �ϰų� ��Ŭ������ �Ҹ��Ѱ͵�
    void OnRightMouseEvent(MouseEvent evt)
    {

    }
    
    void OnMouseAction(MouseEvent evt)
    {

    }
    #endregion [ Mouse Event ]

    public void OnDamage(float damage)
    {
        if (isDead)
            return;

        isDead = Stat.GetHit(damage);
        StopCoroutine(OnDamageEvent());
        StartCoroutine(OnDamageEvent());
    }

    IEnumerator OnDamageEvent()
    {
        if (isDead)
        {
            //�׾����� �ൿ
        }
        ChangeColor(Color.gray);
        yield return new WaitForSeconds(0.3f);
        ChangeColor(Color.red);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            nearObject = other.gameObject;
        }


    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            nearObject = null;
            if (talkManager.talkUI.mainObject.activeSelf)
            {
                //��ȭ UI�� ����������� ����
                talkManager.ResetData();
            }
        }
    }
}
