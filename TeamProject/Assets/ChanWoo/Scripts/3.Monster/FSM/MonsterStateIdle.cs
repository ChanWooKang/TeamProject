using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateIdle : TSingleton<MonsterStateIdle>, IFSMState<MonsterController>
{
    public void Enter(MonsterController m)
    {
        m.State = eMonsterState.IDLE;
        if(m.Stat.CharacterType > 0)
        {
            //공격적일때 
            m.SetTarget();
        }
    }

    public void Execute(MonsterController m)
    {

    }

    public void Exit(MonsterController m)
    {

    }
}
