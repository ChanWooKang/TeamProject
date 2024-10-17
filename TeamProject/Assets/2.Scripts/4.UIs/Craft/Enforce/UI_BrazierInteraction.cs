using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BrazierInteraction : UI_EnforceBase
{
    #region [참조]
    BrazierController m_brazierCtrl;
    #endregion [참조]

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
    new public void OpenSlot()
    {
        base.OpenSlot();
        if (m_btnEnforce.onClick.GetPersistentEventCount() == 0)
            m_btnEnforce.onClick.AddListener(ClickEnforceButton);
        int i = 0;        
        foreach (MaterialItemInfo mat in InventoryManager._inst.Dict_Material.Values)
        {
            if (mat.Index == 102)
            {
                m_invenSlots[i].gameObject.SetActive(true);
                m_invenSlots[i].InitSlot(mat.Index, m_prevSlot, m_nextSlot);
                break;
            }
        }
       
    }
    new public void ClickEnforceButton()
    {
        base.ClickEnforceButton();
        OpenSlot();
    }
    void CloseSlot()
    {
        UIManager._inst.UIOn();
        
        m_EnfoceBox.SetActive(false);
        m_InvenBox.SetActive(false);
        m_prevSlot.ResetSlot();
        m_nextSlot.ResetSlot();
        GameManagerEx._inst.ControlUI(false);
        GameManagerEx._inst.ChangeCursorLockForUI(false);
    }
}
