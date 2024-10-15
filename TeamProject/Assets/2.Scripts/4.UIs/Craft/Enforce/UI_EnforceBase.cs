using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_EnforceBase : MonoBehaviour
{
    
    protected TextMeshProUGUI m_txtUiName;
    protected UI_EnforceInvenSlot[] m_invenSlots;
    protected Transform m_invenSlotParent;
    protected Image[] m_arrowImages;
    protected UI_EnforceMenuSlot m_prevSlot;
    protected UI_EnforceMenuSlot m_nextSlot;

    
    public void Init()
    {
        m_txtUiName = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
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

    


}
