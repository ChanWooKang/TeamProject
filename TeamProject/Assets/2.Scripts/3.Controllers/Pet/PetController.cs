using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetController : FSM<PetController>
{      
    NavMeshAgent m_agent;
    Animator m_animator;
    
    CapsuleCollider m_collider;
    Rigidbody m_rigid;

    MonsterInfo m_petInfo;
    

    public MonsterInfo PetInfo { get { return m_petInfo; } }
    Vector3? m_targetPos = null;

    private void Awake()
    {
        //юс╫ц
        InitPet(1000);
        //
    }  
    public void MoveToObject(Vector3 targetPos)
    {
        m_targetPos = targetPos;
        StartCoroutine(MovePetToTarget());
    }

    public void InitPet(int index)
    {
        m_petInfo = Managers._data.Dict_Monster[index];
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<CapsuleCollider>();
        m_rigid = GetComponent<Rigidbody>();
    }
   







    IEnumerator MovePetToTarget()
    {
        while (Vector3.Distance(transform.position, m_targetPos.Value) >= 2)
        {
            if (m_targetPos != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_targetPos.Value, m_petInfo.Speed * Time.deltaTime);
            }
        }
        m_targetPos = null;
        yield return null;
    }
}
