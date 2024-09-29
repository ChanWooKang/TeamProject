using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
using UnityEngine.InputSystem;

public class PlayerEquipCtrl : MonoBehaviour
{
    InputActionAsset _assets;
    InputAction scrollActions;

    PlayerCtrl _manager;
    PlayerAssetsInputs _input;    
    BowCtrl _myBow;

    public List<BaseWeaponCtrl> WeaponLists;
    // Key = WeaponIndex , Value BaseWeaponCtrl
    Dictionary<int, BaseWeaponCtrl> _weapons;
    // Key  = SlotIndex,  Value = WeaponIndex
    Dictionary<int, int> _slotWeapons;           

    public GameObject PetBallModel;
    public Transform BallPos;       

    public PlayerAssetsInputs InputAsset { get { return _input; } }

    #region [ Init ]
    public void Init(PlayerCtrl manager, PlayerAssetsInputs input)
    {        
        _manager = manager;
        _input = input;

        SettingWeapons();
        SettingSlotWeapons();
        isProgress = false;
        InitData();        
    }

    void InitData()
    {        
        _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)WeaponType.None);
    }

    //최초 실행 시 weapon값 저장
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

    public void OnUpdate()
    {
        //ScrollAction();
    }

    public void ChangeSlotWeapon(int slotIndex, int weaponIndex)
    {
        if (_slotWeapons.ContainsKey(slotIndex))
        {
            _slotWeapons[slotIndex] = weaponIndex;
        }
    }

   

    [SerializeField] bool isProgress;
    // 슬롯 변경 인덱스 값
    [SerializeField] int changeSlotIndex = 0;
    // 현재 적용중인 슬롯 인덱스 값
    [SerializeField] int currSlotIndex = 0;
    // 현재 적용중인 무기 인덱스 값
    [SerializeField] int currWeaponIndex = 0;
    // 변경 할 무기 인덱스 값
    [SerializeField] int changeWeaponIndex = 0;


    private void Awake()
    {
        _assets = GetComponent<PlayerInput>().actions;
        scrollActions = _assets.FindAction("Scroll");
    }

    private void OnEnable()
    {
        scrollActions.performed += ScrollAction;
        scrollActions.Enable();
    }

    private void OnDisable()
    {
        scrollActions.performed -= ScrollAction;
        scrollActions.Disable();
    }

    public void ScrollAction(InputAction.CallbackContext context)
    {      
        float scrollValue = context.ReadValue<float>();        

        if (scrollValue > 0)
        {
            changeSlotIndex = (changeSlotIndex + 1) % _slotWeapons.Count;
        }
        else if (scrollValue < 0)
        {
            changeSlotIndex = (changeSlotIndex - 1 + _slotWeapons.Count) % _slotWeapons.Count;
        }



        if (isProgress == false && !_manager._anim.GetAnimation(ePlayerAnimParams.AcivateAnimation))
        {            
            ChangeWeapon(changeSlotIndex);
        }        
            
    }
    

    void ChangeWeapon(int slotIndex)
    {
        isProgress = true;
        if (_slotWeapons.ContainsKey(slotIndex))
        {
            if(slotIndex == currSlotIndex)
            {
                isProgress = false;
                return;
            }

            //변경하려는 인덱스와 현재 적용 중인 인덱스가 동일할경우 패스
            if (_slotWeapons[slotIndex] == _slotWeapons[currSlotIndex])
            {
                isProgress = false;                
                Debug.Log("동일");
                return;
            }


            //변경 할려는 슬롯에 WeaponIndex가 저장이 안되어있으면 패스 *오류*
            if (!_weapons.ContainsKey(_slotWeapons[slotIndex]))
            {
                Debug.Log("오류");
                isProgress = false;
                return;
            }


            //현재 무기 정보 재 확인
            currWeaponIndex = _slotWeapons[currSlotIndex];
            //변경할 무기 정보 기입
            changeWeaponIndex = _slotWeapons[slotIndex];            

            if (_weapons[currWeaponIndex].weaponType == WeaponType.None)
            {
                //현재 장착 무기 상태가 비 무장 일때 장착 진행
                ChangeAnimation(true);
            }
            else
            {
                //현재 장착 무기 상태가 무장 상태 일때 해제 진행
                ChangeAnimation(false);
            }
        }
        else
            isProgress = false;
    }

    public void EquipChangeEnd()
    {        
        if(changeWeaponIndex == _slotWeapons[changeSlotIndex])
        {
            SetLayerWeight();
            currWeaponIndex = changeWeaponIndex;
            currSlotIndex = changeSlotIndex;

            for (int i = 0; i < _weapons.Count; i++)
            {
                WeaponLists[i].gameObject.SetActive(WeaponLists[i].Index == currWeaponIndex);
            }

            isProgress = false;
        }
        else
        {
            ChangeWeapon(changeSlotIndex);
        }
    }

    public void DisarmEvent()
    {
        if(changeWeaponIndex > 0)
        {
            //해제 후 장착
            ChangeAnimation(true);
        }
        else
        {            
            //해제 후 정리 작업            
            EquipChangeEnd();            
        }
    }

    void ChangeAnimation(bool isEquip)
    {
        if (isEquip)
        {            
            _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)_weapons[changeWeaponIndex].weaponType);            
            _manager._anim.SetAnimation(ePlayerAnimParams.Equip);
        }
        else
        {
            _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)_weapons[currWeaponIndex].weaponType);
            _manager._anim.SetAnimation(ePlayerAnimParams.Disarm);
        }
    }
    
    void SetLayerWeight()
    {
        WeaponType nowType = _weapons[currWeaponIndex].weaponType;
        WeaponType changeType = _weapons[changeWeaponIndex].weaponType;

        if(nowType != changeType)
        {
            _manager._anim.SetAniLayerWeight((int)nowType, 0);
            _manager._anim.SetAniLayerWeight((int)changeType, 1);
        }        
    }

    public void ChangeNextWeaponActive(bool isOn)
    {
        _weapons[changeWeaponIndex].gameObject.SetActive(isOn);
    }

    public void ChangeNowWeaponActive(bool isOn)
    {
        _weapons[currWeaponIndex].gameObject.SetActive(isOn);
    }    

    public void ReadyToGottcha(bool isDone)
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

    //공격
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
        go.GetComponent<PetBallController>().ShootEvent();
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
        ReadyToGottcha(true);
    }


}
