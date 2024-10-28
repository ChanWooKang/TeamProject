using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetStateDie : TSingleton<PetStateDie>, IFSMState<PetController>
{
    float cntTime;
    public void Enter(PetController p)
    {
        cntTime = 0;
        p.AttackNavSetting();
        p.State = eMonsterState.DIE;
        p.ChangeLayer(eLayer.DisableObject);
        p.Agent.destination = transform.position;
    }

    public void Execute(PetController p)
    {
        if (p.isActiveAndEnabled)
        {
            cntTime += Time.deltaTime;
            if (cntTime > 2f)
                p.OnDeadEvent();
        }
    }

    public void Exit(PetController p)
    {

    }
}
