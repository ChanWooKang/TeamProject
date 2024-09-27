using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStateAttack : TSingleton<BossStateAttack>, IFSMState<BossCtrl>
{
    float cntTime = 0;
    public void Enter(BossCtrl m)
    {
        m._move.AttackNavSetting();
        m.State = eBossState.IDLE;
        cntTime = m.Stat.AttackDelay;
    }

    public void Execute(BossCtrl m)
    {
        if(m.target == null)
        {
            if (GameManagerEx._inst.playerManager.isDead)
                m.ChangeState(BossStateReturn._inst);
            else
            {
                m.targetPos = m._move._offsetPos;
                m._patrolCount = 0;
                m.ChangeState(BossStatePatrol._inst);
                return;
            }                
        }
        else
        {
            m.transform.LookAt(m.target);
            if (m._move.CheckCloseTarget(m.target.position, m.attackRange))
            {
                if (m.isAttack == false)
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
                    m.ChangeState(BossStateChase._inst);
            }
        }
    }

    public void Exit(BossCtrl m)
    {
        
    }
}
