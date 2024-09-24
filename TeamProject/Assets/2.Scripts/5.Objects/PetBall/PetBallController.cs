using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetBallController : MonoBehaviour
{
    #region [고정값]
    const float m_shakeDelayTime = 2f;
    const int m_shakeMaxCount = 3;
    #endregion [고정값]

    #region [참조]
    MonsterController m_targetMonsterCtrl;
    PetBallInfo m_petBallInfo;
    [SerializeField]
    GameObject m_uiRateBoxPrefab;
    UI_CaptureRateBox m_uiRateBox;
    Rigidbody m_rigidbdy;
    Animator m_animator;
    #endregion [참조]      

    Vector3 m_capturePos;

    
    bool m_isSuccess;
    private void Start()
    {
        //임시
        InitBall(500);        
        m_rigidbdy = GetComponent<Rigidbody>();
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && m_targetMonsterCtrl == null)
        {
            //포획            
            m_rigidbdy.isKinematic = true;
            m_capturePos = gameObject.transform.position;                   
                   
            m_targetMonsterCtrl = other.GetComponent<MonsterController>();
            // 잡히는 이펙트 .... (모델 껏다 켯다 , 살짝위로 이동 )
            m_targetMonsterCtrl.gameObject.SetActive(false);
           // m_targetMonsterCtrl.ChangeState(MonsterStateCapture._inst);
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
        m_animator = GetComponent<Animator>();
    }

    IEnumerator CaptureStart()
    {
        Vector3 targetPos = m_capturePos + Vector3.up * 4;
        
        while (transform.position != targetPos)
        {
            // 최대한 화려한 이펙트 : 재생하는 동안 볼 위치 따라가도록
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
                Debug.Log("볼에 들어왔다!");
            if (count > 0 && RN > b)
            {
                //포획 실패 (볼에서 펫이 탈출)
                Debug.Log("앗 펫이 볼에서 빠져나왔다!");
                m_isSuccess = false;
                m_uiRateBox.CaptureFailed();                
                break;
            }
            else if (RN < b)
            {              
                if (count >= 1 && count < m_shakeMaxCount)
                {
                    Debug.LogFormat("볼이 흔들렸다!" + (count));
                    m_animator.SetTrigger("ShakeBall");
                    //볼 흔들림 or 이펙트
                }
                if (count == m_shakeMaxCount)
                {
                    Debug.Log("잡았나?");
                    m_animator.SetTrigger("ShakeBall");
                    StartCoroutine(m_uiRateBox.SetRateProgress(rate));
                    //볼 흔들림 or 이펙트
                    yield return new WaitForSeconds(m_shakeDelayTime);
                    Debug.Log("잡았다!");
                    PetEntryManager._inst.AddEntry(m_targetMonsterCtrl.Index ,m_targetMonsterCtrl.Stat.UniqueID);
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
            //UI 적용
            m_uiRateBox.CaptureSuccess();
            //몬스터 잡혔으면 setactive false
            
        }
        else
        {
            //실패시 몬스터 튀어나옴 스테이트, 모델 변경해야함
            //m_targetMonsterCtrl.ChangeState(MonsterStateChase._inst);
        }    

        Destroy(gameObject);
    }
    IEnumerator CaptureImmediately()
    {
        StartCoroutine(m_uiRateBox.SetRateProgress(1f));
        //한번 흔들기 or 이펙트
        yield return new WaitForSeconds(m_shakeDelayTime);
        Debug.Log("바로 잡혔다");
        m_uiRateBox.CaptureSuccess();
        Destroy(gameObject);
    }

    void CaculateCaputreRate()
    {
        //float a = (1f - ((2f / 3f) * (m_targetMonsterCtrl.Stat.HP / m_targetMonsterCtrl.Stat.MaxHP))) * m_petBallInfo.BonusRate * m_targetMonsterCtrl.Stat.CaptureRate;
        float a = (1f - ((2f / 3f)) * 1 / 3) * m_petBallInfo.BonusRate * m_targetMonsterCtrl.Stat.CaptureRate;  //테스트 (1/3남은 피)
        //float a = (1f - ((2f / 3f) * (m_targetMonsterCtrl.Stat.HP / m_targetMonsterCtrl.Stat.MaxHP))) * m_petBallInfo.BonusRate * 50; //테스트 (낮은 포획률의 펫)
        if (a >= 255)
        {
            //포획 성공 //확률 100으로 바로 잡힘
            StartCoroutine(CaptureImmediately());
        }
        else
        {
            StartCoroutine(ShakeBall(a));
            //몬스터 가 위로 가는거 
        }
    }
}
