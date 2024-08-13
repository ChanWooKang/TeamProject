using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStatePatrol : TSingleton<MonsterStatePatrol>, IFSMState<MonsterCtrl>
{
    float cntTime;

    public void Enter(MonsterCtrl m)
    {
        m.BaseNavSetting();
        m.targetPos = m._offSet;
        cntTime = 0;
        m.Agent.speed = m.Stat.MoveSpeed;
        m.Agent.avoidancePriority = 47;
        m.State = eMonsterState.PATROL;
    }

    public void Execute(MonsterCtrl m)
    {
        if(m.target != null)
        {
            if (m.IsCloseTarget(m.target.position, m.Stat.TraceRange))
                m.ChangeState(MonsterStateTrace._inst);
            else
            {
                if (m.IsCloseTarget(m.targetPos, 0.5f))
                {
                    cntTime += Time.deltaTime;
                    if(cntTime > m.delayTime)
                    {
                        cntTime = 0;
                        m.targetPos = m.GetRandomPos();
                    }
                    else
                    {
                        //���� �θ��� �Ÿ��ų� �ٸ� �ൿ ����
                    }
                }
                else
                {
                    if (m.State != eMonsterState.PATROL)
                        m.State = eMonsterState.PATROL;
                    m.MoveEvent(m.targetPos);
                }

            }
        }
        else
        {
            if (m.IsCloseTarget(m.targetPos, 0.5f))
            {
                cntTime += Time.deltaTime;
                if (cntTime > m.delayTime)
                {
                    cntTime = 0;
                    m.targetPos = m.GetRandomPos();
                }
                else
                {
                    //���� �θ��� �Ÿ��ų� �ٸ� �ൿ ����
                }
            }
            else
            {
                if (m.State != eMonsterState.PATROL)
                    m.State = eMonsterState.PATROL;
                m.MoveEvent(m.targetPos);
            }
        }
    }

    public void Exit(MonsterCtrl m)
    {
        
    }
}
