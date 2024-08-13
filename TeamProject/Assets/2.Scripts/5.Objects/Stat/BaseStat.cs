using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat : MonoBehaviour
{
    protected int _level;
    protected float _exp;
    protected float _hp;
    protected float _maxHp;
    protected float _damage;
    protected float _defense;
    protected float _moveSpeed;
    protected float _attackedDamage;

    
    //몬스터 추가 스텟 예상
    //( 추격 사거리, 추격 속도, 공격 사거리, 공격 딜레이, 최소 최대 금화 , 경험치, ...)

    #region [ Property ]
    public int Level { get { return _level; } set { _level = value; } }
    public float HP { get { return _hp; } set { _hp = value; } }
    public float MaxHP { get { return _maxHp; } set { _maxHp = value; } }
    public float Damage { get { return _damage; } set { _damage = value; } }
    public float Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    //피격 데미지 표시 할때 사용
    public float AttackDamage { get { return _attackedDamage; } }
    #endregion [ Property ]
  
    //화살 공격, 총 공격 등 플레이어 , 몬스터 스탯이 필요 없는 공격일 경우
    public virtual bool GetHit(float Damage)
    {
        float per = 0.06f;
        float minDamage = 1;
        float damage =
            Mathf.Max(minDamage, Damage * ((_defense * per) / (1 + per * _defense)));
        return CalculateDamage(damage);
    }

    // 피격데미지 값으로 체력이 0 이하가 되는지 확인
    public virtual bool CalculateDamage(float damage)
    {
        _attackedDamage = damage;
        if(_hp > damage)
        {
            HP -= damage;
            return false;
        }
        else
        {
            HP = 0;
            return true;
        }
    }
}
