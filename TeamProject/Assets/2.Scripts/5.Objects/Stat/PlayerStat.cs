using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
public class PlayerStat : BaseStat
{
    //플레이어 추가 스탯 예상
    //( 무게, 스태미너,경험치량, 달리기 속도 ,...)

    protected float _stamina;
    protected float _maxStamina;
    protected float _runSpeed;
    protected int _gold;
    protected float _carryWeight;
    protected float _workAbility;
    [SerializeField] protected int _bonusStat;

    #region [ Property ]
    public float EXP
    {
        get { return _exp; }
        set
        {
            _exp = value;
            int level = 1;
            while (true)
            {
                if (Managers._data.Dict_RequiredExp.
                    TryGetValue(level + 1, out RequiredEXPByLevel stat) == false)
                    break;
                if (_exp < stat.exp)
                    break;
                level++;
            }

            if(level != _level)
            {                
                LevelUp(level);
            }

        }
    }

    public float Stamina { get { return _stamina; } set { _stamina = value; } }
    public float MaxStamina { get { return _maxStamina; } set { _maxStamina = value; } }
    public float RunSpeed { get { return _runSpeed; } set { _runSpeed = value; } }
    public float CarryWeight { get { return _carryWeight; } set { _carryWeight = value; } }
    public float WorkAbility { get { return _workAbility; } set { _workAbility = value; } }
    public int Gold { get { return _gold; } set { _gold = value; } }
    public int BonusStat { get { return _bonusStat; } }
    public float ConvertEXP
    {
        get
        {
            float exp = _exp;

            if (_level > 1)
            {
                if (Managers._data.Dict_RequiredExp.TryGetValue(_level, out RequiredEXPByLevel stat))
                {
                    exp = _exp - stat.exp;
                }
            }

            return exp;
        }
    }

    public float ConvertTotalEXP
    {
        get
        {
            float exp = _exp;

            if (Managers._data.Dict_RequiredExp.TryGetValue(_level, out RequiredEXPByLevel nowStat))
            {
                if (Managers._data.Dict_RequiredExp.TryGetValue(_level + 1, out RequiredEXPByLevel nextStat))
                {
                    exp = nextStat.exp - nowStat.exp;
                }
            }

            return exp;
        }
    }

    public float ConvertRequiredEXP
    {
        get
        {                        
            return ConvertTotalEXP - ConvertEXP;
        }
    }
    #endregion [ Property ]


    public void Init()
    {
        _level = 1;
        _hp = _maxHp = 200;
        _stamina = _maxStamina = 100;
        _damage = 20;
        _defense = 5;
        _exp = 0;
        _gold = 0;
        _moveSpeed = 5;
        _runSpeed = 10;
        _bonusStat = 0;
        _carryWeight = 5000;
        _workAbility = 50;
    }

    public void LevelUp(int level)
    {
        //레벨업 작용 (이펙트 , 보너스 스탯 획득 등등, HP회복);
        Debug.Log("LevelUP");
        _level = level;
        _hp = MaxHP;
        _bonusStat += 5;
        
    }

    public void AddStat(eStatType type)
    {
        _bonusStat = --_bonusStat <= 0 ? 0 : _bonusStat;
        switch (type)
        {
            case eStatType.HP:
                MaxHP += 50;                
                break;
            case eStatType.Statmina:
                MaxStamina += 50;
                break;
            case eStatType.Attack:
                Damage += 5;
                break;
            case eStatType.Defense:
                Defense += 5;
                break;
            case eStatType.WorkAblity:
                WorkAbility += 10;
                break;
            case eStatType.CarryWeight:
                CarryWeight += 50;
                break;
        }
    }    

    public void AddStat(eStatType type, float value)
    {
        switch (type)
        {
            case eStatType.HP:
                MaxHP += value;
                HP = Mathf.Min(HP, MaxHP);
                break;
        }
    }

    public bool CheckUseStamina(float value)
    {
        return _stamina >= value;
    }

    public bool CanUseStamina(float value)
    {
        float tempStamina = _stamina - value;
        if(tempStamina >= 0)
        {
            _stamina = tempStamina;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool UseStaminaByTime(float value)
    {
        _stamina -= value * Time.deltaTime;
        _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);
        return _stamina > 0;
    }

    public void RegenStaminaByTime(float value)
    {
        _stamina += value * Time.deltaTime;
        _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);
    }
}
