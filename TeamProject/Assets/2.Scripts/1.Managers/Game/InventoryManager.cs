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

    [Header("Components")]
    public UI_Inventory invenUI;
    List<BaseItem> _items;
    public int itemCount;
    public float MaxItemWeights;
        
    public float InvenWeight { get { return invenUI.GetItemWeights(); } }
    public List<BaseItem> Items { get { return _items; } }
    public UI_Slot[] InventoryItems { get { return invenUI.GetInvenSlots; } }

    private void Awake()
    {
        _uniqueInstance = this;
    }

    private void Start()
    {
        //Test
        Init();
    }

    public void Init()
    {
        invenUI.Init();
        _items = new List<BaseItem>();
        AddItems(LowDataType.MaterialTable);
        AddItems(LowDataType.WeaponTable);
        itemCount = _items.Count;
    }    

    public void AddItems(LowDataType type)
    {
        LowBase table = Managers._table.Get(type);
        eItemType itemType = eItemType.Unknown;
        int maxCount = table.MaxCount();
        int offSetNum = 0;
        switch (type)
        {
            case LowDataType.MaterialTable:
                offSetNum = 100;
                itemType = eItemType.Material;
                break;
            case LowDataType.WeaponTable:
                offSetNum = 200;
                itemType = eItemType.Weapon;
                break;
        }
        for(int i = 0; i < maxCount; i++)
        {
            MakeItemClass(table, itemType, i, offSetNum);
        }
    }

    public void MakeItemClass(LowBase Table,eItemType type, int num, int offsetNumber)
    {
        int index = offsetNumber + num;
        string nameEn = Table.ToStr(index, "NameEn");
        string nameKr = Table.ToStr(index, "NameKr");
        string desc = Table.ToStr(index, "Desc");
        string spriteName = Table.ToStr(index, "SpriteName");
        float weight = Table.ToFloat(index, "Weight");

        switch (type)
        {
            case eItemType.Weapon:
                {
                    string materialsIndexStr = Table.ToStr(index, "Materials");
                    string[] materialsIndexStrArray = materialsIndexStr.Split('/');
                    int[] materialsIndexArray = new int[materialsIndexStrArray.Length];
                    for (int i = 0; i < materialsIndexStrArray.Length; i++)
                    {
                        if (int.TryParse(materialsIndexStrArray[i], out int number))
                            materialsIndexArray[i] = number;
                    }

                    string materialsCost = Table.ToStr(index, "MaterialsCost");
                    string[] materialsCostStrArray = materialsCost.Split('/');
                    int[] materialsCostArray = new int[materialsCostStrArray.Length];
                    for (int i = 0; i < materialsCostArray.Length; i++)
                    {
                        if (int.TryParse(materialsCostStrArray[i], out int number))
                        {
                            materialsCostArray[i] = number;
                        }
                    }

                    float damage = Table.ToFloat(index, "Damage");
                    WeaponItemInfo weapon = new WeaponItemInfo(index, nameEn, desc, spriteName, nameKr, weight, materialsIndexArray, materialsCostArray, damage);
                    _items.Add(weapon);
                }                
                break;
            case eItemType.Material:
                MaterialItemInfo material = new MaterialItemInfo(index, nameEn, desc, spriteName, nameKr, weight);
                _items.Add(material);
                break;
        }
        
    }

    public BaseItem GetItemData(int index)
    {
        for(int i = 0; i < _items.Count; i++)
        {
            if (index == _items[i].Index)
                return _items[i];
        }

        return null;
    }

    public bool CheckSlot(BaseItem newItem, int cnt = 1)
    {
        if (invenUI.CheckSlotFull(newItem, cnt))
        {
            if (MaxItemWeights < InvenWeight + (newItem.Weight * cnt))
                return true;
            else
                return false;
        }

        return false;
    }

    public void AddInvenItem(BaseItem newItem, int cnt = 1)
    {
        invenUI.AcquireItem(newItem,cnt);
    }

    public ItemSlotAndCount CheckEnoughItem(int[] indexs,int[] costs)
    {
        UI_Slot[] slots = InventoryItems;
        int slotNum;
        int costValue;
        bool isEnough;
        List<int> slotNumbers = new List<int>();
        List<int> itemCosts = new List<int>();

        for(int i = 0; i < indexs.Length; i++)
        {
            slotNum = 0;
            costValue = 0;
            isEnough = false;
            for(int j = 0; j < slots.Length; j++)
            {
                if (slots[j].itemData != null)
                {
                    if(slots[j].itemData.Index == slotNumbers[i] 
                        && slots[j].itemCount >= itemCosts[i])
                    {
                        slotNum = j;
                        costValue = itemCosts[i];
                        isEnough = true;
                        break;
                    }
                }
                if (isEnough == false)
                    return null;
                else
                {
                    slotNumbers.Add(slotNum);
                    itemCosts.Add(costValue);
                }
            }
        }
        return new ItemSlotAndCount(slotNumbers, itemCosts);        
    }

    public void UseItemForBuild(int slotNumber, int itemCost)
    {
        invenUI.UseItemAtSlot(slotNumber, itemCost);
    }
}
