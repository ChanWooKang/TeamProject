using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_CraftSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region [임시 참조]
    [SerializeField] GameObject m_uiCraftBoxObj;   
    GameObject m_prefabObj;
    #endregion [임시 참조]

    #region [참조]
    UI_Craft m_uiCraft;
    GameObject m_previewObj;
    GameObject m_craftingObj;
    Architecture m_architectureInfo;   
    [SerializeField]
    Image m_icon;
    [SerializeField]
    Image m_highlightBG;
    [SerializeField] GameObject m_uiInfoBoxObj;
    [SerializeField] Transform m_infoboxP;
    UI_InfoBox m_uiInfoBox;
    #endregion [참조]

    #region [param]
    [SerializeField]
    Color[] m_highlightColor;
    
    int[] m_materialsIndex;
    int[] m_materialsCosts;
    bool isAllcostReady;
    #endregion [param]


    public void InitSlot(int techLevel, UI_Craft uiCraft)
    {
        if (techLevel == 0)
        {
            m_architectureInfo = null;
            m_uiCraft = null;
            m_highlightBG.enabled = false;
            m_icon.enabled = false;
        }
        else
        {
            m_architectureInfo = new Architecture(techLevel);
            
            m_uiCraft = uiCraft;
            m_highlightBG.enabled = false;
            m_icon.enabled = true;
            m_icon.sprite = PoolingManager._inst._poolingIconByName[m_architectureInfo.NameEn].prefab;
            m_materialsIndex = m_architectureInfo.MaterialsIndex;
            m_materialsCosts = m_architectureInfo.MaterialsCost;
            
            m_prefabObj = TechnologyManager._inst.CraftObjPrefabs[techLevel - 1];
            
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 재료가 충분하면 프리뷰 오브젝트 생성
        if (m_architectureInfo == null || !isAllcostReady)
            return;
        m_previewObj = Instantiate(m_prefabObj);
        m_craftingObj = m_prefabObj;       
        m_uiCraft.IsPreviewActivated(true, m_previewObj, m_craftingObj, m_architectureInfo);
        m_highlightBG.enabled = false;

        m_uiCraftBoxObj.SetActive(false);
        m_uiInfoBox.CloseBox();
        //UI클릭시 커서 잠금
        GameManagerEx._inst.ControlUI(false, true);
        // 얘 켜져있으면 이동 과 카메라 회전만 가능
        GameManagerEx._inst.isOnBuild = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_architectureInfo == null)
            return;
        if(m_uiInfoBox == null)
        {
            GameObject ui = Instantiate(m_uiInfoBoxObj, m_infoboxP);

            m_uiInfoBox = ui.GetComponent<UI_InfoBox>();
        }
        m_uiInfoBox.transform.SetAsLastSibling();
        RectTransform rect = m_uiInfoBox.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(650, 300);
        m_uiInfoBox.OpenBox(m_architectureInfo.NameKr, m_architectureInfo.Desc);
        m_uiInfoBox.OpenMaterialsSlots(m_materialsIndex, m_materialsCosts);
        // 재료가 충분하면 배경색을 파란색 충분하지 못하면 빨간색
        for (int i = 0; i < m_materialsIndex.Length; i++)
        {
            int count = InventoryManager._inst.GetItemCount(m_materialsIndex[i]);
            if (m_materialsCosts[i] > count)
            {
                isAllcostReady = false;

                m_highlightBG.color = m_highlightColor[1];
                break;
            }
            else
                isAllcostReady = true;
        }
        if (isAllcostReady)
            m_highlightBG.color = m_highlightColor[0];
        m_highlightBG.enabled = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        m_highlightBG.enabled = false;
        m_uiInfoBox.CloseBox();
    }
    public void CloseSlot()
    {
        if(m_uiInfoBox != null)
        m_uiInfoBox.CloseBox();
    }

    

}
