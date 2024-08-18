using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_Slot : MonoBehaviour
{
    [Header("Components")]
    public Image Item_Image;
    public Text Count_Text;
    public Text Weight_Text;
    public GameObject Count_Parent;
    public GameObject Weight_Parent;

    public BaseItem itemData;
    public int itemCount;
    public float itemWeight;

    public void Init()
    {
        ClearSlot();
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
        itemWeight = itemData.Weight * itemCount;
        if (itemCount <= 0)
            ClearSlot();
    }

    public void AddItem(BaseItem newItem, int cnt = 1)
    {
        itemData = newItem;
        itemCount = cnt;
        itemWeight = newItem.Weight;
        //Item_Image.sprite = itemData.icon;
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
    }

    public void ClearSlot()
    {
        itemData = null;
        itemCount = 0;
        itemWeight = 0;
        Item_Image.sprite = null;
        SetAlpha(0);
        Count_Text.text = "";
        Count_Parent.SetActive(false);
        Weight_Text.text = "";
        Weight_Parent.SetActive(false);
    }
}
