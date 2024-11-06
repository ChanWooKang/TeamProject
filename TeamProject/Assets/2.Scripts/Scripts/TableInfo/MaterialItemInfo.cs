using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MaterialItemInfo : BaseItem
{

    protected int _rate;
    
    public int Rate { get { return _rate; } }


    public override void Init(int index, string nameEn, string desc, string spriteName, string nameKr, float weight)
    {
        base.Init(index, nameEn, desc, spriteName, nameKr, weight);
        
    }

    public MaterialItemInfo(int index, string nameEn, string desc, string spriteName, string nameKr, float weight, int rate)
    {
        Init(index, nameEn, desc, spriteName, nameKr, weight);
        _rate = rate;
        _type = eItemType.Material;
    }
}
