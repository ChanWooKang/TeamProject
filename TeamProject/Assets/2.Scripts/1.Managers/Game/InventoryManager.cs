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
    public Dictionary<int, Sprite> Dict_itemSprite;
    
    public List<ItemSprite> itemSprites;
    public List<int> itemNames;
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
        if (Dict_itemSprite.ContainsKey(index))
        {
            return Dict_itemSprite[index];
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
            itemNames.Add(Items[i].Index);
        }

        Dict_itemSprite = new Dictionary<int, Sprite>();
        for (int i = 0; i < itemSprites.Count; i++)
        {
            if (Dict_itemSprite.ContainsKey(itemSprites[i].index) == false)
                Dict_itemSprite.Add(itemSprites[i].index, itemSprites[i].icon);
            else
                Dict_itemSprite[itemSprites[i].index] = itemSprites[i].icon;
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
            //��� ����
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
            //��� �ڸ� �ִ��� Ȯ��
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
                    //��� �ڸ��� ���� ���
                    // �׳� �߰�
                    AddEquipItem(type, item, index);
                }
                else
                {
                    //��� �ڸ��� ���� ���
                    //��� ���� , ����
                    BaseItem tempItem = null;
                    int minSlotIndex = 0;
                    // ���� �ε��� ���� ���� ���� �켱������ ����
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

    public bool CheckSlot(BaseItem newItem, int cnt = 1)
    {
        if (invenUI.CheckSlotFull(newItem, cnt))
        {
            if (MaxItemWeights < InvenWeight + (newItem.Weight * cnt))
                return true;
            else
                return false;
        }
        else
        {
            if (MaxItemWeights < InvenWeight + (newItem.Weight * cnt))
                return true;
            else
                return false;
        }        
    }

    public void AddInvenItem(BaseItem newItem, int cnt = 1)
    {
        invenUI.AcquireItem(newItem, cnt);
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



    //SlotIndex �� ���� ��Ű ��� 1 2 3 4
    public int GetActiveWeaponIndex(int slotIndex)
    {

        UI_EquipSlot data = equipUI.GetEquipData(slotIndex);
        if (data == null)
        {
            // �� �������� ��ȯ
            //Debug.Log("�κ��丮�Ŵ��� : ����");
            return 0;
        }
        else
        {
            if (data.item == null)
            {
                Debug.Log("�κ��丮�Ŵ��� : ������ ���� ����");
                return 0;
            }
            else
            {
                return data.item.Index;
            }

        }
    }
}
