using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_MenuSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image m_icon;
    [SerializeField] RectTransform[] m_infoBoxPoses;
    [SerializeField] GameObject m_infoBoxPrefab;
    UI_InfoBox m_uiInfoBox;
    UI_CraftDeskInteraction m_uiInteraction;
    

    int m_x;
    int m_y;
    int m_weaponIndex;
    public void InitSlot(int num, int x, int y, UI_CraftDeskInteraction interation)
    {
        m_uiInteraction = interation;

        m_weaponIndex = 200 + num;
        m_x = x;
        m_y = y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_uiInfoBox == null)
        {
            GameObject ui = Instantiate(m_infoBoxPrefab, m_infoBoxPoses[0]);

            m_uiInfoBox = ui.GetComponent<UI_InfoBox>();

        }


        if (m_y < 3)
        {
            if (m_x < 5)
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[0]);

            }
            else
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[1]);
            }
        }
        else
        {
            if (m_x < 5)
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[2]);
            }
            else
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[3]);
            }
        }

        m_uiInfoBox.transform.SetParent(transform.parent);
        m_uiInfoBox.transform.SetAsLastSibling();
        RectTransform rect = m_uiInfoBox.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(650, 300);
        string wNameKr = InventoryManager._inst.Dict_Weapon[m_weaponIndex].NameKr;
        string wDesc = InventoryManager._inst.Dict_Weapon[m_weaponIndex].Desc;
        int[] wMaterialsIndex = InventoryManager._inst.Dict_Weapon[m_weaponIndex].MaterialsIndex;
        int[] wMaterialsCost = InventoryManager._inst.Dict_Weapon[m_weaponIndex].MaterialsCost;
        m_uiInfoBox.OpenBox(wNameKr, wDesc);
        m_uiInfoBox.OpenMaterialsSlots(wMaterialsIndex, wMaterialsCost);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_uiInfoBox != null)
            m_uiInfoBox.CloseBox();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        m_uiInteraction.ReadyToCraftSometing(m_weaponIndex);
        m_uiInteraction.OpenInteractionCraftTable();
        m_uiInfoBox.CloseBox();
        m_uiInteraction.SetPetWork(m_uiInteraction.m_tableCtrl);
    }
}
