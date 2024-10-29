using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class TreeCtrl : HitObjectCtrl, IHitAble
{
    TreeComponent _tree;    

    public override void Init()
    {
        base.Init();
    }

    public override void OnDamage(float damage, Transform attacker, Vector3 hitPoint)
    {
        if (!isInit)
        {
            Init();
            _tree = GetComponent<TreeComponent>();
            _tree.Init(this);
        }

        base.OnDamage(damage, attacker, hitPoint);
        MakeParticleEffect(hitPoint);
    }

    public override void OnDamage()
    {        
        if (isDead)
        {
            _tree.FallDownTree();
        }
    }

    public override void OnDeadEvent()
    {
        base.OnDeadEvent();
    }

    public void ContactGround()
    {

    }
}
