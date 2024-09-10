using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetStatePatrol : TSingleton<PetStatePatrol>,IFSMState<PetController>
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

        if (m._targetMon != null) //  타겟이 있다.
        {                     
            if (m.Movement.CheckCloseTarget(m.target.position, m.Stat.AttackRange)) // range 수정할것
            {
                m.transform.LookAt(m.target);
                m.ChangeState(PetStateChase._inst);
                return;
            }
            else
            {
                if (m.Movement.CheckCloseTarget(m.targetPos, 0.5f))
                {
                    m.ChangeState(PetStateIdle._inst);
                    return;
                }
                else
                {
                    m.Movement.MoveFunc(m.targetPos);
                }
            }
        }
        else
        {            
            if (m.Movement.CheckCloseTarget(m.targetPos, 0.5f) || m.Movement.CheckCloseTarget(m.player.position, 1.2f))
            {
                //쿨타임 돌리고
                //랜덤위치 받아오기                
                 m.ChangeState(PetStateIdle._inst);
            }
            else
            {                
                m.Movement.MoveFunc(m.targetPos);
            }
        }
    }

    public void Exit(PetController m)
    {

    }
}
