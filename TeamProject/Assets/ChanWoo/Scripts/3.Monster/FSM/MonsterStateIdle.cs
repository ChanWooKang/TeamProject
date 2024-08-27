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
            //�������϶� �÷��̾ Ÿ������ �����ϰ� ���� ����
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
                //IDLE�����϶� �ൿ�Ҹ��Ѱ� ���� ����
            }
        }
        
    }

    public void Exit(MonsterController m)
    {

    }
}
