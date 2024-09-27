using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateDie : TSingleton<MonsterStateDie>, IFSMState<MonsterController>
{
    float cntTime;
    public void Enter(MonsterController m)
    {
        cntTime = 0;
        //m.AttackNavSetting();
        m.State = eMonsterState.DIE;
        m.ChangeLayer(eLayer.DisableObject);        
        //m.Agent.destination = transform.position;
    }

    public void Execute(MonsterController m)
    {
        if (m.isActiveAndEnabled)
        {
            cntTime += Time.deltaTime;
            if (cntTime > m.delayTime)
                m.OnDeadEvent();
        }
    }

    public void Exit(MonsterController m)
    {

    }
}
