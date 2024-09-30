using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class BaseWeaponCtrl : MonoBehaviour
{
    //Components
    [SerializeField] protected Transform FirePos;

    protected PlayerEquipCtrl _playerEquip;
    protected WeaponItemInfo _weaponStat;

    //Ray
    [SerializeField, Range(0.1f, 0.5f)] protected float RayRadius;
    [SerializeField] protected LayerMask AcceptLayer;

    //Datas
    [SerializeField] int index;
    [SerializeField] protected WeaponType _type;
    protected float _weaponDamage;
    protected float _minDist = 1.0f;
    [SerializeField] protected float _weaponRange = 0f;

    public int Index { get { return index; } }
    public WeaponType weaponType { get { return _type; } }

    protected float PlayerDamage { get { return GameManagerEx._inst.playerManager._stat.Damage; } }
    protected float TotalDamage { get { return _weaponDamage + PlayerDamage; } }

    public virtual void Init(PlayerEquipCtrl player, float damage)
    {
        _playerEquip = player;        
        GetWeaponData();
        
        gameObject.SetActive(false);
    }

    protected virtual void GetWeaponData()
    {
        Dictionary<int, WeaponItemInfo> datas = InventoryManager._inst.Dict_Weapon;
        if (datas.ContainsKey(index))
        {
            _weaponStat = datas[index];
            _weaponDamage = _weaponStat.Damage;            
            return;
        }
        else
            _weaponDamage = 0;
    }


    public abstract void AttackAction();

    protected virtual void ShootRay()
    {
        if (Physics.SphereCast(FirePos.position, RayRadius, FirePos.forward, out RaycastHit rhit, _minDist+_weaponRange, AcceptLayer))
        {
            if (rhit.transform.TryGetComponent(out MonsterController mc))
            {
                Debug.Log(rhit.point);
                mc.OnDamage(TotalDamage, _playerEquip.transform, rhit.point);
            }
            else if(rhit.transform.TryGetComponent(out BossCtrl boss))
            {
                boss.OnDamage(TotalDamage, _playerEquip.transform, rhit.point);
            }

        }
    }

    public virtual bool CheckAttackAble()
    {
        return true;
    }
    
    public virtual void ChargeStart()
    {

    }


    public virtual void ChargeEnd()
    {

    }

    public virtual void Reload()
    {

    }
}
