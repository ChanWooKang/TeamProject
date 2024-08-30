using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStatePatrol : TSingleton<MonsterStatePatrol>, IFSMState<MonsterController>
{
    Transform target;
    public void Enter(MonsterController m)
    {        
        m.Agent.speed = m.Stat.MoveSpeed;
        m.Agent.avoidancePriority = 47;       
        m.State = eMonsterState.PATROL;
        target = GameManagerEx._inst.playeManager.transform;

    }

    public void Execute(MonsterController m)
    {
        //��ȯ��ġ���� 
        if (m._movement.CheckFarOffset())
        {
            m.ChangeState(MonsterStateReturn._inst);
            return;
        }

        if(m.target != null)
        {
            //�÷��̾� Ÿ�� ������ ���߿� üũ 
            //if(m._movement.CheckCloseTarget(m.target.position, m.Stat.Sight))
            if (m._movement.CheckCloseTarget(m.target.position, m.Stat.ChaseRange))
            {
                m.transform.LookAt(m.target);
                m.ChangeState(MonsterStateChase._inst);
                return;
            }
            else
            {                
                if(m._movement.CheckCloseTarget(m.targetPos, 0.5f))
                {                    
                    m.ChangeState(MonsterStateIdle._inst);
                    return;
                }
                else
                {
                    m._movement.MoveFunc(m.targetPos);
                }
            }
        }
        else
        {
            if(GameManagerEx._inst.playeManager.isDead == false)
            {
                //m.Stat.Sight
                //if (m._movement.CheckCloseTarget(target.position, m.Stat.Sight))
                if (m._movement.CheckCloseTarget(target.position, 20))
                {                    
                    m.SetTarget(target,true);
                    m.transform.LookAt(target);
                    m.ChangeState(MonsterStateSense._inst);
                }                
            }            
            if (m._movement.CheckCloseTarget(m.targetPos, 0.5f))
            {
                //��Ÿ�� ������
                //������ġ �޾ƿ���                
                m.ChangeState(MonsterStateIdle._inst);
            }
            else
            {
                m._movement.MoveFunc(m.targetPos);
            }
        }
    }

    public void Exit(MonsterController m)
    {

    }
}
