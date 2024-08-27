using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    MonsterStat m_petStat;
    
    public MonsterStat PetStat { get { return m_petStat; } }
    Vector3? m_targetPos = null;

    private void Awake()
    {
        // юс╫ц
        m_petStat = new MonsterStat();
        m_petStat.Init();
    }
    private void Update()
    {
        if (m_targetPos != null)
        {
            if (Vector3.Distance(transform.position, m_targetPos.Value) < 2)
            {
                m_targetPos = null;
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, m_targetPos.Value, 10f * Time.deltaTime);
        }
    }


    public void MoveToObject(Vector3 targetPos)
    {
        m_targetPos = targetPos;
    }
   

}
