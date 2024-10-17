using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_EnforceMenuSlot : MonoBehaviour
{
    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_txtLevel;

    int m_itemIndex;
    int m_itemLevel;
    public int ItemIndex { get { return m_itemIndex; } }
    public int ItemLevel { get { return m_itemLevel; } }
    public void InitSlot(Sprite icon, int index = 0, int Level = 0)
    {        
        if (icon == null)
        {
            m_icon.enabled = false;
            m_txtLevel.enabled = false;
        }
        else
        {
            if (m_icon.sprite == PoolingManager._inst._poolingIconByIndex[105].prefab)
            {
                m_itemLevel += 10;
                m_txtLevel.text = m_itemLevel.ToString();
                return;
            }
            m_icon.enabled = true;
            m_txtLevel.enabled = true;
            m_icon.sprite = icon;
            m_txtLevel.text = Level.ToString();
        }
        m_itemIndex = index;
        m_itemLevel = Level;
    }


}
