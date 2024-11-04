using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHandCtrl : BaseWeaponCtrl
{
    [SerializeField] Collider _collider;
    public override void AttackAction()
    {
        if(_collider == null)
            _collider = GetComponent<Collider>();

        if (GameManagerEx._inst.playerManager._stat.CanUseStamina(_useStamina))
            _collider.enabled = true;
    }

    public override void AttackActionEnd()
    {
        if (_collider == null)
            _collider = GetComponent<Collider>();

        _collider.enabled = false;
    }

    public override bool CheckAttackAble()
    {
        return GameManagerEx._inst.playerManager._stat.CheckUseStamina(_useStamina);
    }
  
}
