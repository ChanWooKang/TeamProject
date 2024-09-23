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
        m._move.AttackNavSetting();
        m.State = eBossState.DIE;
        m._render.ChangeLayer(eLayer.DisableObject);
        m.Agent.destination = transform.position;

    }

    public void Execute(BossCtrl m)
    {
        if(m.isActiveAndEnabled)
        {
            cntTime += Time.deltaTime;
            if(cntTime > m.delayTime)
            {
                m.OnDeadEvent();
            }
        }
    }

    public void Exit(BossCtrl m)
    {
        m._move.BaseNavSetting();
    }
}
