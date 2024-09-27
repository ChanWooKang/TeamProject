using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
using UnityEngine.InputSystem;

public class PlayerEquipCtrl : MonoBehaviour
{
    PlayerCtrl _manager;
    PlayerAssetsInputs _input;    
    BowCtrl _myBow;

    public PlayerInput playerInput;
    InputAction scrollAction;

    public List<BaseWeaponCtrl> WeaponLists;
    Dictionary<int, BaseWeaponCtrl> _weapons;

    // Key  = SlotIndex,  Value = WeaponIndex
    Dictionary<int, int> _slotWeapons;

    public int nowWeaponIndex;
    public int nextWeaponIndex;
    public WeaponType currWeaponType;
    public WeaponType prevWeaponType;

    int _currentSlotIndex;
    int _activeSlotIndex;
    int maxSlotCount = 0;

    bool isProgress = false;

    public int nowActiveLayer;

    public GameObject PetBallModel;
    public Transform BallPos;    

    public int MaxSlotCount
    {
        get { return maxSlotCount; }
        set
        {
            if (value >= 0)
                maxSlotCount = ++maxSlotCount > 4 ? 4 : maxSlotCount;
            else
                maxSlotCount = --maxSlotCount < 0 ? 0 : maxSlotCount;
        }
    }

    public PlayerAssetsInputs InputAsset { get { return _input; } }


    public void Init(PlayerCtrl manager, PlayerAssetsInputs input)
    {
        scrollAction = playerInput.actions.FindAction("WeaponSelect");
        _manager = manager;
        _input = input;

        SettingWeapons();
        SettingSlotWeapons();
        isProgress = false;
        InitData();
        _currentSlotIndex = 0;
    }

    void InitData()
    {
        nowWeaponIndex = 0;
        nextWeaponIndex = 0;
        currWeaponType = WeaponType.None;
        prevWeaponType = currWeaponType;
        nowActiveLayer = 1;
        SetOnOffWeapon(true);
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

    public void ChangeSlotWeapon(int slotIndex, int weaponIndex)
    {
        if (_slotWeapons.ContainsKey(slotIndex))
        {
            _slotWeapons[slotIndex] = weaponIndex;
        }
    }

    void OnScroll(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();

        if(scrollValue > 0)
        {
            _currentSlotIndex = (_currentSlotIndex + 1) % _slotWeapons.Count;
        }
        else if (scrollValue < 0)
        {
            _currentSlotIndex = (_currentSlotIndex - 1 + _slotWeapons.Count) % _slotWeapons.Count;
        }

        if(!isProgress)
            ChangeWeapon(_currentSlotIndex);
    }

    void ChangeWeapon(int slotIndex)
    {
        isProgress = true;

        
        if(_slotWeapons.ContainsKey(slotIndex))
        {
            if (_slotWeapons[slotIndex] == _slotWeapons[_activeSlotIndex])
            {
                //변경하려는 인덱스와 현재 적용 중인 인덱스가 동일할경우 패스
                isProgress = false;
                return;
            }

            //현재 적용되고 있는 ActiveSlotIndex 내용물 확인 비무장 , 장비 착용 확인해서 Disarm / Equip 구분
            // 값이 0 일경우 비무장
            if (_slotWeapons[_activeSlotIndex] == 0)
            {
                //장착
                Debug.Log("장착");
                _activeSlotIndex = slotIndex;
                isProgress = false;
                return;
            }
            else
            {                
                if (_slotWeapons[slotIndex] == 0) 
                {
                    //해제
                    Debug.Log("해제");
                    _activeSlotIndex = 0;
                    isProgress = false;
                    return;
                }
                else
                {
                    //장착 및 해제
                    Debug.Log("장착 및 해제");
                    _activeSlotIndex = slotIndex;
                    isProgress = false;
                    return;
                }
            }
        }

        isProgress = false;
    }





    

    

    public void SetOnOffWeapon(bool isOn)
    {        
        _weapons[nowWeaponIndex].gameObject.SetActive(isOn);
    }

    public void ReadyToGottcha(bool isDone)
    {
        if (_weapons[nowWeaponIndex].gameObject.activeSelf != isDone)
            _weapons[nowWeaponIndex].gameObject.SetActive(isDone);
    }

    public bool CheckAttackAble()
    {
        bool isAble = _weapons[nowWeaponIndex].CheckAttackAble();
        _manager._anim.SetAnimation(ePlayerAnimParams.AttackAble, isAble);
        return isAble;
    }

    //공격
    public void AttackAction()
    {
        _weapons[nowWeaponIndex].AttackAction();
    }    

    public void ChargeStart()
    {
        _weapons[nowWeaponIndex].ChargeStart();
        _manager._anim.SetAnimation(ePlayerAnimParams.Charge, true);
        _manager._anim.SetAnimation(ePlayerAnimParams.AttackEnd, false);
    }

    public void ChargeEnd()
    {
        _weapons[nowWeaponIndex].ChargeEnd();
        _manager._anim.SetAnimation(ePlayerAnimParams.Charge, false);
    }

    public void ChargeCancle()
    {
        _manager._anim.SetAnimation(ePlayerAnimParams.Charge, false);
        _manager._anim.SetAnimation(ePlayerAnimParams.AttackEnd, true);
    }

    public void Reload()
    {
        _weapons[nowWeaponIndex].Reload();
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

    private void OnEnable()
    {
        if(scrollAction != null)
        {
            scrollAction.performed += OnScroll;
            scrollAction.Enable();
        }
    }

    private void OnDisable()
    {
        if(scrollAction != null)
        {
            scrollAction.performed -= OnScroll;
            scrollAction.Disable();
        }        
    }
}
