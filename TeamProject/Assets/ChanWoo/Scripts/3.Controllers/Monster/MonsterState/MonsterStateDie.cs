using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateDie : TSingleton<MonsterStateDie>, IFSMState<MonsterCtrl>
{
    float cntTime;
    public void Enter(MonsterCtrl m)
    {
        m.AttackNavSetting();
        m.Agent.destination = m.transform.position;
        m.State = eMonsterState.DEAD;
        cntTime = 0;
    }

    public void Execute(MonsterCtrl m)
    {
        if (m.isActiveAndEnabled)
        {
            cntTime += Time.deltaTime;
            if (cntTime > m.delayTime)
                m.OnDeadEvent();
        }
    }

    public void Exit(MonsterCtrl m)
    {
        
    }
}
