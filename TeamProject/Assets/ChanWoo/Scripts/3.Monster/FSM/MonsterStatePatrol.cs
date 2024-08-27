using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStatePatrol : TSingleton<MonsterStatePatrol>, IFSMState<MonsterController>
{

    public void Enter(MonsterController m)
    {
        m.BaseNavSetting();        
        m.Agent.speed = m.Stat.MoveSpeed;
        m.Agent.avoidancePriority = 47;       
        m.State = eMonsterState.PATROL;       
    }

    public void Execute(MonsterController m)
    {
        //소환위치에서 
        if (m._movement.CheckFarOffset())
        {
            m.ChangeState(MonsterStateReturn._inst);
            return;
        }

        if(m.target != null)
        {
            if(m._movement.CheckCloseTarget(m.target.position, m.Stat.Sight))
            {
                m.ChangeState(MonsterStateChase._inst);
            }
            else
            {
                if(m._movement.CheckCloseTarget(m.targetPos, 0.5f))
                {                    
                    m.ChangeState(MonsterStateIdle._inst);                    
                }
                else
                {
                    m._movement.MoveFunc(m.targetPos);
                }
            }
        }
        else
        {
            if (m._movement.CheckCloseTarget(m.targetPos, 0.5f))
            {
                //쿨타임 돌리고
                //랜덤위치 받아오기                
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
