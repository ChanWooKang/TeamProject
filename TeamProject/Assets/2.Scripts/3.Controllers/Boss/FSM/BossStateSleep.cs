using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStateSleep : TSingleton<BossStateSleep>, IFSMState<BossCtrl>
{
    Transform target;
    public void Enter(BossCtrl m)
    {               
       
        m.State = eBossState.SLEEP;
        m.OnRegenerate();

        target = GameManagerEx._inst.playerManager.transform;
    }

    public void Execute(BossCtrl m)
    {
        if(m._move.CheckCloseTarget(target.position,m.Stat.Sight))
        {
            m.SetTarget();
            m.transform.LookAt(target.position);
            m.ChangeState(BossStateGrowl._inst);
        }
    }

    public void Exit(BossCtrl m)
    {
       
    }
}
