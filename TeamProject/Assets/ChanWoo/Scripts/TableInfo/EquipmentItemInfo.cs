using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class EquipmentItemInfo : BaseItem
{
    int[] _materialsIndex;
    int[] _materialsCost;
    float _hp;

    public int[] MaterialsIndex { get { return _materialsIndex; } }
    public int[] MaterialsCost { get { return _materialsCost; } }

    public float HP { get { return _hp; } }
    public override void Init(int index, string nameEn, string desc, string spriteName, string nameKr, float weight)
    {
        base.Init(index, nameEn, desc, spriteName, nameKr, weight);
    }

    public void AddInit(int[] Indexs, int[] Costs, float hp)
    {
        _materialsIndex = Indexs;
        _materialsCost = Costs;
        _hp = hp;
    }

    public EquipmentItemInfo(int index, string nameEn, string desc, string spriteName, string nameKr, float weight, int[] Indexs, int[] Costs,float hp, int type)
    {
        Init(index, nameEn, desc, spriteName, nameKr, weight);
        AddInit(Indexs, Costs, hp);

        _type = eItemType.Equipment;
        _eType = (eEquipType)type;
    }
}
