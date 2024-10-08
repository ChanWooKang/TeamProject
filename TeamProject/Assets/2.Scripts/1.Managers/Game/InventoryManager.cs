using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class InventoryManager : TSingleton<InventoryManager>
{    
    [Header("Components")]
    public UI_Inventory invenUI;
    public UI_Equipment equipUI;

    //Action
    public Action<eEquipType, BaseItem, int, bool> OnChangeEvent;

    public List<BaseItem> Items;
    public Dictionary<int, BaseItem> Dict_Item;
    public Dictionary<int, MaterialItemInfo> Dict_Material;
    public Dictionary<int, WeaponItemInfo> Dict_Weapon;
    public Dictionary<int, PetBallInfo> Dict_Petball;
    public Dictionary<int, ItemDatas> Dict_SlotItem;
    //public Dictionary<int, Sprite> Dict_itemSprite;
    //public List<ItemSprite> itemSprites;
    //public List<int> itemNames;
    //PlayerManager playerManager;

    public static bool ActiveChangeEquip = false;
    public int itemCount;
    //public float MaxItemWeights;

    Coroutine EquipCoroutine = null;

    public float MaxItemWeights { get { return GameManagerEx._inst.playerManager._stat.CarryWeight; } set { GameManagerEx._inst.playerManager._stat.CarryWeight = value; } }
    public float InvenWeight { get { return invenUI.GetItemWeights() + equipUI.GetItemWeights(); } }    
    public UI_Slot[] InventoryItems { get { return invenUI.GetInvenSlots; } }

    private void Awake()
    {
        Items = new List<BaseItem>();
        Dict_Material = new Dictionary<int, MaterialItemInfo>();
        Dict_Weapon = new Dictionary<int, WeaponItemInfo>();
        Dict_Petball = new Dictionary<int, PetBallInfo>();
        

        AddItems(LowDataType.MaterialTable);
        AddItems(LowDataType.WeaponTable);
        AddItems(LowDataType.PetBallTable);
    }

    private void Start()
    {
        //Test
        InitData();
        OnChangeEvent -= ChangeEquipment;
        OnChangeEvent += ChangeEquipment;
    }

    public void InitData()
    {
        itemCount = Items.Count;
        invenUI.Init();
        equipUI.Init();
        SetDictionary();
        SetInitInventoryData();
        AddInvenItem(Dict_Item[200]);
        AddInvenItem(Dict_Item[201]);
        AddInvenItem(Dict_Item[202]);
        AddInvenItem(Dict_Item[203]);
        AddInvenItem(Dict_Item[204]);

    }

    #region [ Item Data Load ]
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
            case LowDataType.PetBallTable:
                offSetNum = 500;
                itemType = eItemType.PetBall;
                break;
        }
        for (int i = 0; i < maxCount; i++)
        {
            MakeItemClass(table, itemType, i, offSetNum);
        }
    }

    public void SetInitInventoryData()
    {
        // Key = UI_Slot slotIndex, 
        Dict_SlotItem = new Dictionary<int, ItemDatas>();
        UI_Slot[] slots = InventoryItems;
        for(int i = 0; i < slots.Length; i++)
        {
            int index = 0;
            if (slots[i].itemData != null)
                index = slots[i].itemData.Index;
            ItemDatas datas = new ItemDatas(index, slots[i].itemCount);
            Dict_SlotItem.Add(slots[i].slotIndex, datas);
        }
    }

    public void ChangeInventoryData(int slotIndex, ItemDatas datas)
    {
        if (Dict_SlotItem.ContainsKey(slotIndex))
        {
            Dict_SlotItem[slotIndex] = datas;
        }
    }

    public void MakeItemClass(LowBase Table, eItemType type, int num, int offsetNumber)
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
                    Items.Add(weapon);
                    Dict_Weapon.Add(index, weapon);
                }
                break;
            case eItemType.Material:
                int rate = Table.ToInt(index, "Rate");
                MaterialItemInfo material = new MaterialItemInfo(index, nameEn, desc, spriteName, nameKr, weight,rate);
                Items.Add(material);
                Dict_Material.Add(index, material);
                break;
            case eItemType.PetBall:
                string pmaterialsIndexStr = Table.ToStr(index, "Materials");
                string[] pmaterialsIndexStrArray = pmaterialsIndexStr.Split('/');
                int[] pmaterialsIndexArray = new int[pmaterialsIndexStrArray.Length];
                for (int i = 0; i < pmaterialsIndexStrArray.Length; i++)
                {
                    if (int.TryParse(pmaterialsIndexStrArray[i], out int number))
                        pmaterialsIndexArray[i] = number;
                }

                string pmaterialsCost = Table.ToStr(index, "MaterialsCost");
                string[] pmaterialsCostStrArray = pmaterialsCost.Split('/');
                int[] pmaterialsCostArray = new int[pmaterialsCostStrArray.Length];
                for (int i = 0; i < pmaterialsCostArray.Length; i++)
                {
                    if (int.TryParse(pmaterialsCostStrArray[i], out int number))
                    {
                        pmaterialsCostArray[i] = number;
                    }
                }
                float bonusRate = Table.ToFloat(index, "BonusRate");
                PetBallInfo petball = new PetBallInfo(index, nameEn, desc, spriteName, nameKr, weight, pmaterialsIndexArray, pmaterialsCostArray, bonusRate);
                Items.Add(petball);
                Dict_Petball.Add(index, petball);
                break;
        }

    }

    public BaseItem GetItemData(int index)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (index == Items[i].Index)
                return Items[i];
        }

        return null;
    }

    public Sprite GetItemSprite(int index)
    {
        Dictionary<int, PoolIcon> icon = PoolingManager._inst._poolingIconByIndex;

        if (icon.ContainsKey(index))
        {
            return icon[index].prefab;
        }        

        return null;
    }

    #endregion [ Item Data Load ]

    void SetDictionary()
    {
        Dict_Item = new Dictionary<int, BaseItem>();
        for (int i = 0; i < Items.Count; i++)
        {
            Dict_Item.Add(Items[i].Index, Items[i]);            
        }
        
    }

    void ChangeEquipment(eEquipType type, BaseItem item, int slotIndex = 0, bool isWear = true)
    {
        if (item == null || ActiveChangeEquip)
            return;

        if (EquipCoroutine != null)
            StopCoroutine(EquipCoroutine);
        EquipCoroutine = StartCoroutine(ChangeCoroutine(type, item, slotIndex, isWear));

    }

    IEnumerator ChangeCoroutine(eEquipType type, BaseItem item, int slotIndex = 0, bool isWear = true)
    {
        ActiveChangeEquip = true;
        Dictionary<eEquipType, UI_EquipSlot[]> equipSlots = equipUI.Dict_EquipSlot;
        int i;
        if (isWear == false)
        {
            //장비 해제
            if (equipSlots.ContainsKey(type))
            {
                for (i = 0; i < equipSlots[type].Length; i++)
                {
                    if (equipSlots[type][i].item == item && equipSlots[type][i].SlotIndex == slotIndex)
                    {
                        AddInvenItem(item);
                        AddEquipItem(type, null, slotIndex);
                        break;
                    }
                }
            }
        }
        else
        {
            int index = 0;
            //장비 자리 있는지 확인
            if (item != null && equipSlots.ContainsKey(type))
            {
                bool isEmpty = false;

                for (i = 0; i < equipSlots[type].Length; i++)
                {
                    if (equipSlots[type][i].item == null)
                    {
                        isEmpty = true;
                        index = equipSlots[type][i].SlotIndex;
                        break;
                    }
                }


                if (isEmpty)
                {
                    //장비 자리가 있을 경우
                    // 그냥 추가
                    AddEquipItem(type, item, index);
                }
                else
                {
                    //장비 자리가 없을 경우
                    //장비 해제 , 장착
                    BaseItem tempItem = null;
                    int minSlotIndex = 0;
                    // 슬롯 인덱스 값이 낮은 곳을 우선적으로 변경
                    for (i = 0; i < equipSlots[type].Length; i++)
                    {
                        if (equipSlots[type][i].SlotIndex < minSlotIndex)
                        {
                            index = i;
                            minSlotIndex = equipSlots[type][i].SlotIndex;
                        }
                    }

                    tempItem = equipSlots[type][index].item;
                    AddInvenItem(tempItem);
                    AddEquipItem(type, item, minSlotIndex);
                }
            }
        }
        yield return new WaitForSeconds(1.0f);
        ActiveChangeEquip = false;
    }

    // True = 인벤토리 공간 여유로움, False = 인벤토리 공간 or 무게 가득 참 
    public bool CheckSlot(BaseItem newItem, int cnt = 1)
    {
        if (invenUI.CheckSlotFull(newItem, cnt))
        {
            if (MaxItemWeights < InvenWeight + (newItem.Weight * cnt))
                return false;
            else
                return true;
        }
        else
        {
            if (MaxItemWeights < InvenWeight + (newItem.Weight * cnt))
                return false;
            else
                return true;
        }        
    }

    public bool CheckSlotAndAddItem(List<ItemDatas> datas)
    {
        List<ItemDatas> insideItems = new List<ItemDatas>();
        bool isOk = false;
        int i = 0;
        for(; i < datas.Count; i++)
        {
            if (Dict_Item.ContainsKey(datas[i].index))
            {
                BaseItem itemData = Dict_Item[datas[i].index];
                if (CheckSlot(itemData, datas[i].count))
                {
                    //Possible                                
                    AddInvenItem(itemData, datas[i].count);
                    insideItems.Add(datas[i]);
                    isOk = true;
                }
                else
                {
                    //Impossible               
                    isOk = false;
                    break;
                }
            }
            else
            {
                isOk = false;
                break;
            }
        }

        if (!isOk)
        {
            if (insideItems.Count > 0)
            {
                //들어간만큼의 아이템을 다시 제거
                for (i = 0; i < insideItems.Count; i++)
                {
                    if(UseItem(insideItems[i]) == false)
                    {
                        Debug.Log("오류");
                    }
                    else
                    {
                        Debug.Log($"{Dict_Item[insideItems[i].index].NameKr}를 삭제했습니다.");
                    }
                }
            }
        }

        return isOk;
    }
        
    public void AddInvenItem(BaseItem newItem, int cnt = 1)
    {
        invenUI.AcquireItem(newItem, cnt);
    }

    public bool UseItem(int itemIndex, int count = 1)
    {
        bool isOk = false;
        foreach (var Data in Dict_SlotItem)
        {
            if (Data.Value.index == itemIndex && Data.Value.count >= count)
            {
                isOk = true;
                InventoryItems[Data.Key].SetSlotCount(-count);
                break;
            }
        }
        return isOk;
    }

    public bool UseItem(BaseItem item, int count)
    {
        return UseItem(item.Index, count);
    }

    public bool UseItem(ItemDatas datas)
    {
        return UseItem(datas.index, datas.count);
    }

    public void AddEquipItem(eEquipType type, BaseItem newItem, int slotIndex = 0)
    {
        equipUI.AcquireItem(type, newItem, slotIndex);
    }

    public ItemSlotAndCount CheckEnoughItem(int[] indexs, int[] costs)
    {
        UI_Slot[] slots = InventoryItems;
        int slotNum;
        int costValue;
        bool isEnough;
        List<int> slotNumbers = new List<int>();
        List<int> itemCosts = new List<int>();

        for (int i = 0; i < indexs.Length; i++)
        {
            slotNum = 0;
            costValue = 0;
            isEnough = false;
            for (int j = 0; j < slots.Length; j++)
            {
                if (slots[j].itemData != null)
                {
                    if (slots[j].itemData.Index == slotNumbers[i]
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

    
    public int GetActiveWeaponIndex(int slotIndex)
    {

        UI_EquipSlot data = equipUI.GetEquipData(slotIndex);
        if (data != null)
        {
            if (data.item == null)                            
                return 0;            
            else            
                return data.item.Index;           
        }
        else
            return 0;
    }
}
