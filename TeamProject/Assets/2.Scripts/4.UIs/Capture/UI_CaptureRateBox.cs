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
    [SerializeField]
    GameObject m_RateBox;

    Vector3? m_targetPos;


    private void Awake()
    {
        m_captureProgress.fillAmount = 0;
        m_captureProgress.color = Color.white;
    }
    private void Update()
    {
        if (m_targetPos != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(m_targetPos.Value);
            m_RateBox.transform.position = screenPos;
            if (IsObjectFront(m_targetPos))
            {
                m_RateBox.SetActive(true);
            }
            else
            {
                m_RateBox.SetActive(false);
            }
        }
    }
    public void OpenUI(Vector3 pos)
    {

        m_targetPos = pos;
    }
    public void CaptureSuccess()
    {
        //ÀÌÆåÆ® »ý¼º½ÃÅ°°í ¿¢Æ¼ºê ²û
        gameObject.SetActive(false);
    }
    public void CaptureFailed()
    {
        //ÀÌÆåÆ® »ý¼º Æê Æ¢¾î³ª¿È ¿¢Æ¼ºê ²û
        gameObject.SetActive(false);
    }
    public IEnumerator SetRateProgress(float rate)
    {
        m_textRate.text = Mathf.RoundToInt(rate * 100f).ToString();
        SetProgressImageColor(rate);
        float roundRate = Mathf.Floor(rate * 100f) / 100f;
        while (m_captureProgress.fillAmount < roundRate)
        {
            m_captureProgress.fillAmount = Mathf.Lerp(m_captureProgress.fillAmount, roundRate, 4 * Time.deltaTime);
            if (rate - m_captureProgress.fillAmount < 0.01f)
                m_captureProgress.fillAmount = roundRate;
            yield return null;
        }
    }
    void SetProgressImageColor(float rate)
    {
        Color color;
        if (rate <= 0.25f)
            color = Color.red;
        else if (rate > 0.25f && rate <= 0.5f)
            color = new Color(255f, 177f, 0f);
        else if (rate > 0.5f && rate <= 0.75f)
            color = Color.green;
        else
            color = new Color(0, 255f, 255f);
        m_captureProgress.color = color;
    }
    bool IsObjectFront(Vector3? obj)
    {
        Vector3 relativePos = obj.Value - Camera.main.transform.position;
        if (Vector3.Dot(relativePos, Camera.main.transform.forward) > 0)
            return true;
        else
            return false;
    }
}
