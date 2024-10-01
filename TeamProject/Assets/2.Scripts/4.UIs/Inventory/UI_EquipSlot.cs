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
        ItemIcon,
        WeightParent,
        WeightText
    }

    [Header("Components")]
    public eEquipType slotType;
    public BaseItem item;

    Image Item_Image;
    GameObject Weight_Parent;
    Text Weight_Text;
    public int SlotIndex;
    public float itemWeight;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Item_Image = GetObject((int)GameObjects.ItemIcon).GetComponent<Image>();
        Weight_Parent = GetObject((int)GameObjects.WeightParent);
        Weight_Text = GetObject((int)GameObjects.WeightText).GetComponent<Text>();
        ClearSlot();
    }

    void SetAlpha(float alpha)
    {
        Color color = Item_Image.color;
        color.a = alpha;
        Item_Image.color = color;
    }

    public void SetItem(BaseItem _item = null)
    {
        if (_item != null)
        {
            if (_item.EquipType == eEquipType.Weapon)
            {
                GameManagerEx._inst.playerManager._equip.ChangeSlotWeapon(SlotIndex, _item.Index);
                
            }                
            item = _item;
            Item_Image.sprite = InventoryManager._inst.GetItemSprite(item.Index) ;
            itemWeight = item.Weight;
            Weight_Text.text = itemWeight.ToString();
            Weight_Parent.SetActive(true);
            SetAlpha(1);
        }
        else
            ClearSlot();
    }

    public void ClearSlot()
    {
        if (item != null)
        {
            if(item.EquipType == eEquipType.Weapon)
            {                                
                GameManagerEx._inst.playerManager._equip.ChangeSlotWeapon(SlotIndex, 0);
            }                
        }            
        item = null;
        Item_Image.sprite = null;
        itemWeight = 0;
        Weight_Text.text = "";
        Weight_Parent.SetActive(false);
        SetAlpha(0);
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
                    if (InventoryManager._inst.CheckSlot(item) == false)
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
                if (InventoryManager._inst.CheckSlot(item) == false)
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
