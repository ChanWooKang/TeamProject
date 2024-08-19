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

    Transform m_player;
    RaycastHit m_hitInfo;
    [SerializeField] LayerMask m_layerMask;
    [SerializeField] float m_range;

    bool m_isPreviewActivated;
    bool m_isActivated;
    private void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
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
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    public void ClickButton()
    {
        m_previewObj = Instantiate(m_prefabObj, m_player.position + m_player.forward, Quaternion.identity);
        m_craftingObj = m_prefabObj;
        m_isPreviewActivated = true;
        m_craftBoxObj.SetActive(false);
        //UI클릭시 커서 잠금
        GameManagerEx._inst.ChangeCursorLockForUI(false);
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
            op.FixedObject();
            m_isActivated = false;
            m_isPreviewActivated = false;
            m_previewObj = null;
            gameObject.SetActive(false);
        }
    }
    void CancleCraft()
    {
        if (m_isPreviewActivated)
        {
            Destroy(m_previewObj);
            m_isActivated = false;
            m_isPreviewActivated = false;
            m_previewObj = null;
        }
        gameObject.SetActive(false);
    }
}
