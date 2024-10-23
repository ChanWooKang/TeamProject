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
    [SerializeField]
    Material[] m_Mball;
    
    Material m_material;
    MonsterController m_targetMonsterCtrl;
    PetBallInfo m_petBallInfo;
    [SerializeField]
    GameObject m_uiRateBoxPrefab;
    UI_CaptureRateBox m_uiRateBox;
    Rigidbody m_rigidbdy;
    Animator m_animator;
    #endregion [����]      

    Vector3 m_capturePos;


    bool m_isSuccess;
    bool m_isRecall;
    float _shootPower = 20.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (m_isRecall)
        {
            if(other.CompareTag("Ground"))
            {
                //Recall
                UIManager._inst.UIPetEntry.RecallOrPutIn(transform.position);
                DestoryObject();
            }
        }
        else
        {
            if (other.CompareTag("Monster") && m_targetMonsterCtrl == null)
            {
                //��ȹ            
                m_rigidbdy.isKinematic = true;
                m_capturePos = gameObject.transform.position;

                m_targetMonsterCtrl = other.GetComponent<MonsterController>();
                // ������ ����Ʈ .... (�� ���� �ִ� , ��¦���� �̵� )
                m_targetMonsterCtrl.gameObject.SetActive(false);
                // m_targetMonsterCtrl.ChangeState(MonsterStateCapture._inst);
                StartCoroutine(CaptureStart());
            }
            else
            {
                DestoryObject();
            }
        }
        
    }    

    void InitBall(int index)
    {
        m_rigidbdy = GetComponent<Rigidbody>();
        m_petBallInfo = InventoryManager._inst.Dict_Petball[index];
        SetMaterial(m_petBallInfo.NameEn);
        
        m_animator = GetComponent<Animator>();
    }

    public void ShootEvent(Vector3 direction, int petBallIndex = 500, bool isreCall = false)
    {
        // �ӽ�
        InitBall(petBallIndex);               
        //
        Vector3 dir = direction * _shootPower;
        m_rigidbdy.AddForce(dir, ForceMode.Impulse);
        m_isRecall = isreCall;
    }
    
    void DestoryObject()
    {
        m_rigidbdy.velocity = Vector3.zero;
        m_rigidbdy.inertiaTensor = Vector3.zero;
        m_rigidbdy.isKinematic = false;
        m_targetMonsterCtrl = null;

        gameObject.DestroyAPS();
    }
    void SetMaterial(string Name)
    {
        Material mat = null;
        switch (Name)
        {
            case "PetBall":
                mat = m_Mball[0];
                break;
            case "GreatBall":
                mat = m_Mball[1];
                break;
            case "SuperBall":
                mat = m_Mball[2];
                break;
        }
        GetComponent<MeshRenderer>().material = mat;
        m_material = mat;
    }
    IEnumerator CaptureStart()
    {
        Vector3 targetPos = m_capturePos + Vector3.up * 4;

        while (transform.position != targetPos)
        {
            // �ִ��� ȭ���� ����Ʈ : ����ϴ� ���� �� ��ġ ���󰡵���
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPos, 4f * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                gameObject.transform.position = targetPos;
                break;
            }
            yield return null;
        }
        m_animator.SetTrigger("SpinBall");
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
                m_targetMonsterCtrl.InitState();
                break;
            }
            else if (RN < b)
            {
                if (count >= 1 && count < m_shakeMaxCount)
                {
                    Debug.LogFormat("���� ���ȴ�!" + (count));
                    m_animator.SetTrigger("ShakeBall");
                    //�� ��鸲 or ����Ʈ
                }
                if (count == m_shakeMaxCount)
                {
                    Debug.Log("��ҳ�?");
                    m_animator.SetTrigger("ShakeBall");
                    StartCoroutine(m_uiRateBox.SetRateProgress(rate));
                    //�� ��鸲 or ����Ʈ
                    yield return new WaitForSeconds(m_shakeDelayTime);
                    Debug.Log("��Ҵ�!");
                    PetEntryManager._inst.AddEntry(m_targetMonsterCtrl.Index, m_targetMonsterCtrl.Stat.UniqueID);
                    StopCoroutine(CaptureStart());
                    m_targetMonsterCtrl.gameObject.DestroyAPS();
                    m_isSuccess = true;
                    break;
                }
                count++;

            }
            StartCoroutine(m_uiRateBox.SetRateProgress(rate));
            yield return new WaitForSeconds(m_shakeDelayTime);
        }
        if (m_isSuccess)
        {
            //UI ����
            m_uiRateBox.CaptureSuccess();
            //���� �������� setactive false

        }
        else
        {
            //���н� ���� Ƣ��� ������Ʈ, �� �����ؾ���
            //m_targetMonsterCtrl.ChangeState(MonsterStateChase._inst);
        }

        DestoryObject();
    }
    IEnumerator CaptureImmediately()
    {
        StartCoroutine(m_uiRateBox.SetRateProgress(1f));
        //�ѹ� ���� or ����Ʈ
        yield return new WaitForSeconds(m_shakeDelayTime);
        Debug.Log("�ٷ� ������");
        m_uiRateBox.CaptureSuccess();
        DestoryObject();
    }

    void CaculateCaputreRate()
    {
        float a = (1f - ((2f / 3f) * (m_targetMonsterCtrl.Stat.HP / m_targetMonsterCtrl.Stat.MaxHP))) * m_petBallInfo.BonusRate * m_targetMonsterCtrl.Stat.CaptureRate;
        //float a = (1f - ((2f / 3f)) * 1 / 3) * m_petBallInfo.BonusRate * m_targetMonsterCtrl.Stat.CaptureRate;  //�׽�Ʈ (1/3���� ��)
        //float a = (1f - ((2f / 3f) * (m_targetMonsterCtrl.Stat.HP / m_targetMonsterCtrl.Stat.MaxHP))) * m_petBallInfo.BonusRate * 50; //�׽�Ʈ (���� ��ȹ���� ��)
        if (a >= 255)
        {
            //��ȹ ���� //Ȯ�� 100���� �ٷ� ����
            StartCoroutine(CaptureImmediately());
        }
        else
        {
            StartCoroutine(ShakeBall(a));
            //���� �� ���� ���°� 
        }
    }
}
