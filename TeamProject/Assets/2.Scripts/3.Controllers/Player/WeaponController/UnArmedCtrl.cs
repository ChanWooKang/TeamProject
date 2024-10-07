using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnArmedCtrl : BaseWeaponCtrl
{
    public override bool CheckAttackAble()
    {
        return GameManagerEx._inst.playerManager._stat.CheckUseStamina(_useStamina);
    }

    public override void AttackAction()
    {
        if (GameManagerEx._inst.playerManager._stat.CanUseStamina(_useStamina))
            ShootRay();
    }    
}
