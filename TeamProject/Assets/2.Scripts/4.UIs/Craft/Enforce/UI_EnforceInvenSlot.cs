using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_EnforceInvenSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_txtLevel;
    UI_EnforceMenuSlot m_enforcePrevSlot;
    UI_EnforceMenuSlot m_enforceNextSlot;

    int m_level;
    public void InitSlot(int index, UI_EnforceMenuSlot prevSlot, UI_EnforceMenuSlot nextSlot)
    {
        m_icon.sprite = PoolingManager._inst._poolingIconByIndex[index].prefab;
        m_level = InventoryManager._inst.Dict_Weapon[index].Level;
        
        m_txtLevel.text = m_level.ToString();
        m_enforcePrevSlot = prevSlot;
        m_enforceNextSlot = nextSlot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        m_enforcePrevSlot.InitSlot(m_icon.sprite, m_txtLevel.text);
        m_enforceNextSlot.InitSlot(m_icon.sprite, (m_level + 1).ToString());
    }
}
