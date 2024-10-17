using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class WeaponItemInfo : BaseItem
{
    int[] _materialsIndex;
    int[] _materialsCost;
    float _damage;
    
    public int[] MaterialsIndex { get { return _materialsIndex; } }
    public int[] MaterialsCost { get { return _materialsCost; } }
   
    public float Damage { get { return _damage; } }

    public override void Init(int index, string nameEn, string desc, string spriteName, string nameKr, float weight)
    {
        base.Init(index, nameEn, desc, spriteName, nameKr, weight);
    }

    public void AddInit(int[] Indexs, int [] Costs, float damage)
    {
        _materialsIndex = Indexs;
        _materialsCost = Costs;
        _damage = damage;
    }

    public WeaponItemInfo(int index, string nameEn, string desc, string spriteName, string nameKr, float weight, int[] Indexs, int[] Costs, float damage)
    {
        Init(index, nameEn, desc, spriteName, nameKr, weight);
        AddInit(Indexs, Costs, damage);
              
        _type = eItemType.Equipment;
        _eType = eEquipType.Weapon;
    }
}
