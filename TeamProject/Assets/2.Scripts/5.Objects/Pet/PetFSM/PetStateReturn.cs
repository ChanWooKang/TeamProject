using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetStateReturn : TSingleton<PetStateReturn>, IFSMState<PetController>
{
    public void Enter(PetController m)
    {
        m.Agent.speed = m.Stat.MoveSpeed * 5;
        m.State = eMonsterState.RETURN;
        m.BaseNavSetting();
    }
    public void Execute(PetController m)
    {
        if (m.Movement.CheckCloseTarget(m.Movement._offsetPos, 0.5f))
        {
            m.SetTarget(null, false);
            m.ChangeState(PetStateIdle._inst);
        }
        else
        {
            m.Movement.MoveFunc(m.Movement._offsetPos);
        }
    }
    public void Exit(PetController m)
    {

    }
}
