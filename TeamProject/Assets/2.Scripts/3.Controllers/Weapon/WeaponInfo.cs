using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo
{
    public int Index { get; set; }
    public string NameEn { get; set; }
    public string NameKr { get; set; }
    public string Desc { get; set; }
    public string SpriteName { get; set; }
    public int[] MaterialsIndex{get;set;}
    public int[] MaterialsCost { get; set; }
    public float Damage { get; set; }
    public float Weight { get; set; }

    public WeaponInfo(int index, string enName, string krName, string desc, string spriteName,int[] MatIndex, int[]MatCost, float damage, float weight)
    {
        Index = index;
        NameEn = enName;
        NameKr = krName;
        Desc = desc;
        SpriteName = spriteName;
        MaterialsIndex = MatIndex;
        MaterialsCost = MatCost;
        Damage = damage;
        Weight = weight;
    }
}
