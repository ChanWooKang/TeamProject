using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_EnforceMenuSlot : MonoBehaviour
{
    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_txtLevel;

    public void InitSlot(Sprite icon, string txtLevel = null)
    {
        if (icon == null)
        {
            m_icon.enabled = false;
            m_txtLevel.enabled = false;
        }
        else
        {
            m_icon.enabled = true;
            m_txtLevel.enabled = true;
            m_icon.sprite = icon;
            m_txtLevel.text = txtLevel;
        }       
    }


}
