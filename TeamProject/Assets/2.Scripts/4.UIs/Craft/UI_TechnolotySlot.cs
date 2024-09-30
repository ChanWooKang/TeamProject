using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_TechnolotySlot : MonoBehaviour
{
    #region [Component]
    [SerializeField] Image m_imgBg;
    [SerializeField] Image m_imgInActive;
    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_name;
     
    #endregion [Component]

    int m_slotNum;

    public void InitSlot(int num, string name)
    {
        m_slotNum = num;
        m_name.text = name;
        m_icon.sprite = PoolingManager._inst._poolingIconByName[name].prefab;
    }

    public void ActiveSlot()
    {
        m_imgInActive.enabled = false;        
    }
}
