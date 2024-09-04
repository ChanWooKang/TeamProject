using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;


public class PetStateIdle : TSingleton<PetStateIdle>, IFSMState<PetController>
{
    float cntTime;
    public void Enter(PetController m)
    {
        m.Agent.speed = m.Stat.MoveSpeed;
        m.State = eMonsterState.IDLE;
        cntTime = 0;
    }

    public void Execute(PetController m)
    {
        if (!m.isStatic)
        {
            cntTime += Time.deltaTime;
            if (cntTime > m.delayTime)
            {
                m.targetPos = m.Movement.GetRandomPos();
            //    m.ChangeState(MonsterStatePatrol._inst);
            }
            else
            {
                if (m.target != null)
                {
                    if (m.Movement.CheckCloseTarget(m.target.position, m.Stat.AttackRange))
                    {
                      //  m.ChangeState(MonsterStateChase._inst);
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

    public void Exit(PetController m)
    {
        cntTime = 0;
    }
}
