using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Craft : MonoBehaviour
{
    [SerializeField] GameObject m_craftBoxObj;

    [SerializeField] GameObject m_prefabObj;
    
   
    GameObject m_previewObj;
    GameObject m_craftingObj;
    Architecture m_architectureInfo;
   
    [SerializeField] List<UI_CraftSlot> m_listCraftSlot;
    Dictionary<int, UI_CraftSlot> m_dicCraftSlots;

    RaycastHit m_hitInfo;
    [SerializeField] LayerMask m_layerMask;
    [SerializeField] float m_range;

    bool m_isPreviewActivated;
    bool m_isOn = false;
    int m_tech;
    private void Awake()
    {
        m_dicCraftSlots = new Dictionary<int, UI_CraftSlot>();

        //юс╫ц
        for (int i = 0; i < m_listCraftSlot.Count; i++)
        {
            m_dicCraftSlots.Add(i + 1, m_listCraftSlot[i]);
        }
        //
        
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && m_previewObj != null)
            Build();

        if (m_isPreviewActivated)
        {
            PreviewPositionUpdate();

            if (Input.GetKeyDown(KeyCode.Escape))
                CancleCraft();
        }
    }
    public void InteractionUI(int techLevel = 0)
    {
        if (m_isPreviewActivated)
            return;        

        if (!m_isOn)
        {
            m_isOn = true;
            m_craftBoxObj.SetActive(m_isOn);
            UIManager._inst.UIOff();
            GameManagerEx._inst.ControlUI(m_isOn, true);
            transform.SetAsLastSibling();
        }
        else
        {
            m_isOn = false;
            for(int i = 0; i < m_listCraftSlot.Count;i++)
            {
                m_listCraftSlot[i].CloseSlot();
            }
            m_craftBoxObj.SetActive(m_isOn);
            UIManager._inst.UIOn();
            GameManagerEx._inst.ControlUI(m_isOn, true);
            return;
        }

        if (techLevel != 0)
        {
            for (int i = 1; i < m_listCraftSlot.Count + 1; i++)
            {
                int level = 0;
                level = i;
                if (i > techLevel)
                    level = 0;
                InitSlots(i, level);
            }
        }
    }   
    public void InitSlots(int index, int techLevel)
    {
        m_dicCraftSlots[index].InitSlot(techLevel, this);
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
            GameObject go = Instantiate(m_craftingObj, m_hitInfo.point, m_previewObj.transform.rotation);
            Destroy(m_previewObj);
            ObjectPreview op = go.GetComponentInChildren<ObjectPreview>();
            op.SetArchitectureInfo(m_architectureInfo);
            op.FixedObject();

            m_isPreviewActivated = false;
            m_previewObj = null;
            m_architectureInfo = null;
            m_isOn = false;
            m_craftBoxObj.SetActive(m_isOn);
            OffBuildAction();
            UIManager._inst.UIOn();
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
        m_isOn = false;
        m_craftBoxObj.SetActive(m_isOn);
        OffBuildAction();
        GameManagerEx._inst.ControlUI(false, true);
    }

    void OffBuildAction()
    {
        GameManagerEx._inst.OffBuildAction();
    }
}
