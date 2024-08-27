using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateIdle : TSingleton<MonsterStateIdle>, IFSMState<MonsterController>
{
    float cntTime;
    public void Enter(MonsterController m)
    {
        m.State = eMonsterState.IDLE;
        cntTime = 1.5f;

        if (m.Stat.CharacterType > 0)
        {
            //공격적일때 플레이어를 타겟으로 설정하고 순찰 시작
            m.SetTarget();
        }
    }

    public void Execute(MonsterController m)
    {
        if (m.Stat.CharacterType > 0 || m.isStatic == false)
        {
            cntTime += Time.deltaTime;
            if (cntTime > m.delayTime)
            {
                cntTime = 0;
                m.targetPos = m._movement.GetRandomPos();
                m.ChangeState(MonsterStatePatrol._inst);
                m.SetTarget();
            }
            else
            {
                //IDLE상태일때 행동할만한거 진행 가능
            }
        }
        
    }

    public void Exit(MonsterController m)
    {

    }
}
