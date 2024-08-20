using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineDatas;

public class UI_InfoBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_txtName;
    [SerializeField] TextMeshProUGUI m_txtDesc;
    [SerializeField] Transform m_materialsSlotBox;
    [SerializeField] RectTransform m_startSlot;
    [SerializeField] GameObject m_matSlotprefab;
    GameObject m_matSlotObj;
    RectTransform m_parentRT;

    List<UI_MaterialSlot> m_listMeterialSlots;
    
    public void OpenBox(string name, string desc)
    {
        gameObject.SetActive(true);
        m_parentRT = GetComponentInParent<RectTransform>();
        m_parentRT.offsetMin = Vector2.zero;
        m_parentRT.offsetMax = Vector2.zero;

        m_txtName.text = name;
        m_txtDesc.text = desc;
    }
    public void OpenMaterialsSlots(int[] materials, int[]matCosts)
    {
        m_listMeterialSlots = new List<UI_MaterialSlot>();
        LowBase matTable = Managers._table.Get(LowDataType.MaterialTable);
        for(int i = 0; i < materials.Length; i++)
        {
            m_matSlotObj = Instantiate(m_matSlotprefab, m_startSlot);
            UI_MaterialSlot slot = m_matSlotObj.GetComponent<UI_MaterialSlot>();
            RectTransform rect = m_matSlotObj.GetComponent<RectTransform>();
            float y = -(m_startSlot.sizeDelta.y + 1) * i;
            rect.anchoredPosition = new Vector2(0, y);
            slot.InitSlot(null, matTable.ToStr(materials[i], "NameKr"), matCosts[i], 0);
            m_parentRT.sizeDelta += m_startSlot.sizeDelta;
            m_listMeterialSlots.Add(slot);
        }
    }
    public void CloseBox()
    {
        for(int i = 0; i < m_listMeterialSlots.Count; i++)
        {
            m_listMeterialSlots[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
