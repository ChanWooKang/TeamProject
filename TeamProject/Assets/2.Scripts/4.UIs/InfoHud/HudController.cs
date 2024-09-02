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

    Vector3 m_targetPos;
    private void Update()
    {
        if (isActiveAndEnabled)
        {
            Vector3 screenPos = Camera.main.ScreenToWorldPoint(m_targetPos);
            transform.position = screenPos;
        }
    }
    public void InitHud(string name, int level, Vector3 tartgetPos ,Color color) // ���� ������ ���ÿ� �ʱ�ȭ, Color - ���� : ����, �� : �׸�
    {
        m_txtName.text = name;
        m_txtLevel.text = level.ToString();
        m_hpBar.value = 1f;
        m_bgColor.color = color;
        m_targetPos = tartgetPos;
    }
    public void DisPlay(float normalizedHp) // �������� �԰ų� ī�޶� ray�� ����� �� 
    {
        ShowHud();
        if (IsInvoking("HideHud"))
            CancelInvoke("hideHud");
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
}
