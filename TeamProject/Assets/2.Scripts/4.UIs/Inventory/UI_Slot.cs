using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;
using System.Threading;

public class UI_Slot : UI_Base, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    enum GameObjects
    {        
        CountParent,        
        WeightParent,
        LevelParent
    }

    enum Texts
    {
        CountText,
        WeightText,
        LevelText
    }

    enum Images
    {
        ItemIcon,

    }

    [Header("Components")]
    Image Item_Image;
    Text Count_Text;
    Text Weight_Text;
    Text Level_Text;
    GameObject Count_Parent;
    GameObject Weight_Parent;
    GameObject Level_Parent;
    RectTransform m_canvasTF;

    public int slotIndex;
    public BaseItem itemData;
    public int itemCount;
    public float itemWeight;
    public int itemLevel;
    bool isSetting = false;
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
        Item_Image = GetImage((int)Images.ItemIcon);
        Count_Parent = GetObject((int)GameObjects.CountParent);
        Weight_Parent = GetObject((int)GameObjects.WeightParent);
        Level_Parent = GetObject((int)GameObjects.LevelParent);
        Count_Text = GetText((int)Texts.CountText);
        Weight_Text = GetText((int)Texts.WeightText);
        Level_Text = GetText((int)Texts.LevelText);        
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

    void ChangeUI(BaseItem newItem = null) 
    {
        //아이템 클리어 하는건지 세팅하는건지 확인
        bool isClear = newItem == null;
        Sprite icon = isClear ? null : InventoryManager._inst.GetItemSprite(newItem.Index);
        float alpha = isClear ? 0f : 1.0f;        

        Weight_Parent.SetActive(itemWeight > 0);
        if (isClear)
        {            
            Count_Parent.SetActive(false);
            Level_Parent.SetActive(false);
        }
        else
        {                        
            bool isEquipment = newItem.Type == eItemType.Equipment || newItem.Type == eItemType.Weapon;
            Count_Parent.SetActive(!isEquipment);
            Level_Parent.SetActive(isEquipment && itemLevel > 0);
        }

        Item_Image.sprite = icon;        
        Count_Text.text = itemCount.ToString();
        Level_Text.text = itemLevel.ToString();

        SetAlpha(alpha);
    }

    public void ChangeItemData(BaseItem changeData)
    {
        itemData = changeData;
        itemLevel = changeData.Level;
        ChangeUI(changeData);
    }

    public void AddItem(BaseItem newItem, int cnt = 1)
    {        
        itemData = newItem;
        itemCount = cnt;
        itemLevel = newItem.Level;
        ChangeWeight(newItem.Weight);
        ChangeUI(newItem);
        ChangeSlotData();
    }

    void ChangeWeight(float weight)
    {
        float tempWeight = weight - itemWeight;
        if(tempWeight != 0)
        {
            InventoryManager._inst.InventoryWeight += tempWeight;            
        }
        itemWeight = weight;
        Weight_Text.text = itemWeight.ToString();
    }

    void ChangeSlotData()
    {
        int index = 0;
        if (itemData != null)
            index = itemData.Index;
        ItemDatas datas = new ItemDatas(index, itemCount,itemLevel);
        InventoryManager._inst.ChangeInventoryData(slotIndex, datas);        
    }

    public void ClearSlot()
    {
        itemData = null;
        itemCount = 0;
        itemLevel = 0;
        ChangeWeight(0);
        ChangeUI();
        SetAlpha(0);
        if (isSetting)
            ChangeSlotData();
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
            m_canvasTF = DragSlot._inst.gameObject.GetComponent<RectTransform>();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemData != null)
        {            

            //DragSlot._inst._rect.position = eventData.position;
            Vector3 screenPos = eventData.position;

            Vector3 newPos = Vector3.zero;

            Camera cam = eventData.pressEventCamera;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_canvasTF, screenPos, cam, out newPos))
            {
                DragSlot._inst.gameObject.transform.position = newPos;
                DragSlot._inst.gameObject.transform.rotation = m_canvasTF.rotation;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {       
        DragSlot._inst.SetAlpha(0);
        DragSlot._inst.SetCanvas(true);
        DragSlot._inst.SlotInven = null;
        //DragSlot._inst._rect.position = Vector2.zero;
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
