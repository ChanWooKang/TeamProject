using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_CraftSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region [임시 참조]
    [SerializeField] GameObject m_craftBoxObj;

    [SerializeField] GameObject m_prefabObj;
    #endregion [임시 참조]

    #region [참조]
    UI_Craft m_uiCraft;
    GameObject m_previewObj;
    GameObject m_craftingObj;
    Architecture m_architectureInfo;
    ObjectPreview m_craftingObjOP; 
    [SerializeField]
    Image m_icon;
    [SerializeField]
    Image m_highlightBG;
    #endregion [참조]

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
        // 재료가 충분하면 프리뷰 오브젝트 생성
        m_previewObj = Instantiate(m_prefabObj);
        m_craftingObj = m_prefabObj;       
        m_uiCraft.IsPreviewActivated(true, m_previewObj, m_craftingObj, m_architectureInfo);
        

        m_craftBoxObj.SetActive(false);

        m_craftBoxObj.SetActive(false);

        //UI클릭시 커서 잠금
        GameManagerEx._inst.ControlUI(false, true);
        // 얘 켜져있으면 이동 과 카메라 회전만 가능
        GameManagerEx._inst.isOnBuild = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // 재료가 충분하면 배경색을 파란색 충분하지 못하면 빨간색
        m_highlightBG.enabled = true;
        m_highlightBG.color = m_highlightColor[0];
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        m_highlightBG.enabled = false;
    }

    

}
