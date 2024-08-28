using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateIdle : TSingleton<MonsterStateIdle>, IFSMState<MonsterController>
{
    float cntTime;
    public void Enter(MonsterController m)
    {
        m.State = eMonsterState.IDLE;
        cntTime = 0;               
    }

    public void Execute(MonsterController m)
    {
        if (m.isStatic == false)
        {
            cntTime += Time.deltaTime;
            if (cntTime > m.delayTime)
            {
                cntTime = 0;
                m.SetTarget();
                m.targetPos = m._movement.GetRandomPos();                
                m.ChangeState(MonsterStatePatrol._inst);
                
            }
            else
            {                
                if (m.State != eMonsterState.SENSE)
                {
                    Debug.Log("Sense");
                }                    
            }
        }        
    }

    public void Exit(MonsterController m)
    {

    }
}
