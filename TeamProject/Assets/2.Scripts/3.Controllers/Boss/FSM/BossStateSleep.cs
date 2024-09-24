using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStateSleep : TSingleton<BossStateSleep>, IFSMState<BossCtrl>
{
    public void Enter(BossCtrl m)
    {               
        m._move.AttackNavSetting();
        m.State = eBossState.SLEEP;
        m.OnRegenerate();
    }

    public void Execute(BossCtrl m)
    {

    }

    public void Exit(BossCtrl m)
    {
        m._move.BaseNavSetting();
    }
}
