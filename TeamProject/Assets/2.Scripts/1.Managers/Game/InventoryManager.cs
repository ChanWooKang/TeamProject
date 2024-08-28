using System;
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
    public UI_Equipment equipUI;

    //Action
    public Action<eEquipType, BaseItem, int, bool> OnChangeEvent;

    public List<BaseItem> Items;
    public Dictionary<int, BaseItem> Dict_Item;
    public Dictionary<int, WeaponItemInfo> Dict_Weapon;
    public Dictionary<int, PetBallInfo> Dict_Petball;
    public Dictionary<int, Sprite> Dict_itemSprite;
    public List<ItemSprite> itemSprites;
    public List<int> itemNames;
    PlayerManager playerManager;

    public static bool ActiveChangeEquip = false;
    public int itemCount;
    public float MaxItemWeights;

    Coroutine EquipCoroutine = null;

    public float InvenWeight { get { return invenUI.GetItemWeights(); } }
    public UI_Slot[] InventoryItems { get { return invenUI.GetInvenSlots; } }

    private void Awake()
    {
        _uniqueInstance = this;
        Items = new List<BaseItem>();
        Dict_Weapon = new Dictionary<int, WeaponItemInfo>();
        Dict_Petball = new Dictionary<int, PetBallInfo>();

        AddItems(LowDataType.MaterialTable);
        AddItems(LowDataType.WeaponTable);
        AddItems(LowDataType.PetBallTable);
    }

    private void Start()
    {
        //Test
        Init();
        OnChangeEvent -= ChangeEquipment;
        OnChangeEvent += ChangeEquipment;
    }

    public void Init()
    {

        itemCount = Items.Count;
        invenUI.Init();
        equipUI.Init();
        SetDictionary();
        AddInvenItem(Dict_Item[202]);

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
                MaterialItemInfo material = new MaterialItemInfo(index, nameEn, desc, spriteName, nameKr, weight);
                Items.Add(material);
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



    //SlotIndex 로 무기 핫키 출력 1 2 3 4
    public int GetActiveWeaponIndex(int slotIndex, PlayerManager manager = null)
    {
        if (playerManager == null)
            playerManager = manager;

        UI_EquipSlot data = equipUI.GetEquipData(slotIndex);
        if (data == null)
        {
            // 비 무장으로 전환
            Debug.Log("인벤토리매니저 : 비무장");
            return -1;
        }
        else
        {
            if (data.item == null)
            {
                Debug.Log("인벤토리매니저 : 아이템 정보 없음");
                return -1;
            }
            else
            {
                return data.item.Index;
            }

        }


    }
}
