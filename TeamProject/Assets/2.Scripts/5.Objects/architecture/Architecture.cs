using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class Architecture
{
    public int Index { get; set; }
    public string NameEn { get; set; }
    public string NameKr { get; set; }
    public string Desc { get; set; }
    public string SpriteName { get; set; }
    public int[] MaterialsIndex { get; set; }
    public int[] MaterialsCost { get; set; }
    public float Progress { get; set; }

    public Architecture(int index)
    {
        LowBase archTable = Managers._table.Get(LowDataType.ArchitectureTable);
        Index = index;
        NameEn = archTable.ToStr(index, "NameEn");
        NameKr = archTable.ToStr(index, "NameKr");
        Desc = archTable.ToStr(index, "Desc");
        SpriteName = archTable.ToStr(index, "SpriteName");
        string materialsIndexStr = archTable.ToStr(Index, "Materials");
        string[] materialsIndexStrArray = materialsIndexStr.Split('/');
        MaterialsIndex = new int[materialsIndexStrArray.Length];
        string materialsCost = archTable.ToStr(Index, "MaterialsCost");
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
        Progress = archTable.ToFloat(index, "Progress");
    }

}
