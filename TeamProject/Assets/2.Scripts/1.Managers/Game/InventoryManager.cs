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
    public Dictionary<int, EquipmentItemInfo> Dict_Equipment;
    public Dictionary<int, PetBallInfo> Dict_Petball;
    public Dictionary<int, ItemDatas> Dict_SlotItem;
    
    public static bool ActiveChangeEquip = false;
    public int itemCount;    

    Coroutine EquipCoroutine = null;

    public float MaxItemWeights { get { return GameManagerEx._inst.playerManager._stat.CarryWeight; } }    
    public UI_Slot[] InventoryItems { get { return invenUI.GetInvenSlots; } }

    public float InventoryWeight;

    private void Awake()
    {
        Items = new List<BaseItem>();
        Dict_Material = new Dictionary<int, MaterialItemInfo>();
        Dict_Weapon = new Dictionary<int, WeaponItemInfo>();
        Dict_Equipment = new Dictionary<int, EquipmentItemInfo>();
        Dict_Petball = new Dictionary<int, PetBallInfo>();
        

        AddItems(LowDataType.MaterialTable);
        AddItems(LowDataType.WeaponTable);
        AddItems(LowDataType.EquipmentTable);
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
        InventoryWeight = 0;
        invenUI.Init();
        equipUI.Init();
        SetDictionary();
        SetInitInventoryData();
        AddInvenItem(Dict_Item[200]);
        AddInvenItem(Dict_Item[201]);
        AddInvenItem(Dict_Item[202]);
        AddInvenItem(Dict_Item[203]);
        AddInvenItem(Dict_Item[204]);
        AddInvenItem(Dict_Item[101],5);

        AddInvenItem(Dict_Item[102], 100);
        AddInvenItem(Dict_Item[100], 100);
        AddInvenItem(Dict_Item[300]);
        AddInvenItem(Dict_Item[301]);
    }

        


    #region [ Item Data Load ]
    public bool ChangeItemLevel(int itemIndex, int itemLevel)
    {
        if (Dict_Item.ContainsKey(itemIndex))
        {
            Dict_Item[itemIndex].Level = itemLevel;
            int[] itemSlotIndexs = GetSlotIndex(itemIndex);
            if (itemSlotIndexs.Length > 0)
            {
                for (int i = 0; i < itemSlotIndexs.Length; i++)
                {
                    ItemDatas datas = Dict_SlotItem[itemSlotIndexs[i]];
                    ItemDatas changeDatas = new ItemDatas(itemIndex, datas.count, itemLevel);
                    ChangeInventoryData(itemSlotIndexs[i], changeDatas);
                    InventoryItems[itemSlotIndexs[i]].ChangeItemData(Dict_Item[itemIndex]);
                }
            }                            
            
            if(Dict_Item[itemIndex].Type == eItemType.Equipment)
            {
                ChangeEquipLevel(Dict_Item[itemIndex].EquipType, itemIndex, itemLevel);
            }
               
            return true;
        }
        

        return false;
    }

    void ChangeEquipLevel(eEquipType equipType,int itemIndex, int itemLevel)
    {
        equipUI.ChangeEquipData(itemIndex, Dict_Item[itemIndex]);
        switch (equipType)
        {
            case eEquipType.Weapon:
                if (Dict_Weapon.ContainsKey(itemIndex))
                {
                    Dict_Weapon[itemIndex].Level = itemLevel;
                    GameManagerEx._inst.playerManager._equip.ChangeWeaponLevelEffect(itemIndex);
                }
                break;
            
        }        
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
            case LowDataType.EquipmentTable:
                offSetNum = 300;
                itemType = eItemType.Equipment;
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
            ItemDatas datas = new ItemDatas(index, slots[i].itemCount,slots[i].itemLevel);
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
            case eItemType.Equipment:
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

                    if(type == eItemType.Weapon)
                    {
                        float damage = Table.ToFloat(index, "Damage");
                        WeaponItemInfo weapon = new WeaponItemInfo(index, nameEn, desc, spriteName, nameKr, weight, materialsIndexArray, materialsCostArray, damage);
                        Items.Add(weapon);
                        Dict_Weapon.Add(index, weapon);
                    }
                    else
                    {
                        int equiptype = Table.ToInt(index, "Type");
                        float hp = Table.ToFloat(index, "HP");
                        EquipmentItemInfo equip = new EquipmentItemInfo(index, nameEn, desc, spriteName, nameKr, weight, materialsIndexArray, materialsCostArray, hp, equiptype);
                        Items.Add(equip);
                        Dict_Equipment.Add(index, equip);
                    }
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

    public void EquipItem(int index, eEquipType type, bool isEquip)
    {
        if (Dict_Equipment.ContainsKey(index))
            GameManagerEx._inst.playerManager._equip.EquipItem(index, type, Dict_Equipment[index], isEquip);
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
        if (invenUI.CheckSlot(newItem, cnt))
        {
            if (MaxItemWeights >= InventoryWeight + (newItem.Weight * cnt))
                return true;
            else
                return false;
        }
        else        
            return false;
                
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

    public int GetItemCount(int itemIndex)
    {
        int count = 0;

        foreach(var Data in Dict_SlotItem)
        {
            if (Data.Value.index == itemIndex)
                count += Data.Value.count;
        }

        return count;
    }

    public int[] GetSlotIndex(int itemIndex)
    {
        List<int> slotIndexs = new List<int>();
        foreach(var Data in Dict_SlotItem)
        {
            if(Data.Value.index == itemIndex)
            {
                slotIndexs.Add(Data.Key);
            }
        }

        return slotIndexs.ToArray();
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

    public bool UseItem(BaseItem item, int count = 1)
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
