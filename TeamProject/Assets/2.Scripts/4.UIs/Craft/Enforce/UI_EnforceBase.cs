using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class UI_EnforceBase : MonoBehaviour
{
    protected const int weaponOffset = 500;
    protected const int ironOffset = 102;
    public GameObject UIEnforce { get { return m_EnfoceBox; } }
    protected TextMeshProUGUI m_txtUiName;
    

    protected GameObject m_EnfoceBox;
    protected GameObject m_InvenBox;
    protected GameObject m_uiInteractObj;
   
    protected Slider m_progressCancel;
    protected UI_EnforceInvenSlot[] m_invenSlots;
    protected Transform m_invenSlotParent;
    protected Image[] m_arrowImages;
    protected UI_EnforceMenuSlot m_prevSlot;
    protected UI_EnforceMenuSlot m_nextSlot;

    
    public void Init()
    {
        m_EnfoceBox = transform.GetChild(0).gameObject;
        m_InvenBox = transform.GetChild(1).gameObject;
        m_uiInteractObj = transform.GetChild(2).gameObject;        
        m_txtUiName = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        
        
        m_progressCancel = m_uiInteractObj.transform.GetChild(2).GetChild(1).GetComponent<Slider>();
        m_invenSlotParent = transform.GetChild(1).GetChild(1);
        m_invenSlots = m_invenSlotParent.GetComponentsInChildren<UI_EnforceInvenSlot>();
        for(int i = 0;i < m_invenSlots.Length; i++)
        {
            m_invenSlots[i].gameObject.SetActive(false);
        }
        m_arrowImages = transform.GetChild(0).GetChild(1).GetChild(2).GetComponentsInChildren<Image>();
        m_prevSlot = transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<UI_EnforceMenuSlot>();
        m_nextSlot = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<UI_EnforceMenuSlot>();
    }

    public void ClickEnforceButton()
    {
        if(m_prevSlot.ItemIndex != 0)
        {
            if(m_prevSlot.ItemIndex >= weaponOffset)
            {
                InventoryManager._inst.ChangeItemLevel(m_prevSlot.ItemIndex, m_prevSlot.ItemLevel + 1);
            }
            else
            {
                InventoryManager._inst.UseItem(m_prevSlot.ItemIndex, m_prevSlot.ItemLevel);
                
            }
        }
    }
    public abstract void PressCKey();
    public void UpCKey()
    {
        m_progressCancel.value = 0;
    }

}
