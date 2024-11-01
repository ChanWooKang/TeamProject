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
    PetController m_petCtrl;
    Transform m_targetPos;

    bool m_isPet;
    bool m_isDetected;
    float m_timer;
    private void Update()
    {
        if (m_isDetected && isActiveAndEnabled && m_targetPos != null && (m_petCtrl == null || m_petCtrl.isActiveAndEnabled))
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
    private void LateUpdate()
    {
        if (m_isDetected && !m_isPet)
        {
            m_timer += Time.deltaTime;
            if (m_timer > 5)
            {
                if (m_petCtrl == null)
                {
                    HideHud();
                    m_timer = 0;
                    m_isDetected = false;
                }
            }
        }
    }
    public void InitHud(string name, int level, Transform tartgetPos, Color color, bool isPet, PetController pet = null) // ���� ������ ���ÿ� �ʱ�ȭ, Color - ���� : ����, �� : �׸�
    {
        m_txtName.text = name;
        m_txtLevel.text = level.ToString();
        m_hpBar.value = 1f;
        m_bgColor.color = color;
        m_targetPos = tartgetPos;
        m_isPet = isPet;
        if (pet != null)
            m_petCtrl = pet;
        if (!m_isPet)
            HideHud();
        else
            m_isDetected = true;

    }
    public void DisPlay(int level, float normalizedHp) // �������� �԰ų� ī�޶� ray�� ����� �� ������ �ö��� ��
    {
        m_txtLevel.text = level.ToString();
        Vector3 screenPos = Camera.main.WorldToScreenPoint(m_targetPos.position);
        transform.position = screenPos;
        m_isDetected = true;        
        m_infoBox.SetActive(true);


        m_hpBar.value = normalizedHp;
    }
    public void DisPlayEntryHud(float normalizedHp) // entryHud����
    {
        m_hpBar.value = normalizedHp;
    }
    public void ShowHud()
    {
        m_infoBox.SetActive(true);
    }
    public void HideHud()
    {
        m_infoBox.SetActive(false);
        m_isDetected = false;
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
