using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_Slot : MonoBehaviour
{
    #region [ Component ] 
    public Image Item_Image;
    public Text Count_Text;
    public Text Weight_Text;
    public GameObject Count_Parent;
    public GameObject Weight_Parent;
    #endregion [ Component ]

    public SOItem item;
    public int itemCount;
    public float itemWeight;

    public void Init()
    {
        ClearSlot();
    }

    //아이템 존재 하거나 없을 때 알파 값 조정
    void SetAlpha(float alpha)
    {
        Color color = Item_Image.color;
        color.a = alpha;
        Item_Image.color = color;
    }

    //동일 아이템일때 중첩할 수 있는지 확인
    public bool CheckRestSlot(SOItem newItem,int cnt)
    {        
        if(item != null)
        {
            int count = itemCount + cnt;
            if (item.maxStack >= count)
                return true;
            else
                return false;
        }
        else
        {
            if (newItem.maxStack >= cnt)
                return true;
            else
                return false;
        }                    
    }

    public void SetSlotCount(int cnt)
    {
        itemCount += cnt;
        Count_Text.text = itemCount.ToString();
        itemWeight = item.itemWeight * itemCount;
        if (itemCount <= 0)
            ClearSlot();
    }

    public void AddItem(SOItem newItem, int cnt = 1)
    {
        item = newItem;
        itemCount = cnt;
        itemWeight = newItem.itemWeight;
        Item_Image.sprite = item.icon;
        Weight_Text.text = itemWeight.ToString();
        Weight_Parent.SetActive(true);
        if (item.itemType == eItemType.Equipment)
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
        item = null;
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
