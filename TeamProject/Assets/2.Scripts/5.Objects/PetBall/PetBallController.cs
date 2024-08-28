using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetBallController : MonoBehaviour
{
    #region [����]
    MonsterController m_targetMonsterCtrl;
    PetBallInfo m_petBallInfo;    
    [SerializeField]
    GameObject m_uiRateBoxPrefab;
    UI_CaptureRateBox m_uiRateBox;
    #endregion [����]      

    bool m_isCapturing;
    const float m_shakeDelayTime = 3f;
    private void Start()
    {
        //�ӽ�

        OpenBox(500);
        m_isCapturing = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && !m_isCapturing)
        {
            //��ȹ
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
                //��ȹ ���� (������ ���� Ż��)
                Debug.Log("�� ���� ������ �������Դ�!");
                break;
            }
            else
            {
                if(count == 1)
                    Debug.Log("���� ���Դ�!");
                if (count > 1 && count < 4)
                    Debug.LogFormat("���� ���ȴ�!" + count);
                if(count == 4)
                {
                    Debug.Log("��Ҵ�!");
                    Destroy(gameObject);
                    break;
                }
                count++;
                //�� ��鸲 or ����Ʈ count == 4 �� �� ��鸲
            }
            yield return new WaitForSeconds(m_shakeDelayTime);
        }
        //��ȹ ����
        
    }

    void CaculateCaputreRate()
    {
        float a = (1f - ((2f / 3f) * (m_targetMonsterCtrl.Stat.HP / m_targetMonsterCtrl.Stat.MaxHP))) * m_petBallInfo.BonusRate * m_targetMonsterCtrl.Stat.CaptureRate;
        if (a >= 255)
        {
            //��ȹ ���� //Ȯ�� 100���� �ٷ� ����            
        }
        else
        {
            StartCoroutine(ShakeBall(a));
        }
    }
}
