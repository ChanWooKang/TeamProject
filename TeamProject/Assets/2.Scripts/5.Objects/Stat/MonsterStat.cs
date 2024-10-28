using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStat : BaseStat
{
    protected int _index;
    protected float _chaseRange;
    protected float _runSpeed;
    protected float _attackrange;
    protected float _attackDelay;
    protected float _dropExp;
    protected int _captureRate;
    protected float _sight;
    protected int _characterType;
    protected int _uniqueID;

    MonsterInfo _monster;
    [SerializeField] float baseHp;
    float baseDamage;
    float baseDropExp;

    #region [ Property ]    
    public int Index { get { return _index; }}
    public float ChaseRange { get { return _chaseRange; } }
    public float RunSpeed { get { return _runSpeed; } }
    public float AttackRange { get { return _attackrange; } }
    public float Sight { get { return _sight; } }
    public float AttackDelay { get { return _attackDelay; } }    
    public float DropExp { get { return _dropExp; } }
    public int CaptureRate { get { return _captureRate; } }
    public int CharacterType { get { return _characterType; } }
    public MonsterInfo MonsterInfo { get { return _monster; } }

    public int UniqueID { get { return _uniqueID; } }

    public float EXP 
    {
        get { return _exp; }
        set
        {
            _exp = value;
            int level = 1;
            while (true)
            {
                if (Managers._data.Dict_MonsterLevel.TryGetValue(level + 1, out MonsterLevelInfo info) == false)
                    break;
                if (_exp < info.RequiredExp)
                    break;
                level++;
            }

            if(level != _level)
            {
                _level = level;
                SetStat(Managers._data.Dict_MonsterLevel[_level].RewardAbility, Managers._data.Dict_MonsterLevel[_level].DropExp);
            }
        }
    }
    #endregion [ Property ]

    public void Init(int index)
    {
        if (Managers._data.Dict_Monster.ContainsKey(index))
        {
            _index = index;
            _monster = Managers._data.Dict_Monster[index];
            LoadAndSetData();
            SetBaseStat();
            SettingUniqueID();
        }
        else
        {
            _index = 1000;
            _level = 1;
            _hp = 200;
            _maxHp = 200;
            _damage = 2;
            _defense = 5;
            _moveSpeed = 6;
            _chaseRange = 10;
            _runSpeed = 8;
            _attackrange = 5;
            _attackDelay = 2;
            _exp = 60;

            //������ ������� ����� ����        
            _moveSpeed = 6;
            _chaseRange = 10;
            _runSpeed = 8;
            _attackrange = 4;
            _captureRate = 255;
            _attackDelay = 2;
            _sight = 80;
            _characterType = 0;

            //���� ����� �߰� ����ġ �� ����
            _hp = 200;
            _maxHp = _hp;
            _damage = 5;
            //���� �߰� �� �߰�
            //_defense = info.Defense
            _dropExp = 60;
        }        
    }   

    void SettingUniqueID()
    {
        int id;
        do
        {
            id = Random.Range(0, 10000);
        }
        while (Managers._data.AddUniqueID(id, Index));

        _uniqueID = id;
    }

    public void DeadFunc(PlayerStat stat)
    {
        if (stat != null)
            stat.EXP += _exp;
    }    

    //���� ������ ���� ( ������ �ʴ� �� )
    public void LoadAndSetData()
    {        
        //������ ������� ����� ����        
        _moveSpeed = _monster.Speed;
        _chaseRange = _monster.ChaseRange;
        _runSpeed = _monster.RunSpeed;
        _attackrange = _monster.Range;
        _captureRate = _monster.CaptureRate;
        _attackDelay = _monster.AttackDelay;
        _sight = _monster.Sight;
        //_characterType = _monster.CharacterType;
        _characterType = 1;

        //SetConvertibleStat(info.Index,level);
    }    

    //�ܺο��� ���� ���� �� ó��
    public void SetByLevel()
    {
        MonsterInfo monster = Managers._data.Dict_Monster[_index];        
        if (_level > 1)
        {
            MonsterLevelInfo beforeLevelInfo = Managers._data.Dict_MonsterLevel[_level - 1];
            _exp = beforeLevelInfo.DropExp;                        
            SetStat(Managers._data.Dict_MonsterLevel[_level].RewardAbility, Managers._data.Dict_MonsterLevel[_level].DropExp);
        }
        else
        {
            MonsterLevelInfo info = Managers._data.Dict_MonsterLevel[_level];
            // ������ 1�϶�
            _maxHp = _hp = monster.HP;             
            _exp = 0;            
            SetStat(1, info.DropExp);
        }
    }

    void SetStat(float value , float exp)
    {
        _hp = baseHp * value;        
        _damage = baseDamage * value;
        _maxHp = _hp;
        _dropExp = exp;
    }

    void SetBaseStat()
    {
        int level = 1;
        
        if(Managers._data.Dict_MonsterLevel.TryGetValue(level, out MonsterLevelInfo info))
        {
            baseHp = _monster.HP;
            baseDamage = _monster.Damage;
            baseDropExp = info.DropExp;
        }
        else
        {
            //���� �߻� ��ġ��
        }
            
    }
}
