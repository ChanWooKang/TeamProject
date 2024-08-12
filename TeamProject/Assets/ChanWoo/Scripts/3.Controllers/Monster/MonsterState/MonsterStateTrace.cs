using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateTrace : TSingleton<MonsterStateTrace>, IFSMState<MonsterCtrl>
{
    public void Enter(MonsterCtrl m)
    {
        m.BaseNavSetting();
        m.Agent.speed = m.Stat.TraceSpeed;
        m.Agent.avoidancePriority = 50;
        m.State = eMonsterState.TRACE;
    }

    public void Execute(MonsterCtrl m)
    {
        if(m.target != null)
        {
            if (m.IsCloseTarget(m.target.position, m.Stat.TraceRange))
            {
                m.MoveEvent(m.target.position);
                if (m.IsCloseTarget(m.target.position, m.Stat.AttackRange))
                    m.ChangeState(MonsterStateAttack._inst);
            }
            else
            {
                m.ChangeState(MonsterStatePatrol._inst);
            }
        }
        else
        {
            m.ChangeState(MonsterStatePatrol._inst);
        }
    }

    public void Exit(MonsterCtrl m)
    {
        
    }
}
