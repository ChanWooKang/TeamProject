using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_EquipSlot : UI_Base, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,IPointerEnterHandler,IPointerExitHandler
{
    enum GameObjects
    {        
        WeightParent,
        LevelParent
    }

    enum Texts
    {
        WeightText,
        LevelText
    }

    enum Images
    {
        ItemIcon
    }

    [Header("Components")]
    public eEquipType slotType;
    public BaseItem item;

    Image Item_Image;
    GameObject Weight_Parent;
    GameObject Level_Parent;
    Text Weight_Text;
    Text Level_Text;
    public int SlotIndex;    
    public float itemWeight;
    public int itemLevel;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Item_Image = GetImage((int)Images.ItemIcon);
        Weight_Parent = GetObject((int)GameObjects.WeightParent);
        Level_Parent = GetObject((int)GameObjects.LevelParent);
        Weight_Text = GetText((int)Texts.WeightText);
        Level_Text = GetText((int)Texts.LevelText);
        ClearSlot();
    }

    void SetAlpha(float alpha)
    {
        Color color = Item_Image.color;
        color.a = alpha;
        Item_Image.color = color;
    }

    void ChangeWeight(float weight)
    {
        float tempWeight = weight - itemWeight;
        if (tempWeight != 0)
        {
            InventoryManager._inst.InventoryWeight += tempWeight;
        }
        itemWeight = weight;
        Weight_Text.text = itemWeight.ToString();
    }

    public void ChangeItemData(BaseItem changeData)
    {
        item = changeData;
        itemLevel = changeData.Level;
        ChangeUI(changeData);
    }

    void ChangeUI(BaseItem newItem = null)
    {
        bool isClear = newItem == null;
        Sprite icon = isClear ? null : InventoryManager._inst.GetItemSprite(newItem.Index);
        float alpha = isClear ? 0f : 1f;
        Weight_Parent.SetActive(itemWeight > 0);        
        Level_Parent.SetActive(!isClear && itemLevel > 0);

        Item_Image.sprite = icon;        
        Level_Text.text = itemLevel.ToString();
        SetAlpha(alpha);
    }

    public void SetItem(BaseItem newItem = null)
    {
        if (newItem != null)
        {                            
            switch (newItem.EquipType)
            {
                case eEquipType.Weapon:
                    GameManagerEx._inst.playerManager._equip.ChangeSlotWeapon(SlotIndex, newItem.Index);
                    break;
                case eEquipType.Head:
                case eEquipType.Armor:
                    InventoryManager._inst.EquipItem(newItem.Index,newItem.EquipType, true);
                    break;
            }
            
            item = newItem;
            itemLevel = newItem.Level;
            ChangeWeight(newItem.Weight);
            ChangeUI(newItem);
        }
        else
            ClearSlot();
    }

    public void ClearSlot()
    {
        if (item != null)
        {
            switch (item.EquipType)
            {
                case eEquipType.Weapon:
                    GameManagerEx._inst.playerManager._equip.ChangeSlotWeapon(SlotIndex, 0);
                    break;
                case eEquipType.Head:
                case eEquipType.Armor:
                    InventoryManager._inst.EquipItem(item.Index, item.EquipType, false);                    
                    break;
            }               
        }            
        item = null;
        itemLevel = 0;
        ChangeWeight(0);
        ChangeUI();        
        UI_ItemInfo._info.OffInformation();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {            
            if (item != null)
            {                
                if (InventoryManager.ActiveChangeEquip == false)
                {
                    if (InventoryManager._inst.CheckSlot(item))
                    {
                        InventoryManager._inst.OnChangeEvent?.Invoke(slotType, item, SlotIndex, false);
                        ClearSlot();
                    }                    
                }
            }
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            if (InventoryManager.ActiveChangeEquip == false)
            {
                if (InventoryManager._inst.CheckSlot(item))
                {
                    UI_ItemInfo._info.OffInformation();
                    DragSlot._inst.isFromInven = false;
                    DragSlot._inst.SlotEquip = this;
                    DragSlot._inst.DragSetImage(Item_Image);
                    DragSlot._inst.SetCanvas(false);
                }
            }
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot._inst._rect.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot._inst.SetAlpha(0);
        DragSlot._inst.SetCanvas(true);
        DragSlot._inst.SlotInven = null;
        DragSlot._inst.SlotEquip = null;
        DragSlot._inst._rect.position = Vector3.zero;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot._inst.SlotInven != null)
        {
            UI_Slot slot = DragSlot._inst.SlotInven;

            if (slot.itemData.EquipType == slotType && slot.itemData.Type == eItemType.Equipment)
            {
                if (InventoryManager.ActiveChangeEquip == false)
                {
                    InventoryManager._inst.OnChangeEvent?.Invoke(slot.itemData.EquipType, slot.itemData, SlotIndex, true);
                    DragSlot._inst.SlotInven.ClearSlot();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item != null)
        {
            UI_ItemInfo._info.SetInformation(item, transform.position, item.Type, 1, eventData);            
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_ItemInfo._info.OffInformation();
    }
}
