using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnforceAnvilInteraction : UI_InteractionBase
{
    #region [����]
    EnforceAnvilController m_anvilCtrl;
    #endregion [����]
       
    void Update()
    {
        
    }

   new public void OpenInteractionCraftTable(EnforceAnvilController ctrl)
    {
        if (m_anvilCtrl == null)
            m_anvilCtrl = ctrl;
        base.OpenInteractionCraftTable(ctrl);
    }

    public override void CloseInteraction()
    {
        
    }
    public override void OpenMenu()
    {
        
    }
    public override void CloseMenu()
    {
        
    }
    public override void PressCKey(bool isWeapon)
    {
        m_progressCancel.value += Time.deltaTime;
        if (m_progressCancel.value >= 1)
        {
            if (isWeapon)
            {

                m_weaponIndex = 0;
                UpCKey();
                OpenInteractionCraftTable(m_anvilCtrl);

            }
            else
            {
                Destroy(m_anvilCtrl.transform.parent.gameObject);
                gameObject.SetActive(false);
            }
        }
    }

}
