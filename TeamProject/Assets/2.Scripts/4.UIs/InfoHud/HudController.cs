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
            if (IsObjectFront(m_targetPos.position))
            {
                gameObject.SetActive(true);
            }
            else
            {
                HideHud();
            }
        }
    }
    public void InitHud(string name, int level, Transform tartgetPos ,Color color) // ���� ������ ���ÿ� �ʱ�ȭ, Color - ���� : ����, �� : �׸�
    {
        m_txtName.text = name;
        m_txtLevel.text = level.ToString();
        m_hpBar.value = 1f;
        m_bgColor.color = color;
        m_targetPos = tartgetPos;
        Invoke("HideHud", 2f);
    }
    public void DisPlay(float normalizedHp) // �������� �԰ų� ī�޶� ray�� ����� �� 
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(m_targetPos.position);
        transform.position = screenPos;
        ShowHud();
        if (IsInvoking("HideHud"))
            CancelInvoke("HideHud");
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
}
