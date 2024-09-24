using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateIdle : TSingleton<MonsterStateIdle>, IFSMState<MonsterController>
{
    float cntTime;
    public void Enter(MonsterController m)
    {
        m.Agent.speed = m.Stat.MoveSpeed;
        m.State = eMonsterState.IDLE;        
        cntTime = 0;               
    }

    public void Execute(MonsterController m)
    {
        if (!m.isStatic)
        {
            cntTime += Time.deltaTime;
            if (cntTime > m.delayTime)
            {
                m.targetPos = m._movement.GetRandomPos();
                m.ChangeState(MonsterStatePatrol._inst);
            }
            else
            {
                if (m.target != null)
                {
                    if (m._movement.CheckCloseTarget(m.target.position, m.Stat.ChaseRange))
                    {
                        m.ChangeState(MonsterStateChase._inst);
                        return;
                    }
                }

                if (m.State != eMonsterState.SENSE)
                {
                    m.State = eMonsterState.SENSE;
                }
            }
        }        
    }

    public void Exit(MonsterController m)
    {
        
    }
}
