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
        //�ӽ�
        target = null;

    }

    public void Execute(PetController m)
    {
        //��ȯ��ġ���� 
        if (m.Movement.CheckFarOffset())
        {
          //  m.ChangeState(MonsterStateReturn._inst);
            return;
        }

        if (m._targetMon != null) //  Ÿ���� �ִ�.
        {                     
            if (m.Movement.CheckCloseTarget(m.target.position, m.Stat.AttackRange)) // range �����Ұ�
            {
                m.transform.LookAt(m.target);
                // m.ChangeState(MonsterStateChase._inst);
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
            if (m.Movement.CheckCloseTarget(m.targetPos, 0.5f))
            {
                //��Ÿ�� ������
                //������ġ �޾ƿ���                
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
