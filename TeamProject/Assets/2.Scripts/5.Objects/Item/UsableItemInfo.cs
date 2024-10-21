using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
public class UsableItemInfo : BaseItem
{
    int[] _materialsIndex;
    int[] _materialsCost;
    int value;

    public int[] MaterialsIndex { get { return _materialsIndex; } }
    public int[] MaterialsCost { get { return _materialsCost; } }

    public int Value { get { return value; } }
    public override void Init(int index, string nameEn, string desc, string spriteName, string nameKr, float weight)
    {
        base.Init(index, nameEn, desc, spriteName, nameKr, weight);
    }
    public void AddInit(int[] Indexs, int[] Costs, int value)
    {
        _materialsIndex = Indexs;
        _materialsCost = Costs;
        this.value = value;
    }
    public UsableItemInfo(int index, string nameEn, string desc, string spriteName, string nameKr, float weight, int[] Indexs, int[] Costs, int value, int type)
    {
        Init(index, nameEn, desc, spriteName, nameKr, weight);
        AddInit(Indexs, Costs, value);

        _type = eItemType.Usable;
        _uType = (eUsableType)type;
    }
}
