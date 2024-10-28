using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetStateDizzy : TSingleton<PetStateDizzy>, IFSMState<PetController>
{
    float cntTime;
    float TestDizzyTime;
    public void Enter(PetController p)
    {
        p.AttackNavSetting();
        cntTime = 0;
        TestDizzyTime = 3.0f;
        p.isAttack = false;
        p.State = eMonsterState.DIZZY;
    }

    public void Execute(PetController p)
    {
        cntTime += Time.deltaTime;
        if (cntTime > TestDizzyTime)
        {
            p.ChangeState(PetStateChase._inst);
        }
    }

    public void Exit(PetController p)
    {

    }
}
