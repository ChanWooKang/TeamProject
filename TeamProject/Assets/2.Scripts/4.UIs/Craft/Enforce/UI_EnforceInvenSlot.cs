using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_EnforceInvenSlot : MonoBehaviour, IPointerClickHandler
{
    const int weaponOffset = 200;
    const int ironOffset = 102;

    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_txtLevel;
    [SerializeField] GameObject m_noneObj;
    UI_EnforceMenuSlot m_enforcePrevSlot;
    UI_EnforceMenuSlot m_enforceNextSlot;

    int m_itemIndex;
    int m_level;
    int m_count;
    public void InitSlot(int index, UI_EnforceMenuSlot prevSlot, UI_EnforceMenuSlot nextSlot)
    {
        m_itemIndex = index;
        m_icon.sprite = PoolingManager._inst._poolingIconByIndex[index].prefab;
        if (index >= weaponOffset)
        {
            m_level = InventoryManager._inst.Dict_Weapon[index].Level;
            m_txtLevel.text = m_level.ToString();
        }
        else if (index == ironOffset)
        {
            m_count = InventoryManager._inst.GetItemCount(index);
            m_txtLevel.text = m_count.ToString();
        }
        else
            return;


        m_enforcePrevSlot = prevSlot;
        m_enforceNextSlot = nextSlot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_itemIndex == ironOffset && m_count == 0)
            return;

        if (m_itemIndex != ironOffset)
        {
            m_enforceNextSlot.InitSlot(m_icon.sprite, m_itemIndex, (m_level + 1));
        }
        else
        {
            if (m_count > 10 && m_enforcePrevSlot.ItemLevel  < m_count)
            {
                m_enforcePrevSlot.InitSlot(m_icon.sprite, m_itemIndex, 10);
                m_enforceNextSlot.InitSlot(PoolingManager._inst._poolingIconByIndex[105].prefab, m_itemIndex, 10);
            }
        }
    }
}
