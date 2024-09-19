using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateDizzy : TSingleton<MonsterStateDizzy>, IFSMState<MonsterController>
{
    float cntTime;
    float TestDizzyTime;
    public void Enter(MonsterController m)
    {
        m.AttackNavSetting();
        cntTime = 0;
        TestDizzyTime = 3.0f;
        m.isAttack = false;
        m.State = eMonsterState.DIZZY;
    }

    public void Execute(MonsterController m)
    {
        cntTime += Time.deltaTime;
        if(cntTime > TestDizzyTime)
        {
            m.ChangeState(MonsterStateChase._inst);
        }
    }

    public void Exit(MonsterController m)
    {
        
    }
}
