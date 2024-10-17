using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnforceAnvilInteraction : UI_EnforceBase
{
    #region [참조]
    EnforceAnvilController m_anvilCtrl;
    #endregion [참조]

    bool isAnforcing;
    void Update()
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

    public void OpenEnforce(EnforceAnvilController anvilCtrl)
    {        
        if (m_anvilCtrl == null)
            m_anvilCtrl = anvilCtrl;
        gameObject.SetActive(true);
        m_uiInteractObj.SetActive(true);
        CloseSlot();
    }

    public void CloseEnforce()
    {
        if (isAnforcing)
            return;
        gameObject.SetActive(false);

    }
    public override void PressCKey()
    {
        m_progressCancel.value += Time.deltaTime;
        if (m_progressCancel.value >= 1)
        {
            Destroy(m_anvilCtrl.transform.parent.gameObject);
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
        foreach (int index in InventoryManager._inst.Dict_Weapon.Keys)
        {
            ++i;
            m_invenSlots[i].gameObject.SetActive(true);
            m_invenSlots[i].InitSlot(index, m_prevSlot, m_nextSlot);
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
