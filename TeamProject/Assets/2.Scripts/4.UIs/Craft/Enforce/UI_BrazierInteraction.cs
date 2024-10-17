using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BrazierInteraction : UI_EnforceBase
{
    #region [����]
    BrazierController m_brazierCtrl;
    #endregion [����]

    bool isAnforcing;
    private void Update()
    {
        if (isAnforcing)
            return;
        if (m_uiInteractObj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                OpenSlot();
            }
            if (Input.GetKey(KeyCode.C))
            {
                PressCKey();
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                UpCKey();
            }
        }
        if (m_EnfoceBox.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSlot();
            }
        }
    }
    public void OpenEnforce(BrazierController anvilCtrl)
    {             
        if (m_brazierCtrl == null)
            m_brazierCtrl = anvilCtrl;
        gameObject.SetActive(true);
        m_uiInteractObj.SetActive(true);
        CloseSlot();
    }

    public void CloseEnforce()
    {
        if(isAnforcing)
            return;
        CloseSlot();
        gameObject.SetActive(false);
    }
    public override void PressCKey()
    {
        m_progressCancel.value += Time.deltaTime;
        if (m_progressCancel.value >= 1)
        {
            Destroy(m_brazierCtrl.transform.parent.gameObject);
            gameObject.SetActive(false);
        }
    }
    void OpenSlot()
    {

        UIManager._inst.UIOff();
        m_EnfoceBox.SetActive(true);
        m_InvenBox.SetActive(true);
       
        m_uiInteractObj.SetActive(false);
        int i = 0;
        m_prevSlot.InitSlot(null);
        m_nextSlot.InitSlot(null);
        foreach (MaterialItemInfo mat in InventoryManager._inst.Dict_Material.Values)
        {
            if (mat.Index == 102)
            {
                m_invenSlots[i].gameObject.SetActive(true);
                m_invenSlots[i].InitSlot(mat.Index, m_prevSlot, m_nextSlot);
                break;
            }
        }
        GameManagerEx._inst.ControlUI(false, false);
        GameManagerEx._inst.ChangeCursorLockForUI(true);
    }
    void CloseSlot()
    {
        UIManager._inst.UIOn();
        
        m_EnfoceBox.SetActive(false);
        m_InvenBox.SetActive(false);
        GameManagerEx._inst.ControlUI(false);
        GameManagerEx._inst.ChangeCursorLockForUI(false);
    }
}
