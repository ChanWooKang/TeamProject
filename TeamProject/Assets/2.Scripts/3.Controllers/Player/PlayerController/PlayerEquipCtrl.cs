using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
using UnityEngine.InputSystem;

public class PlayerEquipCtrl : MonoBehaviour
{
    InputActionAsset _assets;
    InputAction numberInputAction;

    PlayerCtrl _manager;
    PlayerAssetsInputs _input;        

    public List<BaseWeaponCtrl> WeaponLists;
    // Key = WeaponIndex , Value BaseWeaponCtrl
    Dictionary<int, BaseWeaponCtrl> _weapons;
    // Key  = SlotIndex,  Value = WeaponIndex
    Dictionary<int, int> _slotWeapons;           

    public GameObject PetBallModel;
    public GameObject HammerModel;
    public Transform BallPos;

    // ���� ���� �ε��� ��
    [SerializeField] int changeSlotIndex = 0;
    // ���� �������� ���� �ε��� ��
    [SerializeField] int currSlotIndex = 0;
    // ���� �������� ���� �ε��� ��
    [SerializeField] int currWeaponIndex = 0;
    // ���� �� ���� �ε��� ��
    [SerializeField] int changeWeaponIndex = 0;


    public PlayerAssetsInputs InputAsset { get { return _input; } }

    #region [ Init ]
    private void Awake()
    {
        _assets = GetComponent<PlayerInput>().actions;
        numberInputAction = _assets.FindAction("NumberInput");
    }
    private void OnEnable()
    {
        numberInputAction.performed += OnNumberInputs;
        numberInputAction.Enable();
    }

    private void OnDisable()
    {
        numberInputAction.performed -= OnNumberInputs;
        numberInputAction.Disable();
    }
    public void Init(PlayerCtrl manager, PlayerAssetsInputs input)
    {        
        _manager = manager;
        _input = input;

        SettingWeapons();
        SettingSlotWeapons();        
        InitData();
        PetBallModel.SetActive(false);
        HammerModel.SetActive(false);       
    }

    void InitData()
    {        
        _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)WeaponType.None);
    }

    //���� ���� �� weapon�� ����
    void SettingWeapons()
    {
        _weapons = new Dictionary<int, BaseWeaponCtrl>();
        int i;
        for (i = 0; i < WeaponLists.Count; i++)
        {
            if (_weapons.ContainsKey(WeaponLists[i].Index) == false)
            {
                _weapons.Add(WeaponLists[i].Index, WeaponLists[i]);
                WeaponLists[i].Init(this, _manager._stat.Damage);
                WeaponLists[i].gameObject.SetActive(WeaponLists[i].Index == 0);
            }
        }
    }

    void SettingSlotWeapons()
    {
        _slotWeapons = new Dictionary<int, int>();
        
        for (int i = 1; i < 5; i++)
        {
            int weaponIndex = InventoryManager._inst.GetActiveWeaponIndex(i);
            _slotWeapons.Add(i, weaponIndex);
        }
        _slotWeapons.Add(0, 0);
    }

    #endregion [ Init ]

    public void ChangeSlotWeapon(int slotIndex, int weaponIndex)
    {
        if (_slotWeapons.ContainsKey(slotIndex))
        {       
            if(weaponIndex > 0)
            {
                //��� ����
                _slotWeapons[slotIndex] = weaponIndex;
                OnNowWeapon(slotIndex);
            }
            else
            {
                //��� ����
                if (currSlotIndex == slotIndex)
                {
                    _slotWeapons[slotIndex] = weaponIndex;
                    OnNowWeapon(slotIndex);
                }
            }
        }
    }   

    public void OnNumberInputs(InputAction.CallbackContext context) 
    {
        var control = context.control;
        if(control != null)
        {
            string keyPressed = control.displayName;
            int pressedNumber = int.Parse(keyPressed);

            if (!_manager._anim.GetAnimation(ePlayerAnimParams.AcivateAnimation))
                OnNowWeapon(pressedNumber);
        }
    }

    //��ư Ŭ���� ����
    void OnNowWeapon(int slotIndex)
    {        
        changeSlotIndex = slotIndex;
        changeWeaponIndex = _slotWeapons[changeSlotIndex];        
        if(changeSlotIndex == currSlotIndex)
        {
            if(currWeaponIndex == 0)
            {
                return;
            }
            else
            {
                //���� -> ���� ���� �ε����� 0���� ��ȯ
                changeSlotIndex = 0;
                changeWeaponIndex = 0;
                ChangeAnimation(false);
            }
        }
        else
        {
            if (currWeaponIndex == 0)
            {
                //���� ����
                if (changeWeaponIndex == 0)
                {
                    //���� -> ����
                    //���� �ε����� ����
                    currSlotIndex = changeSlotIndex;
                    return;
                }
                else
                {
                    //���� -> ����                
                    //��� ���� �ִϸ��̼�
                    ChangeAnimation(true);
                }
            }
            else
            {
                //�ϴ� ��� ����
                ChangeAnimation(false);
            }
        }        
    }

    void SetLayerWeight()
    {
        WeaponType nowType = _weapons[currWeaponIndex].weaponType;
        WeaponType changeType = _weapons[changeWeaponIndex].weaponType;

        if (nowType != changeType)
        {
            _manager._anim.SetAniLayerWeight((int)nowType, 0);
            _manager._anim.SetAniLayerWeight((int)changeType, 1);
        }
    }

    void ChangeAnimation(bool isWear)
    {
        WeaponType type;
        if (isWear)
        {
            type = _weapons[changeWeaponIndex].weaponType;
            _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)type);
            _manager._anim.SetAnimation(ePlayerAnimParams.Equip);
        }
        else
        {
            type = _weapons[currWeaponIndex].weaponType;
            _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)type);
            _manager._anim.SetAnimation(ePlayerAnimParams.Disarm);
        }
    }

    void ChangeEnd()
    {
        SetLayerWeight();
        currWeaponIndex = changeWeaponIndex;
        currSlotIndex = changeSlotIndex;        
    }

    public void EquipEvent()
    {
        ChangeEnd();
    }

    public void DisarmEvent()
    {
        if(changeWeaponIndex > 0)
        {
            // ��� ���� ����
            ChangeAnimation(true);
        }
        else
        {
            ChangeEnd();
        }
    }

    public void EquipActiveWeapon()
    {
        for (int i = 0; i < WeaponLists.Count; i++)
        {
            WeaponLists[i].gameObject.SetActive(WeaponLists[i].Index == changeWeaponIndex);
        }
    }

    public void DisarmActiveWeapon()
    {
        for(int i = 0; i < WeaponLists.Count; i++)
        {
            WeaponLists[i].gameObject.SetActive(false);
        }        
    }
              
    

    #region [ Animation Event ]
    

    public void ReadyToAnimAction(bool isDone)
    {
        if (_weapons[currWeaponIndex].gameObject.activeSelf != isDone)
            _weapons[currWeaponIndex].gameObject.SetActive(isDone);
    }

    public bool CheckAttackAble()
    {
        bool isAble = _weapons[currWeaponIndex].CheckAttackAble();
        _manager._anim.SetAnimation(ePlayerAnimParams.AttackAble, isAble);
        return isAble;
    }

    //����
    public void AttackAction()
    {
        _weapons[currWeaponIndex].AttackAction();
    }    

    public void ChargeStart()
    {
        _weapons[currWeaponIndex].ChargeStart();
        _manager._anim.SetAnimation(ePlayerAnimParams.Charge, true);
        _manager._anim.SetAnimation(ePlayerAnimParams.AttackEnd, false);
    }

    public void ChargeEnd()
    {
        _weapons[currWeaponIndex].ChargeEnd();
        _manager._anim.SetAnimation(ePlayerAnimParams.Charge, false);
    }

    public void ChargeCancle()
    {
        _manager._anim.SetAnimation(ePlayerAnimParams.Charge, false);
        _manager._anim.SetAnimation(ePlayerAnimParams.AttackEnd, true);
    }

    public void Reload()
    {
        _weapons[currWeaponIndex].Reload();
    }


    public void GeneratePetBall()
    {
        PetBallModel.SetActive(true);
    }

    public void ThrowBall()
    {
        BallPos.rotation = Quaternion.LookRotation(GetDirection());
        GameObject go = PoolingManager._inst.InstantiateAPS("PetBall", BallPos.position, BallPos.rotation, Vector3.one * 0.2f);        
        go.GetComponent<PetBallController>().ShootEvent(Camera.main.transform.forward);
    }

    Vector3 GetDirection()
    {
        Vector3 xzVec = BallPos.forward * -1;
        float y = Camera.main.transform.forward.y;
        return new Vector3(xzVec.x, y, xzVec.z);
    }

    public void ThrowEnd()
    {
        PetBallModel.SetActive(false);
        ReadyToAnimAction(true);
    }

    #endregion [ Animation Event ]

}
