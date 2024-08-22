using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerEquipController : MonoBehaviour
{
    PlayerManager manager;

    //Test
    public WeaponType nowWeaponType;
    WeaponType beforeWeaponType;
    

    public void Init(PlayerManager _manager)
    {
        manager = _manager;
        nowWeaponType = WeaponType.None;
        beforeWeaponType = WeaponType.None;        
        
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(WeaponType.Bow);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(WeaponType.None);
        }
    }

    public void ChangeWeapon(WeaponType newWeaponType)
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

    public void EquipWeapon()
    {

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

    
    

    
}
