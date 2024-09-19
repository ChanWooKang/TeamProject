using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] GameObject m_craftBoxObj;

    [SerializeField] GameObject m_prefabObj;
    Button m_slotBtn;    
    GameObject m_previewObj;
    GameObject m_craftingObj;
    Architecture m_architectureInfo;
    [SerializeField] List<UI_CraftSlot> m_listCraftSlot;
    
    RaycastHit m_hitInfo;
    [SerializeField] LayerMask m_layerMask;
    [SerializeField] float m_range;

    bool m_isPreviewActivated;
    int m_tech;
    private void Start()
    {
        //�ӽ�
        for(int i = 0; i< m_listCraftSlot.Count; i++)
        {
            if (i == 0)
                m_listCraftSlot[i].InitSlot(1, this);
            else if (i == 4)
                m_listCraftSlot[i].InitSlot(5, this);
            else
                m_listCraftSlot[i].InitSlot(0, this);
        }
        //
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && m_previewObj != null)
            Build();
        
        if (m_isPreviewActivated)
            PreviewPositionUpdate();

        if (Input.GetKeyDown(KeyCode.Escape))
            CancleCraft();
    }
    public void OpenUI()
    {
        if (m_isPreviewActivated)
            return;
        gameObject.SetActive(true);

        m_craftBoxObj.SetActive(true);
        
        m_craftBoxObj.SetActive(true);

        GameManagerEx._inst.ControlUI(true, true);
    }
    public void CloseUI()
    {      
        gameObject.SetActive(false);
    }

    public void IsPreviewActivated(bool isPreviwActivated, GameObject previewObj, GameObject craftignObj, Architecture arc)
    {
        m_isPreviewActivated = isPreviwActivated;
        m_previewObj = previewObj;
        m_craftingObj = craftignObj;
        m_architectureInfo = arc;
    }
    
    void PreviewPositionUpdate()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out m_hitInfo, m_range, m_layerMask))
        {
            if (m_hitInfo.transform != null)
            {
                Vector3 _location = m_hitInfo.point;
                m_previewObj.transform.position = _location;
            }
        }
    }
    void Build()
    {
        if (m_isPreviewActivated && m_previewObj.GetComponentInChildren<ObjectPreview>().isBuildable())
        {
            GameObject go = Instantiate(m_craftingObj, m_hitInfo.point, Quaternion.identity);            
            Destroy(m_previewObj);  
            ObjectPreview op = go.GetComponentInChildren<ObjectPreview>();
            op.SetArchitectureInfo(m_architectureInfo);
            op.FixedObject();
            
            m_isPreviewActivated = false;
            m_previewObj = null;
            m_architectureInfo = null;
            gameObject.SetActive(false);
            OffBuildAction();
        }
    }
    void CancleCraft()
    {
        if (m_isPreviewActivated)
        {
            Destroy(m_previewObj);
            
            m_isPreviewActivated = false;
            m_previewObj = null;
        }
        gameObject.SetActive(false);
        OffBuildAction();
        GameManagerEx._inst.ControlUI(false, true);
    }

    void OffBuildAction()
    {
        GameManagerEx._inst.OffBuildAction();
    }
}
