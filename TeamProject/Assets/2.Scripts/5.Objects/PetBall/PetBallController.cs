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
                PoolingManager._inst.InstantiateAPS("BallHit", transform.position + (Vector3.up * 0.3f), transform.rotation, Vector3.one * 0.3f);
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
                PoolingManager._inst.InstantiateAPS("BallHit", transform.position + (Vector3.up * 0.3f), transform.rotation, Vector3.one);
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
        SoundManager._inst.PlaySfx("Throw");
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
        SoundManager._inst.PlaySfx("BallBound");
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
        SoundManager._inst.PlaySfx("BallCath");
        GameObject ui = Instantiate(m_uiRateBoxPrefab);
        m_uiRateBox = ui.GetComponent<UI_CaptureRateBox>();
        m_uiRateBox.OpenUI(gameObject.transform.position);
        CaculateCaputreRate();
    }

    IEnumerator ShakeBall(float a)
    {
        int shakeCount = 0;
        float b = 65536 / Mathf.Pow((255 / a), 0.1875f);
        while (shakeCount <= m_shakeMaxCount)
        {
            float RN = Random.Range(0, 65535);
            float rate = Mathf.Pow(b / 65535, m_shakeMaxCount - shakeCount);                            
            if (shakeCount > 0 && RN > b)
            {
                //��ȹ ���� (������ ���� Ż��)                
                m_isSuccess = false;                              
                break;
            }
            else if (RN < b)
            {
                if (shakeCount >= 1 && shakeCount < m_shakeMaxCount)
                {                    
                    m_animator.SetTrigger("ShakeBall");
                    SoundManager._inst.PlaySfx("BallShake");
                    //�� ��鸲
                }
                if (shakeCount == m_shakeMaxCount)
                {                    
                    m_animator.SetTrigger("ShakeBall");
                    SoundManager._inst.PlaySfx("BallShake");
                    StartCoroutine(m_uiRateBox.SetRateProgress(rate));
                    //�� ��鸲 (���� Ȯ��)
                    yield return new WaitForSeconds(m_shakeDelayTime);
                    // ����                   
                    m_isSuccess = true;
                    break;
                }
                shakeCount++;
            }
            StartCoroutine(m_uiRateBox.SetRateProgress(rate));
            yield return new WaitForSeconds(m_shakeDelayTime);
        }
        if (m_isSuccess)
        {
            //UI ����
            m_uiRateBox.CaptureSuccess();
            //���� �������� setactive false
            PoolingManager._inst.InstantiateAPS("CFXR _Catch", transform.position, transform.rotation, Vector3.one);
            PoolingManager._inst.InstantiateAPS("ChatchSucceed", transform.position, transform.rotation, Vector3.one);
            SoundManager._inst.PlaySfx("BallCatchSucceed");
            SoundManager._inst.PlaySfx("BallCatchEntry");
            PetEntryManager._inst.AddEntry(m_targetMonsterCtrl.Index, m_targetMonsterCtrl.Stat.UniqueID, m_petBallInfo.Index);            
            StopCoroutine(CaptureStart());
            m_targetMonsterCtrl.gameObject.DestroyAPS();
        }
        else
        {
            //���н� ���� Ƣ���
            //m_targetMonsterCtrl.ChangeState(MonsterStateChase._inst);
            m_uiRateBox.CaptureFailed();
            m_targetMonsterCtrl.gameObject.SetActive(true);
            m_targetMonsterCtrl.InitState();
            PoolingManager._inst.InstantiateAPS("CFXR_FAIL", transform.position, transform.rotation, Vector3.one);
            PoolingManager._inst.InstantiateAPS("RecallMisc", m_targetMonsterCtrl.gameObject.transform.position, transform.rotation, Vector3.one);
            SoundManager._inst.PlaySfx("BallCatchFail");
        }

        DestoryObject();
    }
    IEnumerator CaptureImmediately()
    {
        StartCoroutine(m_uiRateBox.SetRateProgress(1f));
        SoundManager._inst.PlaySfx("BallShake");
        //�ѹ� ���� or ����Ʈ
        yield return new WaitForSeconds(m_shakeDelayTime);
        // ���
        PetEntryManager._inst.AddEntry( m_targetMonsterCtrl.Index, m_targetMonsterCtrl.Stat.UniqueID, m_petBallInfo.Index);
        PoolingManager._inst.InstantiateAPS("CFXR _Catch", transform.position, transform.rotation, Vector3.one);
        StopCoroutine(CaptureStart());
        m_targetMonsterCtrl.gameObject.DestroyAPS();
        m_isSuccess = true;
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
            // �� ��鸲 ����
        }
    }
}
