using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class InventoryManager : MonoBehaviour
{
    #region [ Singleton ]
    static InventoryManager _uniqueInstance;
    public static InventoryManager _inst { get { return _uniqueInstance; } }
    #endregion [ Singleton ]

    //��ü ������
    public SOItem[] items;    
    public float MaxItemWeights;

    public UI_Inventory invenUI;
    public float InvenWeight { get { return invenUI.GetItemWeights(); } }

    public UI_Slot[] InventoryItems { get { return invenUI.GetInvenSlots; } }

    void Awake()
    {
        _uniqueInstance = this;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        invenUI.Init();
    }

    public bool CheckSlot(SOItem newItem, int cnt = 1)
    {
        if (invenUI.CheckSlotFull(newItem, cnt))
        {            
            Debug.Log(InvenWeight + (newItem.itemWeight) * cnt);
            if (MaxItemWeights < InvenWeight + (newItem.itemWeight) * cnt)
                return true;
            else
                return false;
        }
        else
        {
            if (MaxItemWeights < InvenWeight + (newItem.itemWeight) * cnt)
                return true;
            else
                return false;
        }
            
    }

    public void AddInvenItem(SOItem newItem, int cnt = 1)
    {
        invenUI.AcquireItem(newItem, cnt);
    }

    public ItemSlotAndCount CheckEnoughItem(List<RequiredItem> items)
    {
        UI_Slot[] slots = InventoryItems;
        int slotNum = 0;
        //������ ���� ��ġ ��        
        List<int> slotNumbers = new List<int>();
        //�ʿ� ������ ��
        List<int> itemCounts = new List<int>();
        
        for(int i = 0; i < items.Count; i++)
        {
            bool isEnoughItem = false;
            for (int j = 0; j < slots.Length; j++)
            {

                if (slots[j].item == items[i].items && slots[j].itemCount >= items[i].cnt)
                {
                    //�������� �����ϰ� ������ ������ ���� �� 
                    slotNum = j;
                    isEnoughItem = true;
                    break;
                }
            }
            if (isEnoughItem == false)
                return null;
            else
            {
                slotNumbers.Add(slotNum);
                itemCounts.Add(items[i].cnt);
            }                
        }

        ItemSlotAndCount itemSlots = new ItemSlotAndCount(slotNumbers, itemCounts);
        return itemSlots;
    }

    public void UseItemForBuild(int slotNumber, int itemCount)
    {
        invenUI.UseItemAtSlot(slotNumber, itemCount);
    }

}
