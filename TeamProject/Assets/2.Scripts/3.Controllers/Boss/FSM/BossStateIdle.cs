using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStateIdle : TSingleton<BossStateIdle>, IFSMState<BossCtrl>
{
    float cntTime;
    public void Enter(BossCtrl m)
    {
        m._move.AttackNavSetting();
        m.State = eBossState.IDLE;
        cntTime = 0;
    }

    public void Execute(BossCtrl m)
    {
        cntTime += Time.deltaTime;
        if(cntTime > m.delayTime)
        {
            m.targetPos = m._move.GetRandomPos();
            m.ChangeState(BossStatePatrol._inst);
        }
        else
        {
            if(m.target != null)
            {
                if(m._move.CheckCloseTarget(m.target.position,m.Stat.ChaseRange))
                {
                    m.ChangeState(BossStateChase._inst);
                    return;
                }
            }
        }
    }

    public void Exit(BossCtrl m)
    {
        m._move.BaseNavSetting();
    }
}
