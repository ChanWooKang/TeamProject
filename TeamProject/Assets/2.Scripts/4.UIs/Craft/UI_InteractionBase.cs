using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineDatas;


public abstract class UI_InteractionBase : MonoBehaviour
{
    #region [SerializeField]
    [SerializeField] protected GameObject m_uiCraftObj;
    [SerializeField] protected GameObject m_uiMenuObj;
    [SerializeField] protected GameObject m_uiMenuSlotPrefab;
    [SerializeField] protected GameObject m_uiWorkloadPrefab;
    [SerializeField] protected GameObject m_CancelObj;
    [SerializeField] protected GameObject m_weaponInfoBoxObj;
    [SerializeField] protected GameObject m_noEntrytextBox;
    [SerializeField] protected RectTransform m_startSlot;
    [SerializeField] protected Slider m_progressCraft;
    [SerializeField] protected Slider m_progressCancel;
    [SerializeField] protected Image m_petIcon;

    [SerializeField] protected TextMeshProUGUI m_txtPressOrHold;
    [SerializeField] protected TextMeshProUGUI m_txtMenuName;
    [SerializeField] protected TextMeshProUGUI m_txtMenuOrCraft;
    [SerializeField] protected TextMeshProUGUI m_txtWeaponName;
    #endregion [SerializeField]

    protected PetController m_petCtrl;

    protected GameObject m_uiMenuSlotObj;


    protected List<UI_MenuSlot> m_listUIMenuSlot;
    protected Vector2Int m_maxMenuVolAmount;

    protected bool m_isNew = true;
    protected bool m_isCraftDone;

    protected int m_weaponIndex = 0;
    protected float m_petWorkWeight;
    protected float m_playerWorkWeight;
    public void OpenInteractionCraftTable(CraftTableController ctrl)
    {
        if (m_weaponIndex == 0 && !m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Craft Menu";
            m_weaponInfoBoxObj.SetActive(false);
            m_CancelObj.SetActive(true);
            m_isCraftDone = false;
        }
        else if (m_weaponIndex != 0 && !m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press and Hold";
            m_txtMenuOrCraft.text = "Craft";
            m_txtWeaponName.text = InventoryManager._inst.Dict_Weapon[m_weaponIndex].NameKr;
            m_weaponInfoBoxObj.SetActive(true);
            m_CancelObj.SetActive(true);
        }
        else if (m_weaponIndex == 0 && m_isCraftDone)
        {
            m_txtPressOrHold.text = "Press";
            m_txtMenuOrCraft.text = "Get Weapon";
            m_CancelObj.SetActive(false);
        }
        gameObject.SetActive(true);
        m_uiCraftObj.SetActive(true);

        CloseMenu();
    }
    public void OpenInteractionCraftTable(EnforceAnvilController ctrl)
    {

    }
    public abstract void CloseInteraction();
    public void ReadyToCraftSometing(int weaponindex)
    {
        m_weaponIndex = weaponindex;
    }
    public void SetPetEntry(PetController pet, CraftTableController ctCtrl)
    {
        m_noEntrytextBox.SetActive(false);

        m_petIcon.enabled = true;
        m_petWorkWeight = 1f;
        m_petCtrl = pet;
        if (m_weaponIndex != 0 && !m_isCraftDone)
            pet.MoveToObject(ctCtrl.transform.position);
        //¿ÃπÃ¡ˆ
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
    public void DecideSlotCount()
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

    public void PetWork()
    {
        StartCoroutine(SetProgress());
    }
    public IEnumerator SetProgress()
    {
        m_progressCraft.value += ((m_playerWorkWeight + m_petWorkWeight) * Time.deltaTime);

        yield return null;
    }

}
