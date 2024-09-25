using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerEquipCtrl : MonoBehaviour
{
    PlayerCtrl _manager;
    PlayerAssetsInputs _input;
    BowCtrl _myBow;

    public List<BaseWeaponCtrl> WeaponLists;
    Dictionary<int, BaseWeaponCtrl> _weapons;

    public int nowWeaponIndex;
    public int nextWeaponIndex;
    public WeaponType currWeaponType;
    public WeaponType prevWeaponType;

    int nowSlotIndex = 0;
    int changeSlotIndex = 0;
    int maxSlotCount = 0;
    bool isAniEnd = true;
    public int nowActiveLayer;

    public GameObject PetBallModel;
    public Transform BallPos;


    public PlayerAssetsInputs InputAsset { get { return _input; } }

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

    public void Init(PlayerCtrl manager, PlayerAssetsInputs input)
    {
        _manager = manager;
        _input = input;

        SettingWeapons();

        InitData();
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

    public void OnUpdate()
    {
        CheckScroll();

    }

    void CheckScroll()
    {
        if (_input.scrollY == 0.0f || isAniEnd == false)
        {
            //Debug.Log(changeSlotIndex);
            //Debug.Log(nowSlotIndex);
            return;
        }

        if (isAniEnd)
        {

            if (_input.scrollY > 0)
            {
                changeSlotIndex = ++changeSlotIndex > maxSlotCount ? 0 : changeSlotIndex;
            }
            else
            {
                changeSlotIndex = --changeSlotIndex < 0 ? maxSlotCount : changeSlotIndex;
            }

        }

        if (nowSlotIndex != changeSlotIndex)
        {
            nowSlotIndex = changeSlotIndex;
            OnNowWeapon(nowSlotIndex);
        }
        else
            isAniEnd = true;

    }

    void OnNowWeapon(int slotIndex)
    {
        isAniEnd = false;
        //인벤토리 핫슬롯에 등록되어있는 아이텡이 있을 경우 해당 아이템 Index 값 호출 없을 경우 0
        int weaponIndex = InventoryManager._inst.GetActiveWeaponIndex(slotIndex);
        if (weaponIndex > 0)
        {
            if (_weapons.ContainsKey(weaponIndex))
            {
                WeaponType newType = _weapons[weaponIndex].weaponType;
                bool isWear = nowWeaponIndex != weaponIndex;
                nowActiveLayer = isWear ? (int)newType : (int)WeaponType.None;
                ChangeWeapon(weaponIndex, newType, isWear);
            }
            else
            {
                Debug.Log("오류");
                isAniEnd = true;
            }
        }
        else
        {
            if (nowActiveLayer != (int)WeaponType.None)
            {
                nowActiveLayer = (int)WeaponType.None;
                ChangeWeapon(0, WeaponType.None, false);
            }
            else
            {
                isAniEnd = true;
            }
        }

    }

    void ChangeWeapon(int newWeaponIndex, WeaponType newType, bool isWear)
    {
        //순서 
        // 비무장 -> 1번 클릭시 1번 무기 장착 -> 장착 시 장착 애니메이션 실행
        // 레이어가 다르므로 레이어 가중치 1 설정
        // 장착 해제 혹은 변경 시 장착 해제 애니메이션 재생
        // 레이어 동일 해제 후 해당 레이어 가중치 0 주고 
        // 장착할 레이어 가중치 1 주고 애니메이션 실행
        if (isWear)
        {
            ChangeWeaponType(newType);
            if (prevWeaponType == WeaponType.None)
            {
                SetOnOffWeapon(false);
                nowWeaponIndex = newWeaponIndex;
                nextWeaponIndex = 0;
                _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)currWeaponType);
                _manager._anim.SetAnimation(ePlayerAnimParams.Equip);
            }
            else
            {
                nextWeaponIndex = newWeaponIndex;
                _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)prevWeaponType);
                _manager._anim.SetAnimation(ePlayerAnimParams.Disarm);
            }
        }
        else
        {
            nextWeaponIndex = 0;
            ChangeWeaponType(WeaponType.None);
            _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)prevWeaponType);
            _manager._anim.SetAnimation(ePlayerAnimParams.Disarm);
        }
    }

    void ChangeWeaponType(WeaponType newType)
    {
        prevWeaponType = currWeaponType;
        currWeaponType = newType;
    }

    void SetLayerWeight()
    {
        _manager._anim.SetAniLayerWeight((int)prevWeaponType, 0);
        _manager._anim.SetAniLayerWeight((int)currWeaponType, 1);
    }

    public void ReadyToGottcha(bool isDone)
    {
        if (_weapons[nowWeaponIndex].gameObject.activeSelf != isDone)
            _weapons[nowWeaponIndex].gameObject.SetActive(isDone);
    }

    //공격
    public void AttackAction()
    {
        _weapons[nowWeaponIndex].AttackAction();
    }

    public void EquipEvent()
    {
        SetLayerWeight();
        isAniEnd = true;
    }

    public void DisarmEvent()
    {
        //다르면 진행해줘야함
        if (nextWeaponIndex > 0)
        {
            nowWeaponIndex = nextWeaponIndex;
            nextWeaponIndex = 0;
            _manager._anim.SetAnimation(ePlayerAnimParams.WeaponType, (int)currWeaponType);
            _manager._anim.SetAnimation(ePlayerAnimParams.Equip);
        }
        else
        {
            SetLayerWeight();
            nowWeaponIndex = 0;
            SetOnOffWeapon(true);
            isAniEnd = true;
        }
    }

    public void SetOnOffWeapon(bool isOn)
    {
        _weapons[nowWeaponIndex].gameObject.SetActive(isOn);
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
}
