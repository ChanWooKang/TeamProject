using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class RifleCtrl : BaseWeaponCtrl
{

    [SerializeField] ParticleSystem _muzzle;
    [SerializeField] int _nowBulletCnt;
    [SerializeField] int _maxBulletCnt;
    
    public int NowBulletCount { get { return _nowBulletCnt; } }
    public int MaxBulletCount { get { return _maxBulletCnt; } }

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
        //∏”¡Ò ¿Ã∆Â∆Æ,
        if (CheckAttackAble() == false)
            return;

        _muzzle.Play(true);
        ShootRay();
        _nowBulletCnt--;
    }

    public override void Reload()
    {
        _nowBulletCnt = _maxBulletCnt;
    }
}
