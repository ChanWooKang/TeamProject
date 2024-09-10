using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowCtrl : BaseWeaponCtrl
{    
    [SerializeField] Transform _arrowParent;
    [SerializeField] Transform ArrowPreview;
    Animator _animator;
    ArrowCtrl _myArrow;
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
            //애니메이터 전송 SetBool
            
        }
        _animator.SetBool(Animator.StringToHash("Charge"), _input.aim);
    }

    public override void Init(PlayerEquipCtrl player, float damage)
    {
        base.Init(player, damage);
        _animator = GetComponent<Animator>();
        _myArrow = null;
        _input = _playerEquip.InputAsset;
        isCharging = false;
        isReadyToFire = false;
    }

    public override void AttackAction()
    {
        if (_myArrow != null && isReadyToFire)
        {
            //_myArrow.FireArrow(_playerEquip.transform);
            _myArrow.gameObject.DestroyAPS();
            _myArrow = null;            
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
        if (_myArrow != null)
        {
            _myArrow.gameObject.DestroyAPS();
            _myArrow = null;
        }            
        _playerEquip.ChargeCancle();
    }

    public override void Reload()
    {
        if(_myArrow == null)
        {
            GameObject go = PoolingManager._inst.InstantiateAPS("Arrow", _arrowParent.position, _arrowParent.rotation, Vector3.one, _arrowParent);
            if (go.TryGetComponent(out _myArrow))
            {
                _myArrow.Init();                
            }
            else
            {
                Destroy(go);
                _myArrow = null;
                ChargeCancle();
            }
        }        
    }
    
    
}
