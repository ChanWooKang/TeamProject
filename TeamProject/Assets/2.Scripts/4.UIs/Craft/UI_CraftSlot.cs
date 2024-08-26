using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_CraftSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region [�ӽ� ����]
    [SerializeField] GameObject m_craftBoxObj;

    [SerializeField] GameObject m_prefabObj;
    #endregion [�ӽ� ����]

    #region [����]
    UI_Craft m_uiCraft;
    GameObject m_previewObj;
    GameObject m_craftingObj;
    Architecture m_architectureInfo;
    ObjectPreview m_craftingObjOP; 
    [SerializeField]
    Image m_icon;
    [SerializeField]
    Image m_highlightBG;
    #endregion [����]

    #region [param]
    [SerializeField]
    Color[] m_highlightColor;
    #endregion [param]


    public void InitSlot(int index, UI_Craft uiCraft)
    {
        if (index == 0)
        {
            m_architectureInfo = null;
            m_uiCraft = null;
            m_highlightBG.enabled = false;
            m_icon.enabled = false;
        }
        else
        {
            m_architectureInfo = new Architecture(index);
            m_uiCraft = uiCraft;
            m_highlightBG.enabled = false;
            m_icon.enabled = true;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ��ᰡ ����ϸ� ������ ������Ʈ ����
        m_previewObj = Instantiate(m_prefabObj);
        m_craftingObj = m_prefabObj;       
        m_uiCraft.IsPreviewActivated(true, m_previewObj, m_craftingObj, m_architectureInfo);
        

        m_craftBoxObj.SetActive(false);

        m_craftBoxObj.SetActive(false);

        //UIŬ���� Ŀ�� ���
        GameManagerEx._inst.ControlUI(false, true);
        // �� ���������� �̵� �� ī�޶� ȸ���� ����
        GameManagerEx._inst.isOnBuild = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ��ᰡ ����ϸ� ������ �Ķ��� ������� ���ϸ� ������
        m_highlightBG.enabled = true;
        m_highlightBG.color = m_highlightColor[0];
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        m_highlightBG.enabled = false;
    }

    

}
