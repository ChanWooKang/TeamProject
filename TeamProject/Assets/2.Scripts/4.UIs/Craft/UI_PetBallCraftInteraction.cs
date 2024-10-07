using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class UI_PetBallCraftInteraction : UI_InteractionBase
{
    #region [참조]
    PetBallCraftTableConotroller m_tableCtrl;
    #endregion [참조]
    public override void CloseInteraction()
    {
        CloseMenu();
        gameObject.SetActive(false);
    }

    public override void OpenMenu()
    {
        m_uiMenuObj.SetActive(true);
        m_uiCraftObj.SetActive(false);
        m_CancelObj.SetActive(false);
        if (m_isNew)
        {
            DecideSlotCount(LowDataType.PetBallTable);
            int num = 0;
            for (int i = 0; i < m_maxMenuVolAmount.y; i++)
            {
                for (int j = 0; j < m_maxMenuVolAmount.x; j++)
                {
                    m_listUIMenuSlot = new List<UI_MenuSlot>();
                    m_uiMenuSlotObj = Instantiate(m_uiMenuSlotPrefab, m_startSlot);
                    UI_MenuSlot slot = m_uiMenuSlotObj.GetComponent<UI_MenuSlot>();
                    RectTransform rect = m_uiMenuSlotObj.GetComponent<RectTransform>();
                    float x = (m_startSlot.sizeDelta.x + 10) * j;
                    float y = -(m_startSlot.sizeDelta.y + 10) * i;
                    rect.anchoredPosition = new Vector2(x, y);
                    slot.InitSlot(num, j, i, this);
                    num++;
                    m_listUIMenuSlot.Add(slot);
                    m_isNew = false;
                }
            }
        }
        //UI클릭시 커서 잠금
        GameManagerEx._inst.ControlUI(false, false);
        GameManagerEx._inst.ChangeCursorLockForUI(true);
    }

    public override void CloseMenu()
    {
        m_uiMenuObj.SetActive(false);
        m_CancelObj.SetActive(true);
        m_uiCraftObj.SetActive(true);
    }

    public override void PressCKey(bool isWeapon)
    {
        m_progressCancel.value += Time.deltaTime;
        if (m_progressCancel.value >= 1)
        {
            if (isWeapon)
            {

                m_itemIndex = 0;
                UpCKey();
                OpenInteractionTable(m_tableCtrl);

            }
            else
            {
                Destroy(m_tableCtrl.transform.parent.gameObject);
                gameObject.SetActive(false);
            }
        }
    }

}
