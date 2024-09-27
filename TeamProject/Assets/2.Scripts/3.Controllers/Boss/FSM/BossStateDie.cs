using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStateDie : TSingleton<BossStateDie> ,IFSMState<BossCtrl>
{
    float cntTime;
    public void Enter(BossCtrl m)
    {
        cntTime = 0;        
        m.State = eBossState.DIE;
        m._render.ChangeLayer(eLayer.DisableObject);       

    }

    public void Execute(BossCtrl m)
    {
        if(m.isActiveAndEnabled)
        {
            cntTime += Time.deltaTime;
            if(cntTime > 2.0f)
            {
                m.OnDeadEvent();
            }
        }
    }

    public void Exit(BossCtrl m)
    {        
    }
}
