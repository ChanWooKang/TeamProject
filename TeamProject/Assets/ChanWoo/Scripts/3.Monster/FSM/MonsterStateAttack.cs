using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateAttack : TSingleton<MonsterStateAttack>, IFSMState<MonsterController>
{
    float cntTime;
    public void Enter(MonsterController m)
    {
        m.AttackNavSetting();
        cntTime = m.Stat.AttackDelay;
        m.State = eMonsterState.IDLE;
        
    }

    public void Execute(MonsterController m)
    {
        if(m.target == null)
        {
            m.ChangeState(MonsterStateReturn._inst);
        }
        else
        {
            m.transform.LookAt(m.target);
            if(m._movement.CheckCloseTarget(m.target.position,m.attackRange))
            {
                if(m.isAttack == false)
                    cntTime += Time.deltaTime;

                if(cntTime > m.Stat.AttackDelay && m.isAttack == false)
                {                    
                    cntTime = 0;
                    m.AttackFunc();
                }
            }
            else
            {
                if (m.isAttack == false)
                    m.ChangeState(MonsterStateChase._inst);
            }
        }
    }

    public void Exit(MonsterController m)
    {

    }
}
