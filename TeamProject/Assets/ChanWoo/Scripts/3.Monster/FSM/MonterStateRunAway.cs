using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonterStateRunAway : TSingleton<MonterStateRunAway>, IFSMState<MonsterController>
{    
    Vector3 GoalPos;
    public void Enter(MonsterController m)
    {        
        m.BaseNavSetting();
        m.Agent.speed = m.Stat.RunSpeed * 0.5f;
        GoalPos = m.GetRunAwayPos();        
        m.State = eMonsterState.CHASE;
    }

    public void Execute(MonsterController m)
    {        
        if (m._movement.CheckCloseTarget(GoalPos, 0.5f))
        {
            m.ChangeState(MonsterStatePatrol._inst);
        }
        else
        {
            m._movement.MoveFunc(GoalPos);
        }
       
    }

    public void Exit(MonsterController m)
    {
        m._animCtrl.AttackEnd();
    }
}
