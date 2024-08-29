using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetBallController : MonoBehaviour
{
    #region [������]
    const float m_shakeDelayTime = 2f;
    const int m_shakeMaxCount = 3;
    #endregion [������]

    #region [����]
    MonsterController m_targetMonsterCtrl;
    PetBallInfo m_petBallInfo;
    [SerializeField]
    GameObject m_uiRateBoxPrefab;
    UI_CaptureRateBox m_uiRateBox;
    Rigidbody m_rigidbdy;
    #endregion [����]      

    Vector3 m_capturePos;

    
    bool m_isSuccess;
    private void Start()
    {
        //�ӽ�
        InitBall(501);        
        m_rigidbdy = GetComponent<Rigidbody>();
    }  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && m_targetMonsterCtrl == null)
        {
            //��ȹ            
            m_rigidbdy.isKinematic = true;
            m_capturePos = gameObject.transform.position;                   
                   
            m_targetMonsterCtrl = other.GetComponent<MonsterController>();
            m_targetMonsterCtrl.gameObject.SetActive(false);
            StartCoroutine(CaptureStart());
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void InitBall(int index)
    {
        m_petBallInfo = InventoryManager._inst.Dict_Petball[index];
    }

    IEnumerator CaptureStart()
    {
        Vector3 targetPos = m_capturePos + Vector3.up * 2;
        while (transform.position != targetPos)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, 4f * Time.deltaTime);           
            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                gameObject.transform.position = targetPos;
                break;
            }
            yield return null;
        }
        GameObject ui = Instantiate(m_uiRateBoxPrefab);             
        m_uiRateBox = ui.GetComponent<UI_CaptureRateBox>();        
        m_uiRateBox.OpenUI(gameObject.transform.position);
        CaculateCaputreRate();        
    }

    IEnumerator ShakeBall(float a)
    {
        int count = 0;
        float b = 65536 / Mathf.Pow((255 / a), 0.1875f);
        while (count <= m_shakeMaxCount)
        {
            float RN = Random.Range(0, 65535);
            float rate = Mathf.Pow(b / 65535, m_shakeMaxCount - count);
            if (count == 0)
                Debug.Log("���� ���Դ�!");
            if (count > 0 && RN > b)
            {
                //��ȹ ���� (������ ���� Ż��)
                Debug.Log("�� ���� ������ �������Դ�!");
                m_isSuccess = false;
                m_uiRateBox.CaptureFailed();
                m_targetMonsterCtrl.gameObject.SetActive(true);
                break;
            }
            else if (RN < b)
            {              
                if (count >= 1 && count < m_shakeMaxCount)
                {
                    Debug.LogFormat("���� ���ȴ�!" + (count));
                    //�� ��鸲 or ����Ʈ
                }
                if (count == m_shakeMaxCount)
                {
                    Debug.Log("��ҳ�?");
                    StartCoroutine(m_uiRateBox.SetRateProgress(rate));
                    //�� ��鸲 or ����Ʈ
                    yield return new WaitForSeconds(m_shakeDelayTime);
                    Debug.Log("��Ҵ�!");
                    
                    m_isSuccess = true;
                    break;
                }
                count++;

            }
            StartCoroutine(m_uiRateBox.SetRateProgress(rate));
            yield return new WaitForSeconds(m_shakeDelayTime);
        }
        if (m_isSuccess)
            m_uiRateBox.CaptureSuccess();
        Destroy(gameObject);
    }
    IEnumerator CaptureImmediately()
    {
        StartCoroutine(m_uiRateBox.SetRateProgress(1f));
        //�ѹ� ���� or ����Ʈ
        yield return new WaitForSeconds(m_shakeDelayTime);
        Debug.Log("�ٷ� ������");
        m_uiRateBox.CaptureSuccess();
        Destroy(gameObject);
    }

    void CaculateCaputreRate()
    {
        float a = (1f - ((2f / 3f) * (m_targetMonsterCtrl.Stat.HP / m_targetMonsterCtrl.Stat.MaxHP))) * m_petBallInfo.BonusRate * m_targetMonsterCtrl.Stat.CaptureRate;
        //float a = (1f - ((2f / 3f)) * 1 / 3) * m_petBallInfo.BonusRate * m_targetMonsterCtrl.Stat.CaptureRate;  //�׽�Ʈ (1/3���� ��)
        //float a = (1f - ((2f / 3f) * (m_targetMonsterCtrl.Stat.HP / m_targetMonsterCtrl.Stat.MaxHP))) * m_petBallInfo.BonusRate * 200; //�׽�Ʈ (���� ��ȹ���� ��)
        if (a >= 255)
        {
            //��ȹ ���� //Ȯ�� 100���� �ٷ� ����
            StartCoroutine(CaptureImmediately());
        }
        else
        {
            StartCoroutine(ShakeBall(a));
        }
    }
}
