using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PickaxeCtrl : BaseWeaponCtrl
{
    public override void AttackAction()
    {
        if (GameManagerEx._inst.playerManager._stat.CanUseStamina(5))
            ShootRay();
    }

    public override bool CheckAttackAble()
    {
        return GameManagerEx._inst.playerManager._stat.CheckUseStamina(5);
    }
}
