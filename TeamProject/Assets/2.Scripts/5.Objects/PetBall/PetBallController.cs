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
    GameObject m_player;
    [SerializeField]
    Image m_captureProgress;
    [SerializeField]
    TextMeshProUGUI m_textRate;

    #endregion [����]      

    const float m_shakeDelayTime = 1.5f;
    private void Awake()
    {
        //�ӽ�
        m_captureProgress.fillAmount = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pet"))
        {
            //��ȹ
           // m_player = GameObject.FindGameObjectWithTag("Player");
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
    bool IsObjectFront(Transform obj)
    {
        Vector3 relativePos = obj.position - m_player.transform.position;
        if (Vector3.Dot(relativePos, m_player.transform.forward) > 0)
            return true;
        else
            return false;
    }

    IEnumerator SetCapturePetUIPosition()
    {
        if (m_targetMonsterCtrl != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(m_targetMonsterCtrl.transform.position);
            transform.position = screenPos;
            if (IsObjectFront(m_targetMonsterCtrl.transform))
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        yield return null;
    }
    IEnumerator ShakeBall(float a)
    {
        int count = 0;
        float b = 65536 / Mathf.Pow((255 / a), 0.1875f);
        while (count <= 4)
        {
            float RN = Random.Range(0, 65535);
            float rate = Mathf.Pow(b / 65535, 4 - count);
            //m_textRate.text = Mathf.RoundToInt(rate * 100).ToString();
            StartCoroutine(SetRateProgress(rate));
            if (RN > b)
            {
                //��ȹ ���� (������ ���� Ż��)
                Debug.Log("�� ���� ������ �������Դ�!");
                StopCoroutine(ShakeBall(a));
            }
            else
            {
                count++;
                Debug.Log("���� ���ȴ�!");
                //�� ��鸲 or ����Ʈ count == 4 �� �� ��鸲
            }
            yield return new WaitForSeconds(m_shakeDelayTime);
        }
        //��ȹ ����
        Debug.Log("��Ҵ�!");
    }
    IEnumerator SetRateProgress(float rate)
    {
        while (m_captureProgress.fillAmount < rate)
        {
            m_captureProgress.fillAmount = Mathf.Lerp(m_captureProgress.fillAmount, rate, 0.5f);
            if (rate - m_captureProgress.fillAmount < 0.01f)
                m_captureProgress.fillAmount = rate;
            yield return null;
        }
    }
    void CaculateCaputreRate()
    {
        float a = (1 - ((2 / 3) * (m_targetMonsterCtrl.Stat.HP / m_targetMonsterCtrl.Stat.MaxHP))) * m_petBallInfo.BonusRate * m_targetMonsterCtrl.Stat.CaptureRate;
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
