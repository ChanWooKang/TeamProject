using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_Slot : UI_Base, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    enum GameObjects
    {
        ItemIcon,
        CountParent,
        CountText,
        WeightParent,
        WeightText
    }

    [Header("Components")]
    Image Item_Image;
    Text Count_Text;
    Text Weight_Text;
    GameObject Count_Parent;
    GameObject Weight_Parent;

    public int slotIndex;
    public BaseItem itemData;
    public int itemCount;
    public float itemWeight;
    bool isSetting = false;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Item_Image = GetObject((int)GameObjects.ItemIcon).GetComponent<Image>();
        Count_Text = GetObject((int)GameObjects.CountText).GetComponent<Text>();
        Weight_Text = GetObject((int)GameObjects.WeightText).GetComponent<Text>();
        Count_Parent = GetObject((int)GameObjects.CountParent);
        Weight_Parent = GetObject((int)GameObjects.WeightParent);        
        ClearSlot();
        isSetting = true;
    }

    void SetAlpha(float alpha)
    {
        Color color = Item_Image.color;
        color.a = alpha;
        Item_Image.color = color;
    }

    public bool CheckRestSlot(BaseItem newItem, int cnt)
    {
        if(itemData != null)
        {
            int count = itemCount + cnt;
            if (itemData.MaxStack >= count)
                return true;
            else
                return false;
        }
        else
        {
            if (newItem.MaxStack >= cnt)
                return true;
            else
                return false;
        }
    }

    public void SetSlotCount(int cnt)
    {
        itemCount += cnt;
        Count_Text.text = itemCount.ToString();
        ChangeWeight(itemData.Weight * itemCount);        
        if (itemCount <= 0)
            ClearSlot();
        else
            ChangeSlotData();
    }

    public void AddItem(BaseItem newItem, int cnt = 1)
    {        
        itemData = newItem;
        itemCount = cnt;
        ChangeWeight(newItem.Weight);
        Item_Image.sprite = InventoryManager._inst.GetItemSprite(itemData.Index);
        Weight_Text.text = itemWeight.ToString();
        Weight_Parent.SetActive(true);
        if (itemData.Type == eItemType.Equipment)
        {
            Count_Text.text = "";
            Count_Parent.SetActive(false);
        }
        else
        {
            Count_Text.text = itemCount.ToString();
            Count_Parent.SetActive(true);
        }
        SetAlpha(1);       
        ChangeSlotData();
    }

    void ChangeWeight(float weight)
    {
        float tempWeight = weight - itemWeight;
        if(tempWeight != 0)
        {
            InventoryManager._inst.InventoryWeight += tempWeight;
            Debug.Log(tempWeight);
        }
        itemWeight = weight;
    }

    void ChangeSlotData()
    {
        int index = 0;
        if (itemData != null)
            index = itemData.Index;
        ItemDatas datas = new ItemDatas(index, itemCount);
        InventoryManager._inst.ChangeInventoryData(slotIndex, datas);        
    }

    public void ClearSlot()
    {
        itemData = null;
        itemCount = 0;
        ChangeWeight(0);
        Item_Image.sprite = null;
        Count_Text.text = "";
        Count_Parent.SetActive(false);
        Weight_Text.text = "";
        Weight_Parent.SetActive(false);
        SetAlpha(0);

        if (isSetting)
        {
            ChangeSlotData();
        }
    }

    void ChangeSlot()
    {
        BaseItem tempItem = itemData;
        int tempCount = itemCount;
        AddItem(DragSlot._inst.SlotInven.itemData, DragSlot._inst.SlotInven.itemCount);
        if (tempItem != null)
            DragSlot._inst.SlotInven.AddItem(tempItem, tempCount);
        else
            DragSlot._inst.SlotInven.ClearSlot();

    }

    //우클릭 장착 혹은 사용
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {            
            if (itemData != null)
            {
                switch (itemData.Type)
                {
                    case eItemType.Equipment:                    
                        if(InventoryManager.ActiveChangeEquip == false)
                        {
                            InventoryManager._inst.OnChangeEvent?.Invoke(itemData.EquipType, itemData, 0, true);
                            ClearSlot();
                        }
                        break;                    
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemData != null)
        {
            UI_ItemInfo._info.OffInformation();
            DragSlot._inst.isFromInven = true;
            DragSlot._inst.SetCanvas(false);
            DragSlot._inst.SlotInven = this;
            DragSlot._inst.DragSetImage(Item_Image);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemData != null)
        {            

            DragSlot._inst._rect.position = eventData.position;            
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {       
        DragSlot._inst.SetAlpha(0);
        DragSlot._inst.SetCanvas(true);
        DragSlot._inst.SlotInven = null;
        DragSlot._inst._rect.position = Vector2.zero;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot._inst.isFromInven)
        {
            if (DragSlot._inst.SlotInven != null)
                ChangeSlot();
        }
        else
        {
            if(InventoryManager.ActiveChangeEquip == false)
            {
                UI_EquipSlot slot = DragSlot._inst.SlotEquip;
                if (InventoryManager._inst.CheckSlot(slot.item))
                {
                    InventoryManager._inst.OnChangeEvent?.Invoke(slot.item.EquipType, slot.item, slot.SlotIndex, false);
                    DragSlot._inst.SlotEquip.ClearSlot();
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemData != null)
        {
            UI_ItemInfo._info.SetInformation(itemData, transform.position, itemData.Type, itemCount, eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UI_ItemInfo._info.OffInformation();
    }
}
