using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
public class PetStateAttack : TSingleton<PetStateAttack>, IFSMState<PetController>
{
    float cntTime;
    public void Enter(PetController m)
    {
        m.AttackNavSetting();
        cntTime = m.Stat.AttackDelay;
        m.State = eMonsterState.ATTACK;
    }
    public void Execute(PetController m)
    {
        if (m.target == null)
        {
            m.ChangeState(PetStateReturn._inst);
        }
        else
        {
            m.transform.LookAt(m.target);
            if (m.Movement.CheckCloseTarget(m.target.position, m.attackRange))
            {
                if (m.isAttack == false)
                    cntTime += Time.deltaTime;

                if (cntTime > m.Stat.AttackDelay && m.isAttack == false)
                {
                    cntTime = 0;
                    m.AttackFunc();
                }
            }
            else
            {
                if (m.isAttack == false)
                    m.ChangeState(PetStateChase._inst);
            }
        }
    }
    public void Exit(PetController m)
    {

    }
}
