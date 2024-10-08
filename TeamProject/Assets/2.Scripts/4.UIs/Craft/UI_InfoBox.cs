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
        

        m_txtName.text = name;
        m_txtDesc.text = desc;
       
    }
    public void OpenMaterialsSlots(int[] materials, int[] matCosts)
    {
        m_listMeterialSlots = new List<UI_MaterialSlot>();
        LowBase matTable = Managers._table.Get(LowDataType.MaterialTable);
        for (int i = 0; i < materials.Length; i++)
        {
            m_matSlotObj = Instantiate(m_matSlotprefab, m_startSlot);
            UI_MaterialSlot slot = m_matSlotObj.GetComponent<UI_MaterialSlot>();
            RectTransform rect = m_matSlotObj.GetComponent<RectTransform>();
            float y = (m_startSlot.sizeDelta.y + 1) * i;
            rect.anchoredPosition = new Vector2(0, y);
            string NameEn = matTable.ToStr(materials[i], "NameEn");
            string NameKr = matTable.ToStr(materials[i], "NameKr");
            int index = matTable.Find("NameEn", NameEn);
            slot.InitSlot(PoolingManager._inst._poolingIconByName[NameEn].prefab, NameKr, matCosts[i], InventoryManager._inst.GetItemCount(index));
            if (i > 0)
            {
                m_parentRT.sizeDelta += m_startSlot.sizeDelta;
                m_parentRT.anchoredPosition = new Vector2(m_parentRT.anchoredPosition.x, m_parentRT.anchoredPosition.y - (m_startSlot.sizeDelta.y * 1 / 2));
            }
            m_listMeterialSlots.Add(slot);
        }
    }
    public void CloseBox()
    {
        for (int i = 0; i < m_listMeterialSlots.Count; i++)
        {
            m_listMeterialSlots[i].gameObject.SetActive(false);
            if (i > 0)
                m_parentRT.anchoredPosition = new Vector2(m_parentRT.anchoredPosition.x, m_parentRT.anchoredPosition.y + (m_startSlot.sizeDelta.y * 1 / 2));
        }        
        gameObject.SetActive(false);
    }
}
