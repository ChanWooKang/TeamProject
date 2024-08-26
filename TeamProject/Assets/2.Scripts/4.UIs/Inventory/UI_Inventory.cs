using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineDatas;

public class UI_Inventory : MonoBehaviour
{        
    public GameObject main;
    public GameObject Slot_Parent;
    public Text InvenWeightText;
    UI_Slot[] slots;

    bool isOnUI;

    public UI_Slot[] GetInvenSlots { get { return slots; } }

    public void Init()
    {
        slots = Slot_Parent.GetComponentsInChildren<UI_Slot>();
        for(int i = 0; i < slots.Length; i++)
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
    }

    void CloseUI()
    {
        isOnUI = false;
        main.SetActive(isOnUI);
        UI_ItemInfo._info.OffInformation();
    }

    void SettingInvenWeight()
    {
        float weight = GetItemWeights();
        float maxWeight = InventoryManager._inst.MaxItemWeights;

        InvenWeightText.text = string.Format("{0:D2} / {1:D2}", (int)weight, (int)maxWeight);
    }

    public float GetItemWeights()
    {
        float weights = 0;
        foreach(UI_Slot slot in slots)
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

        if(newItem.Type != eItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].itemData != null)
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

        for(int i = 0; i < slots.Length; i++)
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
}
