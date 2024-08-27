using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo
{
    protected int _index;
    protected string _nameEn;
    protected string _nameKr;
    protected string _desc;
    protected float _hp;
    protected float _speed;
    protected float _runSpeed;
    protected float _range;
    protected float _damage;
    protected float _attackDelay;
    protected float _sight;
    protected float _workAbility;
    //수정요청 할 것 ( Type )
    protected int _type;
    protected int _rate;
    protected int[] _reward;
    protected float _chaseRange;
    protected int[] _rewardCount;    

    #region [ Property ]
    public int Index { get { return _index; } }
    public string NameEn { get { return _nameEn; } }
    public string NameKr { get { return _nameKr; } }
    public string Desc { get { return _desc; } }
    public float HP { get { return _hp; } }
    public float Speed { get { return _speed; } }
    public float RunSpeed { get { return _runSpeed; } }
    public float Range { get { return _range; } }
    public float Damage { get { return _damage; } }
    public float AttackDelay { get { return _attackDelay; } }
    public float Sight { get { return _sight; } }
    public float WorkAbility { get { return _workAbility; } }
    public int CharacterType { get { return _type; } }
    public int CaptureRate { get { return _rate; } }
    public int[] Rewards { get { return _reward; } }
    public float ChaseRange { get { return _chaseRange; } }
    public int[] RewardCounts { get { return _rewardCount; } }    
    #endregion

    public MonsterInfo
        (int index, string nameEn, string nameKr, string desc, float hp, float speed,
        float runSpeed, float range, float damage, float attackDelay, float sight,
        float workAbility, int type, int rate, int[] reward, float chaseRange, int[] rewardCounts
        )
    {
        _index = index;
        _nameEn = nameEn;
        _nameKr = nameKr;
        _desc = desc;
        _hp = hp;
        _speed = speed;
        _runSpeed = runSpeed;
        _range = range;
        _damage = damage;
        _attackDelay = attackDelay;
        _sight = sight;
        _workAbility = workAbility;
        _type = type;
        _rate = rate;
        _reward = reward;
        _chaseRange = chaseRange;
        _rewardCount = rewardCounts;
        
    }


    //처치시 획득 할 수 있는 EXP 값
}
