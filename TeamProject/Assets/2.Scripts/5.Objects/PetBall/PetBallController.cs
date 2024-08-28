using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBallController : MonoBehaviour
{    
    MonsterController m_targetMonsterCtrl;
    PetBallInfo m_petBallInfo;

    float m_bonusCaptureRate;

    float m_targetMaxhp;
    float m_targetCurrenthp;
    float m_targetCaptureRate;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pet"))
        {
            //��ȹ
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
    IEnumerator CapturePet()
    {

        yield return null;
    }
}
