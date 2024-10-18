using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class WeaponItemInfo : BaseItem
{
    int[] _materialsIndex;
    int[] _materialsCost;
    float _damage;
    int _ammo;
    int _currentAmmo;

    public int[] MaterialsIndex { get { return _materialsIndex; } }
    public int[] MaterialsCost { get { return _materialsCost; } }
   
    public float Damage { get { return _damage; } }
    public int Ammo { get { return _ammo; } }
    public int CurrentAmmo { get { return _currentAmmo; } }

    public override void Init(int index, string nameEn, string desc, string spriteName, string nameKr, float weight)
    {
        base.Init(index, nameEn, desc, spriteName, nameKr, weight);
    }

    public void AddInit(int[] Indexs, int [] Costs, float damage, int ammo)
    {
        _materialsIndex = Indexs;
        _materialsCost = Costs;
        _damage = damage;
        _ammo = ammo;
        _currentAmmo = ammo;
    }

    public WeaponItemInfo(int index, string nameEn, string desc, string spriteName, string nameKr, float weight, int[] Indexs, int[] Costs, float damage, int ammo)
    {
        Init(index, nameEn, desc, spriteName, nameKr, weight);
        AddInit(Indexs, Costs, damage, ammo);
              
        _type = eItemType.Equipment;
        _eType = eEquipType.Weapon;
    }
}
