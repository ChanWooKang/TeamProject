using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EnforceAnvilInteraction : UI_InteractionBase
{
    #region [참조]
    EnforceAnvilController m_anvilCtrl;
    #endregion [참조]
       
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
