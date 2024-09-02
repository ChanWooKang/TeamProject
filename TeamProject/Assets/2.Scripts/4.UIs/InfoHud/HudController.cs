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

    public void InitHud(string name, int level, Color color) // ���� ������ ���ÿ� �ʱ�ȭ, Color - ���� : ����, �� : �׸�
    {
        m_txtName.text = name;
        m_txtLevel.text = level.ToString();
        m_hpBar.value = 1f;
        m_bgColor.color = color;
    }
    public void DisPlayDamage(float normalizedHp) // ������
    {
        ShowHud();
        if (IsInvoking("HideHud"))
            CancelInvoke("hideHud");
        Invoke("HideHud", 5f);

        m_hpBar.value = normalizedHp;
    }
    public void ShowHud() // �������� �԰ų� ī�޶� ray�� ����� �� 
    {
        gameObject.SetActive(true);
    }
    void HideHud()
    {
        gameObject.SetActive(false);
    }
}
