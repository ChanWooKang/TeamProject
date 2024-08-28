using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateReturn : TSingleton<MonsterStateReturn>, IFSMState<MonsterController>
{
    public void Enter(MonsterController m)
    {
        m.Agent.speed = m.Stat.MoveSpeed * 5;
        m.State = eMonsterState.RETURN;
        m.BaseNavSetting();
    }

    public void Execute(MonsterController m)
    {
        if (m._movement.CheckCloseTarget(m._movement._offsetPos, 0.5f))
        {
            m.ChangeState(MonsterStateIdle._inst);
        }
        else
        {
            m._movement.MoveFunc(m._movement._offsetPos);
        }
    }

    public void Exit(MonsterController m)
    {

    }
}
