using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
public class PlayerStat : BaseStat
{
    //�÷��̾� �߰� ���� ����
    //( ����, ���¹̳�,����ġ��, �޸��� �ӵ� ,...)

    protected float _stamina;
    protected float _maxStamina;
    protected float _runSpeed;
    protected int _gold;
    protected float _carryWeight;
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
    public int Gold { get { return _gold; } set { _gold = value; } }
    public int BonusStat { get { return _bonusStat; } }
    #endregion [ Property ]


    public void Init()
    {
        _level = 1;
        _hp = _maxHp = 100;
        _stamina = _maxStamina = 100;
        _damage = 20;
        _defense = 5;
        _exp = 0;
        _gold = 0;
        _moveSpeed = 5;
        _runSpeed = 10;
        _bonusStat = 0;
        _carryWeight = 999;
    }

    public void LevelUp(int level)
    {
        //������ �ۿ� (����Ʈ , ���ʽ� ���� ȹ�� ���, HPȸ��);
        Debug.Log("LevelUP");
        _level = level;
        _hp = MaxHP;
        _bonusStat += 5;
        
    }

}
