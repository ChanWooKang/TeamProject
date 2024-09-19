using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable/Item")]
public class SOItems : ScriptableObject
{
    //아이템 아이디
    public int itemID;
    //아이템 이름
    public string itemName;
    //아이템 타입 ( 장비, 소비, 기타 등)
    public eItemType itemType;
    //아이템 스택 최대치 ( 장비 아이템은 1)
    public int maxStack;
    //아이템 가격
    public int price;
    //아이템 거래 가능 여부
    public bool isTradeItem;
    //아이템 무게
    public float itemWeight;

    //제작 아이템 경우 
    //필요 아이템
    public List<RequiredItem> NeedItems;

    //UI 아이콘
    public Sprite icon;
    //UI 아이템 이름 한글 작성
    public string krName;
    //UI 아이템 설명
    [Multiline]
    public string desc;
}


