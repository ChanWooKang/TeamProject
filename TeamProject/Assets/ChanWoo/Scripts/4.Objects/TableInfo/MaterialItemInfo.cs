using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MaterialItemInfo : BaseItem
{


    public override void Init(int index, string nameEn, string desc, string spriteName, string nameKr, float weight)
    {
        base.Init(index, nameEn, desc, spriteName, nameKr, weight);
        _type = eItemType.Material;       
    }

    public MaterialItemInfo(int index, string nameEn, string desc, string spriteName, string nameKr, float weight)
    {
        Init(index, nameEn, desc, spriteName, nameKr, weight);
    }
}
