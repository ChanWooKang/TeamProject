using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowCtrl : BaseWeaponCtrl
{
    [SerializeField] GameObject _arrowModel;
    [SerializeField] Transform _arrowPoint;
    Animator _animator;    
    PlayerAssetsInputs _input;
    bool isCharging;
    bool isReadyToFire;    

    void Update()
    {
        OnCharging();
    }

    void OnCharging()
    {
        if (isCharging)
        {
            if (_input.aim == false)
            {
                isCharging = false;
                isReadyToFire = false;
                ChargeCancle();
            }                        
        }                
        _animator.SetBool(Animator.StringToHash("Charge"), _input.aim);
    }

    public override void Init(PlayerEquipCtrl player, float damage)
    {
        base.Init(player, damage);
        _animator = GetComponent<Animator>();        
        _input = _playerEquip.InputAsset;
        isCharging = false;
        isReadyToFire = false;
        _arrowModel.SetActive(false);
    }

    public override void AttackAction()
    {
        if (isReadyToFire)
        {
            _arrowModel.SetActive(false);
            GameObject go = PoolingManager._inst.InstantiateAPS("Arrow", _arrowPoint.transform.position, _arrowPoint.rotation, Vector3.one);            
            go.GetComponent<ArrowCtrl>().ShootArrow(_playerEquip.transform, TotalDamage);
            isReadyToFire = false;
        }            
    }

    public override void ChargeStart()
    {
        //애니메이션 
        _animator.SetTrigger(Animator.StringToHash("ChargeStart"));
        isCharging = true;
        isReadyToFire = false;
        
    }

    public override void ChargeEnd()
    {
        isCharging = false;
        isReadyToFire = true;
    }

    void ChargeCancle()
    {
        _arrowModel.SetActive(false);     
    }

    public override void Reload()
    {
        _arrowModel.SetActive(true);        
    }

}
