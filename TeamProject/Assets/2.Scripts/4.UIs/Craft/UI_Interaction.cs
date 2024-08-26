using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineDatas;

public class UI_Interaction : MonoBehaviour
{
    #region [참조]
    [SerializeField] GameObject m_uiCraftObj;
    [SerializeField] GameObject m_uiMenuObj;
    [SerializeField] GameObject m_uiMenuSlotPrefab;
    [SerializeField] GameObject m_uiWorkloadPrefab;
    [SerializeField] GameObject m_CancelObj;
    [SerializeField] GameObject m_weaponInfoBoxObj;
    [SerializeField] RectTransform m_startSlot;
    [SerializeField] Slider m_progressCraft;
    [SerializeField] Slider m_progressCancel;
    
    GameObject m_uiMenuSlotObj;
    [SerializeField] TextMeshProUGUI m_txtPressOrHold;
    [SerializeField] TextMeshProUGUI m_txtMenuName;
    [SerializeField] TextMeshProUGUI m_txtMenuOrCraft;
    [SerializeField] TextMeshProUGUI m_txtWeaponName;
   
    
    
    #endregion[참조]

    List<UI_MenuSlot> m_listUIMenuSlot;
    Vector2Int m_maxMenuVolAmount;

    bool m_isNew = true;
    bool m_isCraftDone;
    int m_weaponIndex = 0;
   
   
    private void Update()
    {
        if (m_weaponIndex == 0)
        {
            if (m_uiCraftObj.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    OpenMenu();
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
        else if(m_weaponIndex != 0 & !m_isCraftDone)
        {
            if(Input.GetKey(KeyCode.F))
            {
                PressFkey();
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                UpFKey();
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
        else if(m_weaponIndex != 0 & m_isCraftDone)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                m_weaponIndex = 0;
                m_isCraftDone = false;
                OpenInteraction();
                InventoryManager._inst.AddEquipItem(eEquipType.Weapon, new BaseItem());
            }
        }
    }

    public void OpenInteraction()
    {
        if (m_weaponIndex == 0)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Craft Menu";
            m_weaponInfoBoxObj.SetActive(false);
            m_CancelObj.SetActive(false);
            m_isCraftDone = false;
        }
        else if(m_weaponIndex != 0 & !m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press and Hold";
            m_txtMenuOrCraft.text = "Craft";
            m_txtWeaponName.text = InventoryManager._inst.Dict_Weapon[m_weaponIndex].NameKr;
            m_weaponInfoBoxObj.SetActive(true);
            m_CancelObj.SetActive(true);
        }
        else if(m_weaponIndex != 0 & m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Get Weapon";
            m_CancelObj.SetActive(false);
        }
        gameObject.SetActive(true);
        m_uiCraftObj.SetActive(true);
        CloseMenu();
    }
    public void CloseInteraction()
    {
        CloseMenu();
        gameObject.SetActive(false);
    }
    public void ReadyToCraftSometing(int weaponindex)
    {
        m_weaponIndex = weaponindex;
    }

    void OpenMenu()
    {
        m_uiMenuObj.SetActive(true);
        m_uiCraftObj.SetActive(false);
        if (m_isNew)
        {
            DecideSlotCount();
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

    void CloseMenu()
    {
        m_uiMenuObj.SetActive(false);
        m_uiCraftObj.SetActive(true);
    }

    void DecideSlotCount()
    {
        LowBase weaponTable = Managers._table.Get(LowDataType.WeaponTable);
        int count = weaponTable.MaxCount();
        int x = 0;
        int y = 0;
        if (count < 10)
        {
            x = count;
            y = 1;
        }
        else if(count > 10)
        {
            x = 10;
            y = count % 10;
        }
        m_maxMenuVolAmount = new Vector2Int(x, y);
    }
     void PressFkey()
    {
        StartCoroutine(SetProgress());
    }
     void UpFKey()
    {
        m_progressCraft.value = 0;
    }
     void PressCKey()
    {
        m_progressCancel.value += Time.deltaTime;
        if (m_progressCancel.value >= 1)
        {
            m_progressCancel.value = 0;
            m_weaponIndex = 0;
            OpenInteraction();
        }        
    }
     void UpCKey()
    {
        m_progressCancel.value = 0;
    }
    public void PetWork()
    {
        StartCoroutine(SetProgress());
    }
    IEnumerator SetProgress()
    {
        m_progressCraft.value += Time.deltaTime;
        if (m_progressCraft.value >= 1)
        {
            //데스크 위에 무기 생성
            m_progressCraft.value = 0;
            m_isCraftDone = true;
            OpenInteraction();            
        }
        yield return null;
    }
}
