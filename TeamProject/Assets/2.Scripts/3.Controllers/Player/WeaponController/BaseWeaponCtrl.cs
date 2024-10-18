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
    protected int _weaponLevel;
    protected float _minDist = 1.0f;
    [SerializeField] protected float _weaponRange = 0f;
    [SerializeField] protected float _useStamina = 5.0f;

    protected int _nowBulletCnt;
    protected int _maxBulletCnt;

    //Particle Level System
    [SerializeField] protected LevelEffectCtrl _effectScript;

    public int NowBulletCount { get { return _nowBulletCnt; } }
    public int MaxBulletCount { get { return _maxBulletCnt; } }

    #region [ Property ]
    public int Index { get { return index; } }
    public WeaponType weaponType { get { return _type; } }
    public WeaponItemInfo weaponInfo { get { return _weaponStat; } }
    protected float PlayerDamage { get { return GameManagerEx._inst.playerManager._stat.Damage; } }
    protected float TotalDamage { get { return _weaponDamage + PlayerDamage; } }
    #endregion [ Property ]

    public virtual void Init(PlayerEquipCtrl player, float damage)
    {
        _playerEquip = player;
        if (_effectScript != null)
            _effectScript.Init();
        ChangeWeaponData();

        gameObject.SetActive(false);
    }

    void ChangeStat(WeaponItemInfo info = null)
    {
        _weaponStat = info;
        if (info != null)
        {
            _weaponDamage = info.Damage;
            _weaponLevel = info.Level;
        }
        else
        {
            _weaponDamage = 0;
            _weaponLevel = 0;
        }

    }

    public void ChangeWeaponData()
    {
        Dictionary<int, WeaponItemInfo> datas = InventoryManager._inst.Dict_Weapon;

        if (datas.TryGetValue(index, out WeaponItemInfo info))
        {
            ChangeStat(info);
            if (_effectScript != null)
                _effectScript.ChangeIndex(_weaponLevel);
        }
    }
    public void SetAmmo()
    {
        _maxBulletCnt = weaponInfo.Ammo;
        _nowBulletCnt = weaponInfo.CurrentAmmo;
    }
    public void ChangeParticleState(bool isOn)
    {
        if (_effectScript != null)
        {
            _effectScript.PlayOrStop(isOn);
        }
    }

    public abstract void AttackAction();

    protected virtual void ShootRay()
    {
        if (Physics.SphereCast(FirePos.position, RayRadius, FirePos.forward, out RaycastHit rhit, _minDist + _weaponRange, AcceptLayer))
        {
            if (rhit.transform.TryGetComponent(out IHitAble hit))
            {
                if (hit.CheckAttackType(_type))
                    hit.OnDamage(TotalDamage, _playerEquip.transform, rhit.point);
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
