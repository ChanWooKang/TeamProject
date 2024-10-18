using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class RifleCtrl : BaseWeaponCtrl
{

    [SerializeField] ParticleSystem _muzzle;
    

    public override void Init(PlayerEquipCtrl player, float damage)
    {
        base.Init(player, damage);        
        _nowBulletCnt = _maxBulletCnt;
        
    }

    public override bool CheckAttackAble()
    {
        bool isAble = _nowBulletCnt > 0;

        return isAble;
    }

    protected override void ShootRay()
    {
        if (Physics.Raycast(FirePos.position, Camera.main.transform.forward,
            out RaycastHit rhit, _weaponRange, AcceptLayer)) 
        {
            if(rhit.transform.TryGetComponent(out IHitAble hit))
            {
                if(hit.CheckAttackType(_type))
                    hit.OnDamage(TotalDamage, _playerEquip.transform, rhit.point);
            }            
        }
    }

    public override void AttackAction()
    {
        //¸ÓÁñ ÀÌÆåÆ®,
        if (CheckAttackAble() == false)
            return;

       
        _muzzle.Play(true);
        ShootRay();
        --_nowBulletCnt;
        weaponInfo.CurrentAmmo = _nowBulletCnt;
        InventoryManager._inst.weaponUI.ShootWeapon(_nowBulletCnt);
    }

    public override void Reload()
    {
        int BcountLeft = InventoryManager._inst.GetItemCount(weaponInfo.ShotIndex);        
      
        if (BcountLeft > 0 && BcountLeft < _maxBulletCnt)
        {
            InventoryManager._inst.UseItem(weaponInfo.ShotIndex, BcountLeft);
            _nowBulletCnt = BcountLeft;
        }
        else if(BcountLeft >= _maxBulletCnt)
        {
            InventoryManager._inst.UseItem(weaponInfo.ShotIndex, _maxBulletCnt);
            _nowBulletCnt = _maxBulletCnt;
        }
        else if(BcountLeft == 0)
        {
            
        }
    }
}
