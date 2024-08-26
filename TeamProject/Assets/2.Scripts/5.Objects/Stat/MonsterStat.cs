using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : BaseStat
{
    protected float _traceRange;
    protected float _traceSpeed;
    protected float _attackRange;
    protected float _attackDelay;
    protected int _minGold;
    protected int _maxGold;
    
    #region [ Property ]
    public float TraceRange { get { return _traceRange; } }
    public float TraceSpeed { get { return _traceSpeed; } }
    public float AttackRange { get { return _attackRange; } }
    public float AttackDelay { get { return _attackDelay; } }
    public int Gold { get { return Random.Range(_minGold, _maxGold); } }
    public float Exp { get { return _exp; } }

    #endregion [ Property ]

    public void Init()
    {
        _level = 1;
        _hp = 200;
        _maxHp = 200;
        _damage = 2;
        _defense = 5;
        _moveSpeed = 6;
        _traceRange = 10;
        _traceSpeed = 8;
        _attackRange = 5;
        _attackDelay = 2;
        _minGold = 50;
        _maxGold = 100;
        _exp = 60;
    }   

    public void DeadFunc(PlayerStat stat)
    {
        if (stat != null)
            stat.EXP += _exp;
    }    
}
