using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossStatePatrol : TSingleton<BossStatePatrol>, IFSMState<BossCtrl>
{
    Transform target;
    public void Enter(BossCtrl m)
    {
        m._move.BaseNavSetting();
        m.Agent.speed = m.Stat.MoveSpeed;
        m.Agent.avoidancePriority = 47;
        m.State = eBossState.PATROL;        
    }

    public void Execute(BossCtrl m)
    {
        if (m._move.CheckFarOffset())
        {
            Return(m);
            return;
        }

        if(m._patrolCount >= m._maxPatrolCount)
        {
            Return(m);
            return;
        }
        else
        {
            if(m.target != null)
            {
                if(m._move.CheckCloseTarget(m.target.position,m.Stat.ChaseRange))
                {
                    m.transform.LookAt(m.target);
                    Chase(m);
                    return;
                }
                else
                {
                    if(m._move.CheckCloseTarget(m.targetPos, 0.5f))
                    {
                        Idle(m);
                        return;
                    }
                    else
                    {
                        m._move.MoveFunc(m.targetPos);
                    }
                }
            }
            else
            {
                if(GameManagerEx._inst.playerManager.isDead == false)
                {
                    target = GameManagerEx._inst.playerManager.transform;
                    if (m._move.CheckCloseTarget(target.position, m.Stat.Sight))
                    {
                        m.SetTarget(false);
                        m.transform.LookAt(target);
                        Growl(m);
                        return;
                    }                    
                }
                else
                {
                    Return(m);
                }
            }
        }
    }

    public void Exit(BossCtrl m)
    {
        
    }

    void Return(BossCtrl m)
    {
        m.ChangeState(BossStateReturn._inst);
        m._patrolCount = 0;
    }

    void Chase(BossCtrl m)
    {
        m.ChangeState(BossStateChase._inst);
        m._patrolCount = 0;
    }

    void Idle(BossCtrl m)
    {
        m.ChangeState(BossStateIdle._inst);
        m._patrolCount++;
    }

    void Growl(BossCtrl m)
    {
        m.ChangeState(BossStateGrowl._inst);
        m._patrolCount = 0;
    }
}
