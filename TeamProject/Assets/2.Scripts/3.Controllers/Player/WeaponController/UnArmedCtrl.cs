using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnArmedCtrl : BaseWeaponCtrl
{
    public override void AttackAction()
    {
        ShootRay();
    }    
}
