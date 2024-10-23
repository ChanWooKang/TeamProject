using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class TreeCtrl : HitObjectCtrl, IHitAble
{
    TreeComponent _tree;
    public override void OnDamage()
    {
        if (!isInit)
        {
            Init();
            _tree = GetComponent<TreeComponent>();
            _tree.Init(this);
        }


        if (isDead)
        {
            _tree.FallDownTree();
        }
    }

    public override void OnDeadEvent()
    {
        base.OnDeadEvent();
    }
}
