using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CaptureRateBox : MonoBehaviour
{
    [SerializeField]
    Image m_captureProgress;
    [SerializeField]
    TextMeshProUGUI m_textRate;
    GameObject m_player;
    MonsterController m_targetMonsterCtrl;


    private void Start()
    {
        m_captureProgress.fillAmount = 0;
    }
    private void Update()
    {
        if (isActiveAndEnabled)
            StartCoroutine(SetCapturePetUIPosition());
    }
    public IEnumerator SetRateProgress(float rate)
    {
        m_textRate.text = Mathf.RoundToInt(rate * 100f).ToString();        
        while (m_captureProgress.fillAmount < rate)
        {
            m_captureProgress.fillAmount = Mathf.Lerp(m_captureProgress.fillAmount, rate, 3 * Time.deltaTime);
            if (rate - m_captureProgress.fillAmount < 0.01f)
                m_captureProgress.fillAmount = rate;
            yield return null;
        }
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

    bool IsObjectFront(Transform obj)
    {
        Vector3 relativePos = obj.position - m_player.transform.position;
        if (Vector3.Dot(relativePos, m_player.transform.forward) > 0)
            return true;
        else
            return false;
    }
}
