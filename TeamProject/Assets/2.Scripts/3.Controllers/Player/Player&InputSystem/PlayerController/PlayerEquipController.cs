using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerEquipController : MonoBehaviour
{
    PlayerManager manager;

    public WeaponItemInfo ActiveWeapon;
    //총, 활
    public GameObject LeftWeaponParent;
    // 한손무기 , 볼 , 화살
    public GameObject RightWeaponParent;

    public List<WeaponCtrl> Weapons;
    

    //Test
    public BowCtrl bow;
    public WeaponType nowWeaponType;
    WeaponType beforeWeaponType;

    public bool isAttackEnd = true;
   
    public void Update()
    {
        // Test 나중에 옮길 예정
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnNowWeapon(1);
            ChangeWeapon(WeaponType.Bow);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(WeaponType.None);
        }
    }

    public void Init(PlayerManager _manager)
    {
        manager = _manager;
        nowWeaponType = WeaponType.None;
        beforeWeaponType = WeaponType.None;

        SettingWeaponList();
    }

    void SettingWeaponList()
    {
        Weapons = new List<WeaponCtrl>();
        int i;
        Transform target;
        if(LeftWeaponParent.transform.childCount > 0)
        {
            target = LeftWeaponParent.transform;
            for(i = 0; i < target.childCount; i++)
            {
                Weapons.Add(target.GetChild(i).GetComponent<WeaponCtrl>());
            }
        }

        if (RightWeaponParent.transform.childCount > 0)
        {
            target = RightWeaponParent.transform;
            for (i = 0; i < target.childCount; i++)
            {
                Weapons.Add(target.GetChild(i).GetComponent<WeaponCtrl>());
            }
        }
    }

    //slotIndex 1, 2, 3, 4
    void OnNowWeapon(int index)
    {        
        int weaponIndex = InventoryManager._inst.GetActiveWeaponIndex(index, manager);
        if(weaponIndex > 0)
        {
            bool isNowWeapon;
            for (int i = 0; i < Weapons.Count; i++)
            {
                isNowWeapon = false;
                if (Weapons[i].WeaponIndex == weaponIndex)
                {
                    isNowWeapon = true;
                    ChangeWeapon(Weapons[i].WeaponType, i);
                }
                Weapons[i].gameObject.SetActive(isNowWeapon);
            }
        }
        else
        {
            for(int i = 0; i < Weapons.Count; i++)
            {
                Weapons[i].gameObject.SetActive(false);
                ChangeWeapon(WeaponType.None, 0);
            }
        }        
    }

    void DisableWeapons()
    {

    }

    public void ChangeWeapon(WeaponType newWeaponType , int listIndex = 0)
    {
        //순서 
        // 비무장 -> 1번 클릭시 1번 무기 장착 -> 장착 시 장착 애니메이션 실행
        // 레이어가 다르므로 레이어 가중치 1 설정
        // 장착 해제 혹은 변경 시 장착 해제 애니메이션 재생
        // 레이어 동일 해제 후 해당 레이어 가중치 0 주고 
        // 장착할 레이어 가중치 1 주고 애니메이션 실행
        
        if (nowWeaponType == newWeaponType)
        {
            // 해제
            beforeWeaponType = nowWeaponType;
            nowWeaponType = WeaponType.None;
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Disarm);
        }
        else
        {
            //장착
            if (nowWeaponType == WeaponType.None)
            {
                //해제를 할게 없으니 바로 장착 진행
                beforeWeaponType = nowWeaponType;
                nowWeaponType = newWeaponType;
                manager.AnimCtrl.SetAnimationLayerWeight(nowWeaponType, 1);
                manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Equip);
            }
            else
            {

                beforeWeaponType = nowWeaponType;
                nowWeaponType = newWeaponType;
                manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Disarm);
            }
        }
        manager.AnimCtrl.SetAnimations(ePlayerAnimParams.WeaponType,(int)nowWeaponType);
    }

    public void DisarmWeapon()
    {
        if(beforeWeaponType != WeaponType.None)
            manager.AnimCtrl.SetAnimationLayerWeight(beforeWeaponType, 0);

        if (beforeWeaponType != nowWeaponType)
        {
            manager.AnimCtrl.SetAnimationLayerWeight(beforeWeaponType, 0);
            manager.AnimCtrl.SetAnimationLayerWeight(nowWeaponType, 1);
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Equip);
        }        
    }

    
    public void SetEquipItemLayer()
    {
        
    }
    

    
}
