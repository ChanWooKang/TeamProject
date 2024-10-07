using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
public class PetStateChase : TSingleton<PetStateChase>, IFSMState<PetController>
{
    public void Enter(PetController m)
    {
        m.BaseNavSetting();
        m.Agent.speed = m.Stat.RunSpeed;
        m.Agent.avoidancePriority = 50;
        m.State = eMonsterState.CHASE;
        if (m._attackType == eAttackType.None)
            m.GetRangeByAttackType();
    }

    public void Execute(PetController m)
    {
        //if (m.Movement.CheckFarOffset())
        //{
        //    m.ChangeState(PetStateReturn._inst);
        //    return;
        //}

        if (m.target != null)
        {
            m.Movement.MoveFunc(m.target.position);
            if (m.Movement.CheckCloseTarget(m.target.position, m.attackRange))
            {
                m.ChangeState(PetStateAttack._inst);
            }
        }
        else
        {
            m.ChangeState(PetStateIdle._inst);
        }
    }
    public void Exit(PetController m)
    {

    }
}
