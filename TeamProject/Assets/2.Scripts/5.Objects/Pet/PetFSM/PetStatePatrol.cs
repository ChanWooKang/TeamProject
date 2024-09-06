using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetStatePatrol : TSingleton<PetStatePatrol>,IFSMState<PetController>
{
    Transform target;
    public void Enter(PetController m)
    {
        m.Agent.speed = m.Stat.MoveSpeed;
        m.Agent.avoidancePriority = 47;
        m.State = eMonsterState.PATROL;
        //임시
        target = null;

    }

    public void Execute(PetController m)
    {
        //소환위치에서 
        if (m.Movement.CheckFarOffset())
        {
          //  m.ChangeState(MonsterStateReturn._inst);
            return;
        }

        if (m.target != null)
        {           
            //if(m._movement.CheckCloseTarget(m.target.position, m.Stat.Sight))
            if (m.Movement.CheckCloseTarget(m.target.position, m.Stat.AttackRange)) // range 수정할것
            {
                m.transform.LookAt(m.target);
               // m.ChangeState(MonsterStateChase._inst);
                return;
            }
            else
            {
                if (m.Movement.CheckCloseTarget(m.targetPos, 0.5f))
                {
              //      m.ChangeState(MonsterStateIdle._inst);
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
            if (GameManagerEx._inst.playeManager.isDead == false)
            {
                //m.Stat.Sight
                //if (m._movement.CheckCloseTarget(target.position, m.Stat.Sight))
               // if (m.Movement.CheckCloseTarget(target.position, m.Stat.Sight))
               // {
               //     m.SetTarget(target, true);
               //     m.transform.LookAt(target);
               ////     m.ChangeState(MonsterStateSense._inst);
               // }
            }
            if (m.Movement.CheckCloseTarget(m.targetPos, 0.5f))
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
