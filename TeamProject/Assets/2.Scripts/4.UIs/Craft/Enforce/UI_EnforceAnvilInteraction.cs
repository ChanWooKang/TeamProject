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

    }

    public void OpenEnforce(EnforceAnvilController anvilCtrl)
    {
        if (m_anvilCtrl == null)
            m_anvilCtrl = anvilCtrl;
        gameObject.SetActive(true);
        OpenSlot();
        GameManagerEx._inst.ControlUI(true);
        GameManagerEx._inst.ChangeCursorLockForUI(true);
    }
    public void CloseEnforce()
    {
        if (isAnforcing)
            return;
        gameObject.SetActive(false);
        GameManagerEx._inst.ControlUI(false);
        GameManagerEx._inst.ChangeCursorLockForUI(false);
    }

    void OpenSlot()
    {
        int i = 0;
        m_prevSlot.InitSlot(null);
        m_nextSlot.InitSlot(null);
        foreach (int index in InventoryManager._inst.Dict_Weapon.Keys)
        {
            ++i;
            m_invenSlots[i].gameObject.SetActive(true);
            m_invenSlots[i].InitSlot(index, m_prevSlot, m_nextSlot);            
        }
    }


}
