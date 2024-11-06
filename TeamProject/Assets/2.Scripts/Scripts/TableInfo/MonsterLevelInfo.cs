using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLevelInfo
{
    protected int _index;
    protected int _requiredExp;
    protected float _rewardAbility;
    protected int _dropExp;

    public int Level { get { return _index; } }
    public int RequiredExp { get { return _requiredExp; } }
    public float RewardAbility { get { return _rewardAbility; } }
    public int DropExp { get { return _dropExp; } }


    public MonsterLevelInfo (int index, int requiredExp, float rewardAblity, int drop)
    {
        _index = index;
        _requiredExp = requiredExp;
        _rewardAbility = rewardAblity;
        _dropExp = drop;
    }


}
