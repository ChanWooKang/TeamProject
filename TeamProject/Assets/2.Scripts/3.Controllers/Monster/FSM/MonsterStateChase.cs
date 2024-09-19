using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateChase : TSingleton<MonsterStateChase>, IFSMState<MonsterController>
{
    public void Enter(MonsterController m)
    {
        m.BaseNavSetting();
        m.Agent.speed = m.Stat.RunSpeed;
        m.Agent.avoidancePriority = 50;
        m.State = eMonsterState.CHASE;       
        if (m._attackType == eAttackType.None)
            m.GetRangeByAttackType();
    }

    public void Execute(MonsterController m)
    {
        if (m._movement.CheckFarOffset())
        {
            m.ChangeState(MonsterStateReturn._inst);
            return;
        }

        if(m.target != null)
        {
            if (m._movement.CheckCloseTarget(m.target.position, m.Stat.ChaseRange))
            {
                m._movement.MoveFunc(m.target.position);
                if(m._movement.CheckCloseTarget(m.target.position,m.attackRange))
                {
                    m.ChangeState(MonsterStateAttack._inst);
                }
            }
            else
            {
                m.ChangeState(MonsterStateReturn._inst);
            }
        }
        else
        {            
            m.ChangeState(MonsterStateReturn._inst);
        }
    }

    public void Exit(MonsterController m)
    {

    }
}
