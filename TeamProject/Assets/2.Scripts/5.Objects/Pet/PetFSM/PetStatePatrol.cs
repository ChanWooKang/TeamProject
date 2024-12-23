using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetStatePatrol : TSingleton<PetStatePatrol>, IFSMState<PetController>
{
    public void Enter(PetController m)
    {
        m.Agent.speed = m.Stat.MoveSpeed;
        m.Agent.avoidancePriority = 47;
        m.State = eMonsterState.PATROL;

    }

    public void Execute(PetController m)
    {
        //소환위치에서 
        if (m.Movement.CheckFarOffset())
        {
            m.ChangeState(PetStateReturn._inst);
            return;
        }

        if (m.target != null) //  타겟이 있다.
        {
            m.transform.LookAt(m.target);
            m.ChangeState(PetStateChase._inst);
            return;
        }
        else
        {
            if (m.Movement.CheckCloseTarget(m.player.position, 2))
                m.ChangeState(PetStateIdle._inst);
            else
                m.Movement.MoveFunc(m.player.position);
        }
    }

    public void Exit(PetController m)
    {

    }
}
