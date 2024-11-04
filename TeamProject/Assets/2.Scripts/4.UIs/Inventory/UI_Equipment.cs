using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class UI_Equipment : UI_Base
{
    enum GameObjects
    {
        Slot_Parent
    }

    public List<UI_EquipSlot> Equip_Slots;
    public Dictionary<eEquipType, UI_EquipSlot[]> Dict_EquipSlot;
    public Dictionary<int, UI_EquipSlot> EquipBySlotIndex;
    
    GameObject _slotParent;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        _slotParent = GetObject((int)GameObjects.Slot_Parent);
        Dict_EquipSlot = new Dictionary<eEquipType, UI_EquipSlot[]>();
        
        SettingSlot();        
    }

    public void SettingSlot()
    {
        int i = 0;
        for (; i < Equip_Slots.Count; i++)
        {
            Equip_Slots[i].Init();
        }

        for(i = 1; i < (int)eEquipType.Max_Count; i++)
        {
            List<UI_EquipSlot> slotList = new List<UI_EquipSlot>();
            for(int j = 0; j < Equip_Slots.Count; j++)
            {
                if(Equip_Slots[j].slotType == (eEquipType)i)
                {
                    slotList.Add(Equip_Slots[j]);
                }
            }
            Dict_EquipSlot.Add((eEquipType)i, slotList.ToArray());
        }        
    }

    public void ChangeEquipData(int itemIndex, BaseItem changeData)
    {        
        foreach (var Data in EquipBySlotIndex)
        {
            if(Data.Value.item != null)
            {                
                if (Data.Value.item.Index == itemIndex)
                {
                    Data.Value.ChangeItemData(changeData);                    
                }
            }            
        }
    }

    public UI_EquipSlot GetEquipData(int slotIndex)
    {        
        EquipBySlotIndex = new Dictionary<int, UI_EquipSlot>();
        for(int i = 0; i < Equip_Slots.Count; i++)
        {            
            if (EquipBySlotIndex.ContainsKey(Equip_Slots[i].SlotIndex) == false)
            {
                EquipBySlotIndex.Add(Equip_Slots[i].SlotIndex, Equip_Slots[i]);
            }
        }

        if (EquipBySlotIndex.ContainsKey(slotIndex))        
            return EquipBySlotIndex[slotIndex];
        

        return null;
    }

    public Dictionary<eEquipType,BaseItem[]> GetEquipSlot()
    {
        Dictionary<eEquipType, BaseItem[]> dict = new Dictionary<eEquipType, BaseItem[]>();

        foreach(var slots in Dict_EquipSlot)
        {
            List<BaseItem> listItems = new List<BaseItem>();
            for(int i = 0; i < slots.Value.Length; i++)
            {
                if(slots.Value[i].item != null)
                {
                    listItems.Add(slots.Value[i].item);
                }                
            }
            if (listItems.Count > 0)
                dict.Add(slots.Key, listItems.ToArray());
            else
                dict.Add(slots.Key, null);
        }
        return dict;
    }


    public void AcquireItem(eEquipType type, BaseItem item, int slotIndex = 0)
    {               
        if(item == null)
        {

            foreach (var slot in Dict_EquipSlot[type])
            {
                if(slot.SlotIndex == slotIndex)
                {
                    slot.SetItem(item);
                    break;
                }
            }
            //아이템 제거
            //for (int i = 0; i < Dict_EquipSlot[type].Length; i++)
            //{
            //    //int index = (Dict_EquipSlot[type].Length - 1) - i;
            //    //Dict_EquipSlot[type][i].item != null &&
            //    if (Dict_EquipSlot[type][i].SlotIndex == slotIndex)
            //    {
            //        Dict_EquipSlot[type][i].SetItem(item);
            //        break;
            //    }
            //}

        }
        else
        {
            //아이템 추가
            for (int i = 0; i < Dict_EquipSlot[type].Length; i++)
            {
                if(Dict_EquipSlot[type][i].item == null)
                {
                    Dict_EquipSlot[type][i].SetItem(item);
                    break;
                }
            }
        }        
    }

}
