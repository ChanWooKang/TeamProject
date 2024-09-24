using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStateChase : TSingleton<BossStateChase>, IFSMState<BossCtrl>
{
    public void Enter(BossCtrl m)
    {
        m.Agent.speed = m.Stat.RunSpeed;
        m.Agent.avoidancePriority = 50;
        m.State = eBossState.CHASE;
        if (m._attackType == eAttackType.None)
            m.GetRangeByAttackType();

        
    }

    public void Execute(BossCtrl m)
    {
        if (m._move.CheckFarOffset())
        {
            Return(m);
            return;
        }

        if (m.target != null)
        {
            if(m._move.CheckCloseTarget(m.target.position,m.Stat.ChaseRange))
            {
                m._move.MoveFunc(m.target.position);
                if (m._move.CheckCloseTarget(m.target.position, m.attackRange))
                    m.ChangeState(BossStateAttack._inst);
            }
            else
            {
                m.targetPos = m._move._offsetPos;
                m._patrolCount = 0;
                m.ChangeState(BossStatePatrol._inst);
                return;
            }
        }
        else
            Return(m);
    }

    public void Exit(BossCtrl m)
    {

    }


    void Return(BossCtrl m)
    {
        m.ChangeState(BossStateReturn._inst);
    }
}
