using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStateGrowl : TSingleton<BossStateGrowl>, IFSMState<BossCtrl>
{
    public void Enter(BossCtrl m)
    {
        m._move.AttackNavSetting();
        m.State = eBossState.GROWL;        
    }

    public void Execute(BossCtrl m)
    {

    }

    public void Exit(BossCtrl m)
    {
        m._move.BaseNavSetting();
    }
}
