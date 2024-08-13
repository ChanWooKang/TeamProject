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

    
    //���� �߰� ���� ����
    //( �߰� ��Ÿ�, �߰� �ӵ�, ���� ��Ÿ�, ���� ������, �ּ� �ִ� ��ȭ , ����ġ, ...)

    #region [ Property ]
    public int Level { get { return _level; } set { _level = value; } }
    public float HP { get { return _hp; } set { _hp = value; } }
    public float MaxHP { get { return _maxHp; } set { _maxHp = value; } }
    public float Damage { get { return _damage; } set { _damage = value; } }
    public float Defense { get { return _defense; } set { _defense = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    //�ǰ� ������ ǥ�� �Ҷ� ���
    public float AttackDamage { get { return _attackedDamage; } }
    #endregion [ Property ]
  
    //ȭ�� ����, �� ���� �� �÷��̾� , ���� ������ �ʿ� ���� ������ ���
    public virtual bool GetHit(float Damage)
    {
        float per = 0.06f;
        float minDamage = 1;
        float damage =
            Mathf.Max(minDamage, Damage * ((_defense * per) / (1 + per * _defense)));
        return CalculateDamage(damage);
    }

    // �ǰݵ����� ������ ü���� 0 ���ϰ� �Ǵ��� Ȯ��
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
