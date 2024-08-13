using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateAttack : TSingleton<MonsterStateAttack>, IFSMState<MonsterCtrl>
{
    float cntTime;
    public void Enter(MonsterCtrl m)
    {
        m.AttackNavSetting();
        cntTime = 0;
    }

    public void Execute(MonsterCtrl m)
    {
        if (m.target == null)
            m.ChangeState(MonsterStatePatrol._inst);
        else
        {
            m.LookPlayer();
            if (m.IsCloseTarget(m.target.position, m.Stat.AttackRange))
            {
                cntTime += Time.deltaTime;
                if (cntTime > m.Stat.AttackDelay && m.isAttack == false)
                    m.AttackEvent();
            }
            else
            {
                if (m.isAttack == false)
                    m.ChangeState(MonsterStateTrace._inst);
            }
        }
    }

    public void Exit(MonsterCtrl m)
    {
        
    }
}
