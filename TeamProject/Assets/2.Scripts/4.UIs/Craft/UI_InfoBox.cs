using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InfoBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_txtName;
    [SerializeField] TextMeshProUGUI m_txtDesc;
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
    public void CloseBox()
    {
        gameObject.SetActive(false);
    }
}
