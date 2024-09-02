using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    [SerializeField]
    Slider m_hpBar;
    [SerializeField]
    TextMeshProUGUI m_txtName;
    [SerializeField]
    TextMeshProUGUI m_txtLevel;
    [SerializeField]
    Image m_bgColor;

    Transform m_targetPos;
    private void Update()
    {
        if (isActiveAndEnabled)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(m_targetPos.position);
            transform.position = screenPos;
        }
    }
    public void InitHud(string name, int level, Transform tartgetPos ,Color color) // 몬스터 생성과 동시에 초기화, Color - 몬스터 : 레드, 펫 : 그린
    {
        m_txtName.text = name;
        m_txtLevel.text = level.ToString();
        m_hpBar.value = 1f;
        m_bgColor.color = color;
        m_targetPos = tartgetPos;
        HideHud();
    }
    public void DisPlay(float normalizedHp) // 데미지를 입거나 카메라 ray에 닿았을 때 
    {
        ShowHud();
        if (IsInvoking("HideHud"))
            CancelInvoke("hideHud");
        Invoke("HideHud", 5f);

        m_hpBar.value = normalizedHp;
    }
    public void LookAt()
    {
        ShowHud();
        if (IsInvoking("HideHud"))
            CancelInvoke("hideHud");
        Invoke("HideHud", 5f);
    }
    void ShowHud() 
    {
        gameObject.SetActive(true);
    }
    void HideHud()
    {
        gameObject.SetActive(false);
    }
}
