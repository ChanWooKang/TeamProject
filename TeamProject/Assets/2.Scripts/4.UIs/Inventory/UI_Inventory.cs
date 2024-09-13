using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineDatas;

public class UI_Inventory : MonoBehaviour
{
    public GameObject main;
    public GameObject petInven;
    public GameObject Slot_Parent;
    public GameObject petSlot_Parent;
    public Text InvenWeightText;
    [SerializeField] GameObject[] m_tags;
    [SerializeField] UI_PetEnryInfoBoxController m_petEntryBox;
    UI_Slot[] slots;
    UI_PetInvenSlot[] m_petSlots;

    bool isOnUI;

    public UI_Slot[] GetInvenSlots { get { return slots; } }

    public void Init()
    {
        slots = Slot_Parent.GetComponentsInChildren<UI_Slot>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Init();
        }
        UI_ItemInfo._info.Init();
        CloseUI();

    }

    public void TryOpenInventory()
    {
        if (isOnUI)
            CloseUI();
        else
            OpenUI();

        GameManagerEx._inst.ControlUI(isOnUI, true);
    }

    void OpenUI()
    {
        isOnUI = true;
        SettingInvenWeight();
        main.SetActive(isOnUI);
        petInven.SetActive(false);
        for (int i = 0; i < m_tags.Length; i++)
        {
            m_tags[i].SetActive(true);
        }
        m_petEntryBox.CloseUI();
    }

    void CloseUI()
    {
        isOnUI = false;
        m_petEntryBox.OpenUI();
        main.SetActive(isOnUI);
        petInven.SetActive(false);
        UI_ItemInfo._info.OffInformation();
        for (int i = 0; i < m_tags.Length; i++)
        {
            m_tags[i].SetActive(false);
        }
    }

    #region [Main]
    void SettingInvenWeight()
    {
        float weight = GetItemWeights();
        float maxWeight = InventoryManager._inst.MaxItemWeights;

        InvenWeightText.text = string.Format("{0:D2} / {1:D2}", (int)weight, (int)maxWeight);
    }

    public float GetItemWeights()
    {
        float weights = 0;
        foreach (UI_Slot slot in slots)
        {
            weights += slot.itemWeight;
        }
        return weights;
    }

    public void UseItemAtSlot(int slotNumber, int count)
    {
        if (slotNumber >= slots.Length)
            return;

        slots[slotNumber].SetSlotCount(-count);
    }

    public void AcquireItem(BaseItem newItem, int count = 1)
    {
        if (newItem.Type == eItemType.Gold)
            return;

        if (newItem.Type != eItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemData != null)
                {
                    if (slots[i].itemData.Index == newItem.Index)
                    {
                        if (slots[i].CheckRestSlot(newItem, count))
                        {
                            slots[i].SetSlotCount(count);
                            return;
                        }
                        else
                        {
                            int value = newItem.MaxStack - slots[i].itemCount;
                            slots[i].SetSlotCount(value);
                            count -= value;
                            continue;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData == null)
            {
                slots[i].AddItem(newItem, count);
                return;
            }
        }
    }

    public bool CheckSlotFull(BaseItem newItem, int count = 1)
    {
        if (eItemType.Equipment != newItem.Type)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemData != null)
                {
                    if (slots[i].itemData.Index == newItem.Index)
                    {
                        if (slots[i].itemCount + count <= slots[i].itemData.MaxStack)
                        {
                            return false;
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData == null)
            {
                return false;
            }
        }

        return true;
    }
    #endregion [Main]

    #region [PetInven]
    void InitPetInven()
    {
        m_petSlots = petSlot_Parent.GetComponentsInChildren<UI_PetInvenSlot>();
        for (int i = 0; i < m_petSlots.Length; i++)
        {
            if (i < PetEntryManager._inst.m_listPetEntryCtrl.Count)
                m_petSlots[i].InitSlot(PetEntryManager._inst.m_listPetEntryCtrl[i]);
            else
                m_petSlots[i].InitSlot();
        }

    }
    #endregion [PetInven]

    #region [Button]
    public void MainInvenTagBnt()
    {
        main.SetActive(true);
        petInven.SetActive(false);
    }
    public void PetInvenTagBtn()
    {
        main.SetActive(false);
        petInven.SetActive(true);
        InitPetInven();
    }
    #endregion [Button]
}
