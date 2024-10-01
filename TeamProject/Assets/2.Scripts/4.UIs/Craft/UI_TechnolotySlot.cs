using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_TechnolotySlot : MonoBehaviour, IPointerClickHandler
{
    #region [Component]
    [SerializeField] Image m_imgBg;
    [SerializeField] Image m_imgInActive;
    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_name;

    #endregion [Component]

    int m_slotNum;
    Architecture m_acrInfo;
    public void InitSlot(int num)
    {
        m_slotNum = num;
        m_acrInfo = new Architecture(num + 1);
        m_name.text = m_acrInfo.NameKr;
        if (m_acrInfo.NameEn != "")
            m_icon.sprite = PoolingManager._inst._poolingIconByName[m_acrInfo.NameEn].prefab;
        else
        {
            m_icon.enabled = false;
            m_name.text = "출시 예정";
        }

        if (num < TechnologyManager._inst.TechLevel)
        {
            m_imgInActive.enabled = false;
        }
        else
        {
            m_imgInActive.enabled = true;
        }
    }

    public void ActiveSlot()
    {
        m_imgInActive.enabled = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
