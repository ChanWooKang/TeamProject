using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class PetAnimController : MonoBehaviour
{
    protected PetController _manager;
    protected Animator _animator;


    //공격 방식간 가중치
    public float[] AttackTypeWeight;
    //근접 공격 개수 및 버프
    public float[] _meleeWeightProbs;
    //원거리 공격 개수 및 버프
    public float[] _rangeWeightProbs;

    public virtual void Init(PetController manager, Animator animator)
    {
        _manager = manager;
        _animator = animator;
    }

    public abstract void ChangeAnimation(eMonsterState type);
    public eAttackType GetAttackTypeByWeight()
    {
        eAttackType attackType = eAttackType.None;
        float randValue = Random.Range(0.0f, 1.0f);
        int number;
        for (int i = 0; i < AttackTypeWeight.Length; i++)
        {
            if (randValue <= AttackTypeWeight[i])
            {
                number = i + 1;
                attackType = (eAttackType)number;
                break;
            }
        }

        return attackType;
    }

    public int PickPattern(eAttackType type)
    {
        int index = 0;
        float[] probs = _meleeWeightProbs;
        if (type == eAttackType.RangeAttack)
            probs = _rangeWeightProbs;

        float randValue = Random.Range(0.0f, 1.0f);
        for (int i = 0; i < probs.Length; i++)
        {
            if (randValue <= probs[i])
            {
                index = i;
                break;
            }
        }

        return index;
    }

    #region [ Animation CallEvent ]

    //근접 기본 공격
    public void AttackEvent()
    {
        if (_manager.Movement.CheckCloseTarget(_manager.target.position, _manager.attackRange))
        {            
            if (_manager.target.CompareTag("Monster"))
            {
                _manager.target.GetComponent<MonsterController>().OnDamage(_manager.Stat.Damage, _manager.target);
            }

        }

    }

    //공격 애니메이션 종료 시 호출
    public void AttackEnd()
    {
        _manager.Agent.avoidancePriority = 50;
        _manager.GetRangeByAttackType();
        _manager.isAttack = false;
    }

    //피격 애니메이션 종료 시 호출
    public void GetHitEnd()
    {
        if (_manager.isAttack == true)
        {
            _manager.Agent.avoidancePriority = 50;
            _manager.isAttack = false;
        }
        _manager.BaseNavSetting();


        //if (_manager.isDead == false)
        //    _manager.ChangeState(MonsterStateIdle._inst);
    }

    public void LeafSlashAction()
    {

        GameObject go = PoolingManager._inst.InstantiateAPS("LeafSlash");
        if (go.TryGetComponent(out LeafSlash leaf))
        {
            leaf.SlashEvent(transform, _manager.Stat.Damage, transform.forward);
        }
        else
        {
            Destroy(go);
        }
    }

    public void MushBombAction()
    {
        GameObject go = PoolingManager._inst.InstantiateAPS("MushBomb", transform.position, Quaternion.identity, Vector3.one);
        //if (go.TryGetComponent(out MushBomb bomb))
        //{
        //    bomb.BombEvent(_manager, transform.position, _manager.target.position, _manager.Stat.Damage * 2);
        //}
        //else
        //    Destroy(go);
    }

    public void BuffAction()
    {
        GameObject go = PoolingManager._inst.InstantiateAPS("AuraBuff", transform.position, Quaternion.identity, Vector3.one, transform);
        if (go.TryGetComponent(out AuraBuff aura))
            aura.BuffEvent();
        else
            Destroy(go);
    }
    #endregion [ Animation CallEvent ]
}
