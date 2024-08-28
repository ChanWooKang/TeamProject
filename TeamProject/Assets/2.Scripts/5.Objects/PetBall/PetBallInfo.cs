using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBallInfo : BaseItem
{
    float m_bonusRate;
    int[] m_materialsIndex;
    int[] m_materialsCost;

    public float BonusRate { get { return m_bonusRate; } }
    public int[] MaterialsIndex { get { return m_materialsIndex; } }
    public int[] MaterialsCost { get { return m_materialsCost; } }

    public override void Init(int index, string nameEn, string desc, string spriteName, string nameKr, float weight)
    {
        base.Init(index, nameEn, desc, spriteName, nameKr, weight);
    }
    public void AddInit(int[] indexes, int[]costs, float bonusRate)
    {
        m_materialsIndex = indexes;
        m_materialsCost = costs;
        m_bonusRate = bonusRate;
    }

    public PetBallInfo(int index, string nameEn, string desc, string spriteName, string nameKr, float weight, int[] Indexs, int[] Costs, float bonusRate)
    {
        Init(index, nameEn, desc, spriteName, nameKr, weight);
        AddInit(Indexs, Costs, bonusRate);

        _type = DefineDatas.eItemType.PetBall;
    }
}
