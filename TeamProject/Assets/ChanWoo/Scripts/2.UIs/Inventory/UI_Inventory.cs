using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineDatas;

public class UI_Inventory : MonoBehaviour
{
    public static bool ActiveInventory = false;

    public GameObject main;
    public GameObject Slot_Parent;
    public Text InvenWeightText;
    UI_Slot[] slots;    

    public void Init()
    {
        slots = Slot_Parent.GetComponentsInChildren<UI_Slot>();
        foreach (UI_Slot slot in slots)
        {
            slot.Init();
        }
        CloseUI();
    }  

    public void TryOpenInventory()
    {
        if (ActiveInventory)
            CloseUI();
        else
            OpenUI();            
    }

    void OpenUI()
    {
        ActiveInventory = true;
        SettingInvenWeight();
        main.SetActive(ActiveInventory);
    }

    void CloseUI()
    {
        ActiveInventory = false;
        main.SetActive(ActiveInventory);
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

    public void AcquireItem(SOItem newItem, int count = 1)
    {
        if (newItem.itemType == eItemType.Gold)
            return;

        if(newItem.itemType != eItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].item != null)
                {
                    if (slots[i].item.itemID == newItem.itemID)
                    {
                        if (slots[i].CheckRestSlot(newItem, count))
                        {
                            slots[i].SetSlotCount(count);
                            return;
                        }
                        else
                        {
                            int value = newItem.maxStack - slots[i].itemCount;
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
            if (slots[i].item == null)
            {
                slots[i].AddItem(newItem, count);
                return;
            }
        }
    }

    public bool CheckSlotFull(SOItem newItem, int count = 1)
    {
        if (eItemType.Equipment != newItem.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemID == newItem.itemID)
                    {
                        if (slots[i].itemCount + count <= slots[i].item.maxStack)
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
            if (slots[i].item == null)
            {
                return false;
            }
        }

        return true;
    }
}
