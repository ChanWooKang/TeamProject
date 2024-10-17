using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_MenuSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    const int weaponOffset = 200;
    const int petballOffset = 500;
    [SerializeField] Image m_icon;
    [SerializeField] RectTransform[] m_infoBoxPoses;
    [SerializeField] GameObject m_infoBoxPrefab;
    [SerializeField] GameObject m_inActiveObj;
    UI_InfoBox m_uiInfoBox;
    UI_CraftDeskInteraction m_uiInteraction;
    UI_PetBallCraftInteraction m_uiPetballInteraction;
    Image m_imgBg;
    LowDataType m_type;
    int[] m_materialsIndex;
    int[] m_materialCosts;
    int m_x;
    int m_y;
    int m_itemIndex;
    bool m_isAllColstReady;
    public void InitSlot(LowDataType type, int num, int x, int y, UI_CraftDeskInteraction interation)
    {
        m_imgBg = GetComponent<Image>();
        m_uiInteraction = interation;
        m_type = type;
        m_x = x;
        m_y = y;
        if (type == LowDataType.WeaponTable)
        {
            m_itemIndex = 200 + num;
            m_icon.sprite = PoolingManager._inst._poolingIconByName[InventoryManager._inst.Dict_Weapon[m_itemIndex].NameEn].prefab;
        }       
        else if(type == LowDataType.EquipmentTable)
        {
            m_itemIndex = 300 + num;
            m_icon.sprite = PoolingManager._inst._poolingIconByName[InventoryManager._inst.Dict_Equipment[m_itemIndex].NameEn].prefab;
            m_y += 3;
        }
        
    }
    public void InitSlot(int num, int x, int y, UI_PetBallCraftInteraction interaction)
    {
        m_imgBg = GetComponent<Image>();
        m_uiPetballInteraction = interaction;
        m_itemIndex = 500 + num;
        m_icon.sprite = PoolingManager._inst._poolingIconByName[InventoryManager._inst.Dict_Petball[m_itemIndex].NameEn].prefab;
        m_x = x;
        m_y = y;
    }
    public void InactiveSlot()
    {
        m_inActiveObj.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_inActiveObj.activeSelf)
            return;
        if (m_uiInfoBox == null)
        {
            GameObject ui = Instantiate(m_infoBoxPrefab, m_infoBoxPoses[0]);

            m_uiInfoBox = ui.GetComponent<UI_InfoBox>();

        }
        RectTransform rect = m_uiInfoBox.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(650, 300);

        if (m_y < 3)
        {
            if (m_x < 5)
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[0]);
                rect.anchoredPosition = m_infoBoxPoses[0].anchoredPosition;
            }
            else
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[1]);
                rect.anchoredPosition = m_infoBoxPoses[1].anchoredPosition;
            }
        }
        else
        {
            if (m_x < 5)
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[2]);
                rect.anchoredPosition = m_infoBoxPoses[2].anchoredPosition;
            }
            else
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[3]);
                rect.anchoredPosition = m_infoBoxPoses[3].anchoredPosition;
            }
        }
        
        m_uiInfoBox.transform.SetParent(transform.parent);
        m_uiInfoBox.transform.SetAsLastSibling();
       
        if (m_type == LowDataType.WeaponTable)
        {
            string wNameKr = InventoryManager._inst.Dict_Weapon[m_itemIndex].NameKr;
            string wDesc = InventoryManager._inst.Dict_Weapon[m_itemIndex].Desc;
            m_materialsIndex = InventoryManager._inst.Dict_Weapon[m_itemIndex].MaterialsIndex;
            m_materialCosts = InventoryManager._inst.Dict_Weapon[m_itemIndex].MaterialsCost;

            m_uiInfoBox.OpenBox(wNameKr, wDesc);
            m_uiInfoBox.OpenMaterialsSlots(m_materialsIndex, m_materialCosts);
        }
        else if(m_type == LowDataType.PetBallTable)
        {
            string wNameKr = InventoryManager._inst.Dict_Petball[m_itemIndex].NameKr;
            string wDesc = InventoryManager._inst.Dict_Petball[m_itemIndex].Desc;
            m_materialsIndex = InventoryManager._inst.Dict_Petball[m_itemIndex].MaterialsIndex;
            m_materialCosts = InventoryManager._inst.Dict_Petball[m_itemIndex].MaterialsCost;

            m_uiInfoBox.OpenBox(wNameKr, wDesc);
            m_uiInfoBox.OpenMaterialsSlots(m_materialsIndex, m_materialCosts);
        }
        else if(m_type == LowDataType.EquipmentTable)
        {
            string wNameKr = InventoryManager._inst.Dict_Equipment[m_itemIndex].NameKr;
            string wDesc = InventoryManager._inst.Dict_Equipment[m_itemIndex].Desc;
            m_materialsIndex = InventoryManager._inst.Dict_Equipment[m_itemIndex].MaterialsIndex;
            m_materialCosts = InventoryManager._inst.Dict_Equipment[m_itemIndex].MaterialsCost;

            m_uiInfoBox.OpenBox(wNameKr, wDesc);
            m_uiInfoBox.OpenMaterialsSlots(m_materialsIndex, m_materialCosts);
        }
        m_isAllColstReady = true;
        for (int i = 0; i < m_materialsIndex.Length; i++)
        {
            int count = InventoryManager._inst.GetItemCount(m_materialsIndex[i]);
            if (m_materialCosts[i] > count)
            {
                m_isAllColstReady = false;                
                m_imgBg.color = new Color32(255, 0, 0, 160);
                break;
            }
        }
        if(m_isAllColstReady)
            m_imgBg.color = new Color32(0, 255, 0, 160);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_uiInfoBox != null)
            m_uiInfoBox.CloseBox();
        m_imgBg.color = new Color32(0, 0, 0, 160);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_inActiveObj.activeSelf)
            return;
        if (m_isAllColstReady)
        {
            for (int i = 0; i < m_materialsIndex.Length; i++)
            {
                if (InventoryManager._inst.UseItem(m_materialsIndex[i], m_materialCosts[i]))
                {
                    m_imgBg.color = new Color32(0, 0, 0, 160);
                    m_uiInteraction.ReadyToCraftSometing(m_itemIndex);
                    m_uiInteraction.OpenInteractionCraftTable();
                    m_uiInfoBox.CloseBox();
                    m_uiInteraction.SetPetWork(m_uiInteraction.m_tableCtrl);
                }
            }
        }
    }
}
