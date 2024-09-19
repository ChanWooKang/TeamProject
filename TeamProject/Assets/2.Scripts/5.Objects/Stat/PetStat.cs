using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetStat : BaseStat
{
    protected int _index;
    protected float _runSpeed;
    protected float _chaseRange;
    protected float _attackrange;
    protected float _attackDelay;
    protected float _sight;

    [SerializeField] float baseHp;
    float baseDamage;

    #region [ Property ]    
    public int Index { get { return _index; } }
   
    public float RunSpeed { get { return _runSpeed; } }
    public float ChaseRange { get { return _chaseRange; } }
    public float AttackRange { get { return _attackrange; } }
    public float Sight { get { return _sight; } }
    public float AttackDelay { get { return _attackDelay; } }
    #endregion [ Property ] 




    MonsterInfo _monster;
    public void Init(int index)
    {
        if (Managers._data.Dict_Monster.ContainsKey(index))
        {
            _index = index;
            _monster = Managers._data.Dict_Monster[index];
            LoadAndSetData();
            SetBaseStat();
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
            _runSpeed = 8;
            _attackrange = 4;
            _chaseRange = 10;
            _attackDelay = 2;
            _sight = 80;            

            //레벨 변경시 추가 가중치 들어갈 변수
            _hp = 200;
            _maxHp = _hp;
            _damage = 5;
            //방어력 추가 시 추가
            //_defense = info.Defense            
        }

    }
    //최초 데이터 설정 ( 변하지 않는 값 )
    public void LoadAndSetData()
    {
        //레벨과 관계없이 저장될 변수        
        _moveSpeed = _monster.Speed;
        _chaseRange = _monster.ChaseRange;
        
        _runSpeed = _monster.RunSpeed;
        _attackrange = _monster.Range;
        _attackDelay = _monster.AttackDelay;
        _sight = _monster.Sight;
        //_characterType = _monster.CharacterType;
 

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
            SetStat(Managers._data.Dict_MonsterLevel[_level].RewardAbility, Managers._data.Dict_MonsterLevel[_level].DropExp);
        }
        else
        {
            MonsterLevelInfo info = Managers._data.Dict_MonsterLevel[_level];
            // 레벨이 1일때
            _maxHp = _hp = monster.HP;
            _exp = 0;            
        }
    }

    void SetStat(float value, float exp)
    {
        _hp = baseHp * value;
        _damage = baseDamage * value;
        _maxHp = _hp;        
    }

    void SetBaseStat()
    {
        int level = 1;

        if (Managers._data.Dict_MonsterLevel.TryGetValue(level, out MonsterLevelInfo info))
        {
            baseHp = _monster.HP;
            baseDamage = _monster.Damage;               
        }
        else
        {
            //버그 발생 고치삼
        }

    }
}
