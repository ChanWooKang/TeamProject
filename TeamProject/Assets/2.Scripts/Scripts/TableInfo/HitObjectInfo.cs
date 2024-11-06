using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjectInfo
{
    protected int _index;
    protected string _nameKr;
    protected string _nameEn;
    protected float _hp;
    protected int _reward;
    protected int _rewardCount;

    public int Index { get { return _index; } }
    public string NameEn { get { return _nameEn; } }
    public string NameKr { get { return _nameKr; } }
    public float HP { get { return _hp; } }
    public int RewardIndex { get { return _reward; } }
    public int RewardCount { get { return _rewardCount; } }

    public HitObjectInfo(int index, string nameKr, string nameEn, float hp, int reward, int rewardCount)
    {
        _index = index;
        _nameKr = nameKr;
        _nameEn = nameEn;
        _hp = hp;
        _reward = reward;
        _rewardCount = rewardCount;
    }
}



