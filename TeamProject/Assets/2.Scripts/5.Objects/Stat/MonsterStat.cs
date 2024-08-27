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
    #endregion [ Property ]

    public void Init(int index)
    {
        if (Managers._data.Dict_Monster.ContainsKey(index))
        {
            _index = index;
            LoadAndSetData(Managers._data.Dict_Monster[index]);
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

            //레벨과 관계없이 저장될 변수        
            _moveSpeed = 6;
            _chaseRange = 10;
            _runSpeed = 8;
            _attackrange = 4;
            _captureRate = 255;
            _attackDelay = 2;
            _sight = 80;
            _characterType = 0;

            //레벨 변경시 추가 가중치 들어갈 변수
            _hp = 200;
            _maxHp = _hp;
            _damage = 5;
            //방어력 추가 시 추가
            //_defense = info.Defense
            _dropExp = 60;
        }        
    }   

    public void DeadFunc(PlayerStat stat)
    {
        if (stat != null)
            stat.EXP += _exp;
    }    

    //최초 데이터 설정 ( 변하지 않는 값 )
    public void LoadAndSetData(MonsterInfo info)
    {
        //레벨과 관계없이 저장될 변수        
        _moveSpeed = info.Speed;
        _chaseRange = info.ChaseRange;
        _runSpeed = info.RunSpeed;
        _attackrange = info.Range;
        _captureRate = info.CaptureRate;
        _attackDelay = info.AttackDelay;
        _sight = info.Sight;
        _characterType = info.CharacterType;

        //SetConvertibleStat(info.Index,level);
    }

    //생성 후 재 소환 시 설정


    //외부에서 레벨 설정 시 처리
    public void SetByLevel()
    {
        MonsterInfo monster = Managers._data.Dict_Monster[_index];        
        if (_level > 1)
        {
            MonsterLevelInfo beforeLevelInfo = Managers._data.Dict_MonsterLevel[_level - 1];
            _exp = beforeLevelInfo.DropExp;
            SetStat(monster.HP, monster.Damage, Managers._data.Dict_MonsterLevel[1].DropExp);
            int lev = 1;
            while (true)
            {
                if (Managers._data.Dict_MonsterLevel.TryGetValue(lev + 1, out MonsterLevelInfo nextLevelInfo) == false)
                    break;

                if (_exp < nextLevelInfo.RequiredExp)
                    break;


                SetStat(Managers._data.Dict_MonsterLevel[lev].RewardAbility, Managers._data.Dict_MonsterLevel[lev].DropExp);
                lev++;
            }
        }
        else
        {
            MonsterLevelInfo info = Managers._data.Dict_MonsterLevel[_level];
            // 레벨이 1일때
            _hp = monster.HP;
            _maxHp = _hp;
            _exp = info.RequiredExp;
            _dropExp = info.DropExp;
        }
    }

    void SetStat(float hp, float damage,float exp)
    {
        _maxHp = _hp = hp;
        _damage = damage;
        _dropExp = exp;
    }

    void SetStat(float value , float exp)
    {
        _hp *= value;
        _damage *= value;
        _maxHp = _hp;
        _dropExp = exp;
    }
}
