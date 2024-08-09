using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChanWooDefineDatas;

[CreateAssetMenu(fileName = "Item", menuName ="Scriptable/Item")]
public class ChanWooSOItem : ScriptableObject
{
    public string itemName;
    public eItemType itemType;    
    public int maxStack;

    //UIs
    public Sprite icon;
    public string krName;
    [Multiline]
    public string description;
    public int price;
    //거래 가능 한지 여부
    public bool isTradingItem;
}
