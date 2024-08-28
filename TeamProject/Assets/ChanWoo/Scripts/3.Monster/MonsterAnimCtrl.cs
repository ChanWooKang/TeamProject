using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class MonsterAnimCtrl : MonoBehaviour
{
    protected MonsterController _manager;
    protected Animator _animator;

    //공격 방식간 가중치
    public float[] AttackTypeWeight;
    //근접 공격 개수 및 버프
    public float[] _meleeWeightProbs;
    //원거리 공격 개수 및 버프
    public float[] _rangeWeightProbs;
    public virtual void Init(MonsterController manager, Animator animator)
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
            if(randValue <= AttackTypeWeight[i])
            {
                number = i + 1;
                attackType = (eAttackType)number;
                break;
            }
        }
        
        return attackType;
    }

    public void AttackEnd()
    {
        _manager.Agent.avoidancePriority = 50;
        _manager.GetRangeByAttackType();
        _manager.isAttack = false;
        Debug.Log("AniEnd");
        
    }
}
