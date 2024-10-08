using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineDatas;

public class UI_CraftDeskInteraction : UI_InteractionBase
{
    #region [����]   
    [HideInInspector] public CraftTableController m_tableCtrl;

    #endregion[����]   

    #region [�ڷ���]
    List<UI_MenuSlot> m_listUIPetBallSlot;
    #endregion [�ڷ���]


    private void Update()
    {
        if (m_itemIndex == 0 && !m_isCraftDone) // �������� ���Ⱑ ����
        {
            if (m_uiCraftObj.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    OpenMenu();
                }
                if (Input.GetKey(KeyCode.C))
                {
                    PressCKey(false);
                }
                if (Input.GetKeyUp(KeyCode.C))
                {
                    UpCKey();
                }
            }
            if (m_uiMenuObj.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    CloseMenu();
                }
            }
        }
        else if (m_itemIndex != 0 & !m_isCraftDone) // ���⸦ ���� ��
        {
            if (m_progressCraft.value < m_progressCraft.maxValue)
            {
                StartCoroutine(SetProgress());
            }
            else
            {
                //����ũ ���� ���� ����
                if (m_petCtrl != null)
                    m_petCtrl.JobDone();
                m_isCraftDone = true;
                m_itemIndex = 0;
                OpenInteractionCraftTable(m_tableCtrl);
            }
            if (Input.GetKey(KeyCode.F))
            {
                PressFkey();
                if (m_petCtrl != null)
                    m_petCtrl.MoveToObject(m_tableCtrl.transform.position);
            }
            else 
            {
                UpFKey();
            }
            if (Input.GetKey(KeyCode.C))
            {
                PressCKey(true);
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                UpCKey();
            }
        }
        else if (m_itemIndex == 0 & m_isCraftDone) // ���Ⱑ �� ���� ��
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                m_itemIndex = 0;
                m_isCraftDone = false;
                m_progressCraft.value = 0;
                OpenInteractionCraftTable(m_tableCtrl);
                CloseMenu();
                InventoryManager._inst.AddEquipItem(eEquipType.Weapon, new BaseItem());
                TechnologyManager._inst.TechPointUp();
            }
        }
    }

    public void OpenInteractionCraftTable(CraftTableController ctrl = null)
    {
        if (ctrl != null)
            m_tableCtrl = ctrl;
        base.OpenInteractionTable(ctrl);
    }
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
            DecideSlotCount(LowDataType.WeaponTable);
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
        //UIŬ���� Ŀ�� ���
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
                OpenInteractionCraftTable(m_tableCtrl);

            }
            else
            {
                Destroy(m_tableCtrl.transform.parent.gameObject);
                gameObject.SetActive(false);
            }
        }
    }

   


}
