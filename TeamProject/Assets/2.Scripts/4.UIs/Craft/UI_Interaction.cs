using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Interaction : MonoBehaviour
{
    [SerializeField] GameObject m_uiCraftObj;
    [SerializeField] GameObject m_uiMenuObj;
    [SerializeField] GameObject m_uiMenuSlotPrefab;
    [SerializeField] RectTransform m_startSlot;
    GameObject m_uiMenuSlotObj;
    [SerializeField] TextMeshProUGUI m_txtMenuName;
    ObjectPreview m_objPreview;

    List<UI_MenuSlot> m_listUIMenuSlot;
    Vector2Int m_maxMenuVolAmount;

    bool m_isNew = true;
    private void Awake()
    {
        //юс╫ц
        m_maxMenuVolAmount = new Vector2Int(10, 2);
        
    }
    private void Update()
    {
        if (m_uiCraftObj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                OpenMenu();
            }            
        }
        if(m_uiMenuObj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseMenu();
            }
        }
    }

    public void OpenInteraction()
    {
        gameObject.SetActive(true);
        m_uiCraftObj.SetActive(true);
        CloseMenu();
    }
    public void CloseInteraction()
    {
        CloseMenu();
        gameObject.SetActive(false);
    }

    void OpenMenu()
    {
        m_uiMenuObj.SetActive(true);
        m_uiCraftObj.SetActive(false);
        if (m_isNew)
        {
            int num = 0;
            for (int i = 0; i < m_maxMenuVolAmount.y; i++)
            {
                for (int j = 0; j < m_maxMenuVolAmount.x; j++)
                {
                    m_listUIMenuSlot = new List<UI_MenuSlot>();
                    m_uiMenuSlotObj = Instantiate(m_uiMenuSlotPrefab, m_startSlot);
                    UI_MenuSlot slot = m_uiMenuSlotObj.GetComponent<UI_MenuSlot>();
                    RectTransform rect = m_uiMenuSlotObj.GetComponent<RectTransform>();
                    float x = (m_startSlot.sizeDelta.x + 10) * j;
                    float y = -(m_startSlot.sizeDelta.y + 10) * i;
                    rect.anchoredPosition = new Vector2(x, y);
                    slot.InitSlot(j, i);
                    num++;
                    m_listUIMenuSlot.Add(slot);
                    m_isNew = false;
                }
            }
        }
    }

    void CloseMenu()
    {
        m_uiMenuObj.SetActive(false);
        m_uiCraftObj.SetActive(true);
    }
}
