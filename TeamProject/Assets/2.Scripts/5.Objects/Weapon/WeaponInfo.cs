using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
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

    public WeaponInfo(int index)
    {
        LowBase weaponTable = Managers._table.Get(LowDataType.WeaponTable);
        Index = 200 + index;
        NameEn = weaponTable.ToStr(Index, "NameEn");
        NameKr = weaponTable.ToStr(Index, "NameKr");
        Desc = weaponTable.ToStr(Index, "Desc");
        SpriteName = weaponTable.ToStr(Index, "SpriteName");       
        Damage = weaponTable.ToFloat(Index, "Damage");
        Weight = weaponTable.ToFloat(Index, "Weight");

        string materialsIndexStr = weaponTable.ToStr(Index, "Materials");
        string[] materialsIndexStrArray = materialsIndexStr.Split('/');
        MaterialsIndex = new int[materialsIndexStrArray.Length];
        string materialsCost = weaponTable.ToStr(Index, "MaterialsCost");
        string[] materialsCostStrArray = materialsCost.Split('/');
        MaterialsCost = new int[materialsCostStrArray.Length];
        for (int i = 0; i < materialsIndexStrArray.Length; i++)
        {
            if (int.TryParse(materialsIndexStrArray[i], out int number))
            {
                MaterialsIndex[i] = number;
            }
            if (int.TryParse(materialsCostStrArray[i], out int costs))
            {
                MaterialsCost[i] = costs;
            }
        }                  
    }
}
