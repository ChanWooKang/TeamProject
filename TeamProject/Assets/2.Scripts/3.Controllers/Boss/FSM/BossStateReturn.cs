using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStateReturn : TSingleton<BossStateReturn>, IFSMState<BossCtrl>
{
    public void Enter(BossCtrl m)
    {
        m.Agent.speed = m.Stat.MoveSpeed * 5f;
        m.State = eBossState.RETURN;
        
    }

    public void Execute(BossCtrl m)
    {
        if (m._move.CheckCloseTarget(m._move._offsetPos, 0.5f))
        {
            m.SetTarget(true);
            m._anim.SleepAction();
        }
        else
        {
            m._move.MoveFunc(m._move._offsetPos);
        }
    }

    public void Exit(BossCtrl m)
    {
        
    }
}
