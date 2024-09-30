using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnArmedCtrl : BaseWeaponCtrl
{
    public override bool CheckAttackAble()
    {
        return GameManagerEx._inst.playerManager._stat.CheckUseStamina(5);
    }

    public override void AttackAction()
    {
        if (GameManagerEx._inst.playerManager._stat.CanUseStamina(5))
            ShootRay();
    }    
}
