using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossAnimCtrl : BaseAnimCtrl
{
    protected BossCtrl _manager;
    protected Animator _animator;

    public virtual void Init(BossCtrl manager)
    {
        _manager = manager;
        _animator = GetComponent<Animator>();
    }

    public virtual void ChangeAnimation()
    {

    }

}
