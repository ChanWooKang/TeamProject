using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHandCtrl : BaseWeaponCtrl
{
    public override void AttackAction()
    {
        ShootRay();
    }
}
