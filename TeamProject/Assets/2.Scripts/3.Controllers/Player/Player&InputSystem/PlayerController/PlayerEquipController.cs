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
