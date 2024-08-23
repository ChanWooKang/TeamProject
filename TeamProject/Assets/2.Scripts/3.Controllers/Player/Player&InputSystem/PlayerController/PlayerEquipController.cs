using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerEquipController : MonoBehaviour
{
    PlayerManager manager;

    public WeaponItemInfo ActiveWeapon;
    //��, Ȱ
    public GameObject LeftWeaponParent;
    // �Ѽչ��� , �� , ȭ��
    public GameObject RightWeaponParent;

    public List<WeaponCtrl> Weapons;
    

    //Test
    public BowCtrl bow;
    public WeaponType nowWeaponType;
    WeaponType beforeWeaponType;

    public bool isAttackEnd = true;
   
    public void Update()
    {
        // Test ���߿� �ű� ����
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
        //���� 
        // ���� -> 1�� Ŭ���� 1�� ���� ���� -> ���� �� ���� �ִϸ��̼� ����
        // ���̾ �ٸ��Ƿ� ���̾� ����ġ 1 ����
        // ���� ���� Ȥ�� ���� �� ���� ���� �ִϸ��̼� ���
        // ���̾� ���� ���� �� �ش� ���̾� ����ġ 0 �ְ� 
        // ������ ���̾� ����ġ 1 �ְ� �ִϸ��̼� ����
        
        if (nowWeaponType == newWeaponType)
        {
            // ����
            beforeWeaponType = nowWeaponType;
            nowWeaponType = WeaponType.None;
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Disarm);
        }
        else
        {
            //����
            if (nowWeaponType == WeaponType.None)
            {
                //������ �Ұ� ������ �ٷ� ���� ����
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
