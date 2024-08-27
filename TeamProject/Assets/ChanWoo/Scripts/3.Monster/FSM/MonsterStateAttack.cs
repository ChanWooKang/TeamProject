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
        cntTime = 0;
    }

    public void Execute(MonsterController m)
    {
        if(m.target == null)
        {
            m.ChangeState(MonsterStateReturn._inst);
        }
        else
        {
            if(m._movement.CheckCloseTarget(m.target.position,m.Stat.AttackRange))
            {
                cntTime += Time.deltaTime;
                if(cntTime > m.Stat.AttackDelay && m.isAttack == false)
                {
                    //°í¿Ë°Ý
                    cntTime = 0;
                    Debug.Log(m.Stat.AttackDelay);
                    Debug.Log("ATTACK");
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
