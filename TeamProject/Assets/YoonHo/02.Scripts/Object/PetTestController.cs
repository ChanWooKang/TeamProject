using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetTestController : MonoBehaviour
{
    Vector3? m_targetPos = null;
    private void Update()
    {
        if (m_targetPos != null)
        {
            if (Vector3.Distance(transform.position, m_targetPos.Value) < 2 )
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
