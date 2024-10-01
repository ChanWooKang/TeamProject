using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLevelInfo
{
    protected int _index;
    protected float _requiredExp;
    protected float _rewardAbility;
    protected float _dropExp;

    public int Level { get { return _index; } }
    public float RequiredExp { get { return _requiredExp; } }
    public float RewardAbility { get { return _rewardAbility; } }
    public float DropExp { get { return _dropExp; } }


    public MonsterLevelInfo (int index, float requiredExp, float rewardAblity, float drop)
    {
        _index = index;
        _requiredExp = requiredExp;
        _rewardAbility = rewardAblity;
        _dropExp = drop;
    }


}
