using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterStateSense : TSingleton<MonsterStateSense>, IFSMState<MonsterController>
{
    float cntTime;
    float angleRange = 30f;
    bool isCollision;
    public void Enter(MonsterController m)
    {
        cntTime = 0;
        m.State = eMonsterState.SENSE;
        m.AttackNavSetting();
        isCollision = false;
    }

    public void Execute(MonsterController m)
    {
        cntTime += Time.deltaTime;
        if(cntTime > m.delayTime)
        {
            if (isCollision)
            {
                m.ChangeState(MonsterStateChase._inst);
            }
            else
            {
                m.SetTarget(null, false);
                //���� �ð� ���� �� �ٽ� ����
                m.ChangeState(MonsterStatePatrol._inst);
            }
            
        }
        else
        {
            Vector3 interVec = m.target.position - m.transform.position;

            //m.Stat.ChaseRange
            if (interVec.sqrMagnitude <= Mathf.Pow(10, 2))
            {
                //�νİŸ� �� Ÿ�� ���� ( �÷��̾� )
                float dot = Vector3.Dot(interVec.normalized, m.transform.forward);
                float theta = Mathf.Acos(dot);
                float degree = Mathf.Rad2Deg * theta;

                if (degree <= angleRange / 2)
                {
                    isCollision = true;
                }
                else
                {
                    isCollision = false;
                }
                
            }       
            else
            {
                isCollision = false;
            }
        }
    }

    public void Exit(MonsterController m)
    {
        m.BaseNavSetting();
    }
}
