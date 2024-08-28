using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBallController : MonoBehaviour
{    
    MonsterController m_targetMonsterCtrl;
    PetBallInfo m_petBallInfo;
    GameObject m_player;

    float m_bonusCaptureRate;

    float m_targetMaxhp;
    float m_targetCurrenthp;
    float m_targetCaptureRate;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pet"))
        {
            //Æ÷È¹
            m_player = GameObject.FindGameObjectWithTag("Player");
            m_targetMonsterCtrl = other.GetComponent<MonsterController>();            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Init(int index)
    {
        m_petBallInfo = InventoryManager._inst.Dict_Petball[index];
    }
    bool IsObjectFront(Transform obj)
    {
        Vector3 relativePos = obj.position - m_player.transform.position;
        if (Vector3.Dot(relativePos, m_player.transform.forward) > 0)
            return true;
        else
            return false;
    }

    IEnumerator CapturePet()
    {
        if(m_targetMonsterCtrl != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(m_targetMonsterCtrl.transform.position);
            transform.position = screenPos;
            if(IsObjectFront(m_targetMonsterCtrl.transform))
            {
                
            }
            else
            {

            }
        }
        yield return null;
    }
}
