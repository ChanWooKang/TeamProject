using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineDatas;


public abstract class UI_InteractionBase : MonoBehaviour
{
    //임시
    [SerializeField] public GameObject m_uiMenuSlotPrefab;
    [SerializeField] public GameObject m_uiWorkloadPrefab;
    public GameObject UIMenu { get { return m_uiMenuObj; } }
    #region [ChildComponent]
    protected GameObject m_uiCraftObj;
    protected GameObject m_uiMenuObj;
   
    protected GameObject m_CancelObj;
    protected GameObject m_weaponInfoBoxObj;
    protected GameObject m_noEntrytextBox;
    protected RectTransform m_startSlot;
    protected Slider m_progressCraft;
    protected Slider m_progressCancel;
    protected Image m_petIcon;

    protected TextMeshProUGUI m_txtPressOrHold;
    protected TextMeshProUGUI m_txtMenuName;
    protected TextMeshProUGUI m_txtMenuOrCraft;
    protected TextMeshProUGUI m_txtWeaponName;
    #endregion [ChildComponent]

    protected PetController m_petCtrl;

    protected GameObject m_uiMenuSlotObj;


    protected List<UI_MenuSlot> m_listUIMenuSlot;
    protected Vector2Int m_maxMenuVolAmount;

    protected bool m_isNew = true;
    protected bool m_isCraftDone;

    protected int m_itemIndex = 0;
    protected float m_petWorkWeight;
    protected float m_playerWorkWeight;
   
    public void OpenInteractionTable(CraftTableController ctrl)
    {
        if (m_itemIndex == 0 && !m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Craft Menu";
            m_weaponInfoBoxObj.SetActive(false);
            m_CancelObj.SetActive(true);
            m_isCraftDone = false;
        }
        else if (m_itemIndex != 0 && !m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press and Hold";
            m_txtMenuOrCraft.text = "Craft";
            m_txtWeaponName.text = InventoryManager._inst.Dict_Weapon[m_itemIndex].NameKr;
            m_weaponInfoBoxObj.SetActive(true);
            m_CancelObj.SetActive(true);
        }
        else if (m_itemIndex == 0 && m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Get Weapon";
            m_CancelObj.SetActive(false);
        }
        gameObject.SetActive(true);
        m_uiCraftObj.SetActive(true);

        CloseMenu();
    }
    public void OpenInteractionTable(EnforceAnvilController ctrl)
    {
        if (m_itemIndex == 0 && !m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Enfroce Menu";
            m_weaponInfoBoxObj.SetActive(false);
            m_CancelObj.SetActive(true);
            m_isCraftDone = false;
        }
        else if (m_itemIndex != 0 && !m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press and Hold";
            m_txtMenuOrCraft.text = "Enfroce";
            m_txtWeaponName.text = InventoryManager._inst.Dict_Weapon[m_itemIndex].NameKr;
            m_weaponInfoBoxObj.SetActive(true);
            m_CancelObj.SetActive(true);
        }
        else if (m_itemIndex == 0 && m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Done";
            m_CancelObj.SetActive(false);
        }
        gameObject.SetActive(true);
        m_uiCraftObj.SetActive(true);

        CloseMenu();
    }
    public void OpenInteractionTable(PetBallCraftTableConotroller ctrl)
    {
        if (m_itemIndex == 0 && !m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Craft Menu";
            m_weaponInfoBoxObj.SetActive(false);
            m_CancelObj.SetActive(true);
            m_isCraftDone = false;
        }
        else if (m_itemIndex != 0 && !m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press and Hold";
            m_txtMenuOrCraft.text = "Craft";
            m_txtWeaponName.text = InventoryManager._inst.Dict_Weapon[m_itemIndex].NameKr;
            m_weaponInfoBoxObj.SetActive(true);
            m_CancelObj.SetActive(true);
        }
        else if (m_itemIndex == 0 && m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Get Weapon";
            m_CancelObj.SetActive(false);
        }
        gameObject.SetActive(true);
        m_uiCraftObj.SetActive(true);

        CloseMenu();
    }
    public abstract void CloseInteraction();
    public void ReadyToCraftSometing(int weaponindex)
    {
        m_itemIndex = weaponindex;
    }
    public void SetPetEntry(PetController pet, CraftTableController ctCtrl)
    {
        m_noEntrytextBox.SetActive(false);

        m_petIcon.enabled = true;
        m_petIcon.sprite = PoolingManager._inst._poolingIconByName[pet.PetInfo.NameEn].prefab;
        m_petWorkWeight = 1f;
        m_petCtrl = pet;
        if (m_itemIndex != 0 && !m_isCraftDone)
            pet.MoveToObject(ctCtrl.transform.position);
        //이미지
    }
    public void SetPetWork(CraftTableController ctCtrl)
    {
        if (m_petCtrl != null)
            m_petCtrl.MoveToObject(ctCtrl.transform.position);
    }
    public void SetNoEntry()
    {
        m_petCtrl = null;
        m_noEntrytextBox.SetActive(true);
        m_petIcon.enabled = false;
        m_petWorkWeight = 0;
    }
    public abstract void OpenMenu();
    public abstract void CloseMenu();
    public void DecideSlotCount(LowDataType type)
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
        else if (count > 10)
        {
            x = 10;
            y = count % 10;
        }
        m_maxMenuVolAmount = new Vector2Int(x, y);
    }

    public void PressFkey()
    {
        m_playerWorkWeight = 1f;
    }
    public void UpFKey()
    {
        m_playerWorkWeight = 0;
    }
    public abstract void PressCKey(bool isWeapon);
    public void UpCKey()
    {
        m_progressCancel.value = 0;
    }

   
    public IEnumerator SetProgress()
    {
        m_progressCraft.value += ((m_playerWorkWeight + m_petWorkWeight) * Time.deltaTime);

        yield return null;
    }
    
    public void Init(GameObject menuSlot, GameObject workload)
    {
        m_weaponInfoBoxObj = transform.GetChild(0).gameObject;
        m_txtWeaponName = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        m_uiCraftObj = transform.GetChild(1).gameObject;
        m_noEntrytextBox = transform.GetChild(1).GetChild(1).GetChild(4).GetChild(0).gameObject;
        m_progressCraft = transform.GetChild(1).GetChild(1).GetComponent<Slider>();
        m_petIcon = transform.GetChild(1).GetChild(1).GetChild(4).GetChild(1).GetComponent<Image>();
        m_txtPressOrHold = transform.GetChild(1).GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        m_txtMenuOrCraft = transform.GetChild(1).GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();

        m_CancelObj = transform.GetChild(2).gameObject;
        m_progressCancel = transform.GetChild(2).GetChild(1).GetComponent<Slider>();

        m_uiMenuObj = transform.GetChild(3).gameObject;
        m_startSlot = transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<RectTransform>();
        m_txtMenuName = transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

        m_uiMenuSlotPrefab = menuSlot;
        m_uiWorkloadPrefab = workload;
    }
}
