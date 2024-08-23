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

    int activeIndex = -1;
    public bool isAttackEnd = true;
   
    public void Update()
    {
        // Test ���߿� �ű� ����
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
            //�ֽ��Կ� ���� ������ ���� �Ǿ������� 
            // ���� ���� �ҷ��ͼ� ���� �� �ش� ���� ��ȯ
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
            //�ֽ��Կ� ���������� ���� ��
            //���� ���·� ��ȯ
            //���� �����϶��� �ǳ� �ٱ�
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
        //���� 
        // ���� -> 1�� Ŭ���� 1�� ���� ���� -> ���� �� ���� �ִϸ��̼� ����
        // ���̾ �ٸ��Ƿ� ���̾� ����ġ 1 ����
        // ���� ���� Ȥ�� ���� �� ���� ���� �ִϸ��̼� ���
        // ���̾� ���� ���� �� �ش� ���̾� ����ġ 0 �ְ� 
        // ������ ���̾� ����ġ 1 �ְ� �ִϸ��̼� ����

        if (isEquip)
        {
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
        else
        {
            beforeWeaponType = nowWeaponType;
            nowWeaponType = WeaponType.None;
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Disarm);
        }
        
        manager.AnimCtrl.SetAnimations(ePlayerAnimParams.WeaponType,(int)nowWeaponType);
    }

    //����ִ� �ִϸ��̼� �������� 1������
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

    //������ �ִϸ��̼� ���� �� 1������
    public void EquipWeapon()
    {
        DisableWeapons(activeIndex, false);
    }
    

    
}
