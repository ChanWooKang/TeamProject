using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetStateWork : TSingleton<PetStateWork>, IFSMState<PetController>
{
    float m_cntTime;
    public void Enter(PetController m)
    {
        m.Agent.speed = m.Stat.MoveSpeed;
        m_cntTime = m.Stat.AttackDelay;
        m.Agent.ResetPath();
        m.State = eMonsterState.WORK;        
    }
    public void Execute(PetController m)
    {
        if(m.target != null)
        {

        }
        else
        {
            if(Vector3.Distance(m.transform.position, m.m_workPos.Value) >= 2)
            {
                m.Movement.MoveFunc(m.m_workPos.Value);
                m.isworkReady = false;
            }
            else
            {
                m.Agent.ResetPath();
                m_cntTime += Time.deltaTime;
                if(m_cntTime >= 1)
                {
                    m_cntTime = 0;
                    m.WorkFunc();
                }
                
            }
        }
    }
    public void Exit(PetController m)
    {
     
    }
}
