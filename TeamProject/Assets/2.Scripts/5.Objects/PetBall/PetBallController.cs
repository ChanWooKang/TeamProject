using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetBallController : MonoBehaviour
{
    #region [ÂüÁ¶]
    MonsterController m_targetMonsterCtrl;
    PetBallInfo m_petBallInfo;    
    [SerializeField]
    GameObject m_uiRateBoxPrefab;
    UI_CaptureRateBox m_uiRateBox;
    #endregion [ÂüÁ¶]      

    bool m_isCapturing;
    const float m_shakeDelayTime = 3f;
    private void Start()
    {
        //ÀÓ½Ã

        OpenBox(500);
        m_isCapturing = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && !m_isCapturing)
        {
            //Æ÷È¹
            // m_player = GameObject.FindGameObjectWithTag("Player");
            GameObject ui = Instantiate(m_uiRateBoxPrefab);
            m_uiRateBox = ui.GetComponent<UI_CaptureRateBox>();
            
            m_isCapturing = true;
            m_targetMonsterCtrl = other.GetComponent<MonsterController>();
            CaculateCaputreRate();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OpenBox(int index)
    {
        m_petBallInfo = InventoryManager._inst.Dict_Petball[index];
    }



    IEnumerator ShakeBall(float a)
    {
        int count = 1;
        float b = 65536 / Mathf.Pow((255 / a), 0.1875f);
        while (count <= 4)
        {
            float RN = Random.Range(0, 65535);
            float rate = Mathf.Pow(b / 65535, 4 - count);
            StartCoroutine(m_uiRateBox.SetRateProgress(rate));
            if (RN > b)
            {
                //Æ÷È¹ ½ÇÆÐ (º¼¿¡¼­ ÆêÀÌ Å»Ãâ)
                Debug.Log("¾Ñ ÆêÀÌ º¼¿¡¼­ ºüÁ®³ª¿Ô´Ù!");
                break;
            }
            else
            {
                if(count == 1)
                    Debug.Log("º¼¿¡ µé¾î¿Ô´Ù!");
                if (count > 1 && count < 4)
                    Debug.LogFormat("º¼ÀÌ Èçµé·È´Ù!" + count);
                if(count == 4)
                {
                    Debug.Log("Àâ¾Ò´Ù!");
                    Destroy(gameObject);
                    break;
                }
                count++;
                //º¼ Èçµé¸² or ÀÌÆåÆ® count == 4 ¸é ¾È Èçµé¸²
            }
            yield return new WaitForSeconds(m_shakeDelayTime);
        }
        //Æ÷È¹ ¼º°ø
        
    }

    void CaculateCaputreRate()
    {
        float a = (1f - ((2f / 3f) * (m_targetMonsterCtrl.Stat.HP / m_targetMonsterCtrl.Stat.MaxHP))) * m_petBallInfo.BonusRate * m_targetMonsterCtrl.Stat.CaptureRate;
        if (a >= 255)
        {
            //Æ÷È¹ ¼º°ø //È®·ü 100À¸·Î ¹Ù·Î ÀâÈû            
        }
        else
        {
            StartCoroutine(ShakeBall(a));
        }
    }
}
