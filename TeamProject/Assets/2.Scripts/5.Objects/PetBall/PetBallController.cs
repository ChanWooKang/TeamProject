using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBallController : MonoBehaviour
{    
    MonsterController m_targetMonsterCtrl;

    float m_bonusCaptureRate;

    float m_targetMaxhp;
    float m_targetCurrenthp;
    float m_targetCaptureRate;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pet"))
        {
            //Æ÷È¹
            m_targetMonsterCtrl = other.GetComponent<MonsterController>();            
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Init()
    {
        
    }
    IEnumerator CapturePet()
    {

        yield return null;
    }
}
