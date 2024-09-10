using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    [SerializeField]
    GameObject m_infoBox;
    [SerializeField]
    Slider m_hpBar;
    [SerializeField]
    TextMeshProUGUI m_txtName;
    [SerializeField]
    TextMeshProUGUI m_txtLevel;
    [SerializeField]
    Image m_bgColor;

    Transform m_targetPos;

    bool m_isPet;
    private void Update()
    {
        if (isActiveAndEnabled && m_targetPos != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(m_targetPos.position);
            transform.position = screenPos;
            if (IsObjectFront(m_targetPos.position))
            {
                m_infoBox.SetActive(true);
            }
            else
            {
                m_infoBox.SetActive(false);
            }
        }
    }
    public void InitHud(string name, int level, Transform tartgetPos, Color color, bool isPet) // 몬스터 생성과 동시에 초기화, Color - 몬스터 : 레드, 펫 : 그린
    {
        m_txtName.text = name;
        m_txtLevel.text = level.ToString();
        m_hpBar.value = 1f;
        m_bgColor.color = color;
        m_targetPos = tartgetPos;
        m_isPet = isPet;
        if (!m_isPet)
            HideHud();
    }
    public void DisPlay(float normalizedHp) // 데미지를 입거나 카메라 ray에 닿았을 때 
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(m_targetPos.position);
        transform.position = screenPos;
        ShowHud();
        if (IsInvoking("HideHud"))
            CancelInvoke("HideHud");
        if (!m_isPet)
            Invoke("HideHud", 5f);

        m_hpBar.value = normalizedHp;
    }
    void ShowHud()
    {
        gameObject.SetActive(true);
    }
    void HideHud()
    {
        gameObject.SetActive(false);
    }
    bool IsObjectFront(Vector3? obj)
    {
        Vector3 relativePos = obj.Value - Camera.main.transform.position;
        if (Vector3.Dot(relativePos, Camera.main.transform.forward) > 0)
            return true;
        else
            return false;
    }
    public bool IsInit()
    {
        if (m_targetPos == null)
            return false;
        else
            return true;
    }
}
