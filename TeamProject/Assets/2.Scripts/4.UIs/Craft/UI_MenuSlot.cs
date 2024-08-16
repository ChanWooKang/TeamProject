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
    int m_x;
    int m_y;
    public void InitSlot(int x, int y)
    {
        m_x = x;
        m_y = y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(m_uiInfoBox == null)
        {
            GameObject ui =Instantiate (m_infoBoxPrefab, m_infoBoxPoses[0]);           
            m_uiInfoBox = ui.GetComponent<UI_InfoBox>();
        }


        if (m_y < 3)
        {
            if(m_x < 5)
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[0]);
                m_uiInfoBox.OpenBox("test", "test");
            }
            else
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[1]);
                m_uiInfoBox.OpenBox("test", "test");
            }
        }
        else
        {
            if (m_x < 5)
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[2]);
                m_uiInfoBox.OpenBox("test", "test");
            }
            else
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[3]);
                m_uiInfoBox.OpenBox("test", "test");
            }
        }
        m_uiInfoBox.transform.SetParent(transform.parent);
        m_uiInfoBox.transform.SetAsLastSibling();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_uiInfoBox != null)
            m_uiInfoBox.CloseBox();
    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
