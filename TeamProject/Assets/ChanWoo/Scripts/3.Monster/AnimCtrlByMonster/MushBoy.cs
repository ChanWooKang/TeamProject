using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MushBoy : MonsterAnimCtrl
{
    enum AnimationIndex
    {
        GetHit = 0,
        HeadAttack = 1,
        KickAttack = 2,
        LeafAttack = 3,
        BombAttack = 4,
        Buff = 5
    }    

    enum MeleeAttack
    {
        HeadAttack = 0,
        KickAttack,
        Buff
    }

    enum RangeAttack
    {
        LeafAttack = 0,
        BombAttack,
        Buff
    }

    //피격 애니메이션 후 돌아갈 스테이트 저장
    protected eMonsterState _beforeState = eMonsterState.IDLE;
    int _animIDGetHit;
    int _animIDDizzy;

    public override void Init(MonsterController manager, Animator animator)
    {
        base.Init(manager, animator);
        _animIDGetHit = Animator.StringToHash("GetHit");
        _animIDDizzy = Animator.StringToHash("Dizzy");
        
    }

    public override void ChangeAnimation(eMonsterState type)
    {
        switch (type)
        {
            case eMonsterState.IDLE:
                if (_manager.isStatic)
                    _animator.CrossFade("Idle_Plant",0.1f);
                else
                    _animator.CrossFade("Idle_Normal", 0.1f);                
                break;
            case eMonsterState.SENSE:
                _animator.CrossFade("Sense", 0.1f, -1, 0);
                break;
            case eMonsterState.PATROL:
            case eMonsterState.RETURN:
                _animator.CrossFade("Walk", 0.1f);                
                break;
            case eMonsterState.CHASE:
                _animator.CrossFade("Run", 0.1f);
                _beforeState = eMonsterState.CHASE;
                break;
            case eMonsterState.ATTACK:                
                AttackAction();
                break;
            case eMonsterState.GETHIT:
                _manager.AttackNavSetting();
                _animator.SetTrigger(_animIDGetHit);
                break;
            case eMonsterState.DIZZY:
                _animator.SetTrigger(_animIDDizzy);
                break;
            case eMonsterState.DIE:
                _animator.CrossFade("Die", 0.1f);
                break;                        
        }
    }


    public void AttackAction()
    {
        Debug.Log(_manager._attackType);
        string trigger = "";
        switch (_manager._attackType)
        {
            case eAttackType.MeleeAttack:
                trigger = Utilitys.ConvertEnum(
                    (MeleeAttack)PickPattern(_manager._attackType));                
                _animator.SetTrigger(trigger);
                
                break;
            case eAttackType.RangeAttack:
                trigger = Utilitys.ConvertEnum(
                    (RangeAttack)PickPattern(_manager._attackType));                
                _animator.SetTrigger(trigger);
                break;
        }        
    }

    public int PickPattern(eAttackType type)
    {
        int index = 0;
        float[] probs = _meleeWeightProbs;
        if(type == eAttackType.RangeAttack)
            probs = _rangeWeightProbs;        

        float randValue = Random.Range(0.0f, 1.0f);
        for(int i = 0; i < probs.Length; i++)
        {
            if(randValue <= probs[i])
            {
                index = i;
                break;
            }
        }                

        return index;
    }    
}
