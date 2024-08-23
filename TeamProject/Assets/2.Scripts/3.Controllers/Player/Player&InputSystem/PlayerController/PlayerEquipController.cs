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

    int activeIndex = -1;
    public bool isAttackEnd = true;
   
    public void Update()
    {
        // Test 나중에 옮길 예정
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnNowWeapon(1);            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnNowWeapon(2);
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
            //핫슬롯에 무기 정보가 저장 되어있을때 
            // 무기 정보 불러와서 대조 후 해당 무기 소환
            for (int i = 0; i < Weapons.Count; i++)
            {                
                if (Weapons[i].WeaponIndex == weaponIndex)
                {
                    bool isWear = (activeIndex != i);
                    if (isWear)
                        activeIndex = i;
                    else
                        activeIndex = -1;
                    ChangeWeapon(Weapons[i].WeaponType ,isWear);                                        
                }                
            }
        }
        else
        {
            //핫슬롯에 무기정보가 없을 때
            //비무장 상태로 전환
            //비무장 상태일때는 건너 뛰기
            if(nowWeaponType != WeaponType.None)
            {
                ChangeWeapon(WeaponType.None, false);
                activeIndex = -1;
            }                            
        }        
    }

    void DisableWeapons(int index = 0, bool isAll = true)
    {               
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (isAll)
            {
                Weapons[i].gameObject.SetActive(false);
            }
            else
            {
                if(i == index)
                {
                    Weapons[i].gameObject.SetActive(true);
                }
                else
                {
                    Weapons[i].gameObject.SetActive(false);
                }
            }
            
        }
    }

    public void ChangeWeapon(WeaponType newWeaponType , bool isEquip = true)
    {
        //순서 
        // 비무장 -> 1번 클릭시 1번 무기 장착 -> 장착 시 장착 애니메이션 실행
        // 레이어가 다르므로 레이어 가중치 1 설정
        // 장착 해제 혹은 변경 시 장착 해제 애니메이션 재생
        // 레이어 동일 해제 후 해당 레이어 가중치 0 주고 
        // 장착할 레이어 가중치 1 주고 애니메이션 실행

        if (isEquip)
        {
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
        else
        {
            beforeWeaponType = nowWeaponType;
            nowWeaponType = WeaponType.None;
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Disarm);
        }
        
        manager.AnimCtrl.SetAnimations(ePlayerAnimParams.WeaponType,(int)nowWeaponType);
    }

    //집어넣는 애니메이션 끝나기전 1프레임
    public void DisarmWeapon()
    {        
        DisableWeapons();                   
        if (beforeWeaponType != nowWeaponType)
        {
            if (beforeWeaponType != WeaponType.None)
                manager.AnimCtrl.SetAnimationLayerWeight(beforeWeaponType, 0);
            manager.AnimCtrl.SetAnimationLayerWeight(nowWeaponType, 1);
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Equip);
        }        
    }

    //꺼내는 애니메이션 시작 후 1프레임
    public void EquipWeapon()
    {
        DisableWeapons(activeIndex, false);
    }
    

    
}
