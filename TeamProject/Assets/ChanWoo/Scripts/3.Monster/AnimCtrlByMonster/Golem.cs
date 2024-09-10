using DefineDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonsterAnimCtrl
{
    enum AnimationIndex
    {
        GetHit = 0,
        BaseAttack,
        ThrowStone
    }

    enum MeleeAttack
    {
        BaseAttack
    }

    enum RangeAttack
    {
        ThrowStone
    }

    
    
    public override void Init(MonsterController manager, Animator animator)
    {
        base.Init(manager, animator);
    }

    int GetAnimationParameter(AnimationIndex type)
    {
        return Animator.StringToHash(Utilitys.ConvertEnum(type));
    }

    public override void ChangeAnimation(eMonsterState type)
    {
        switch (type)
        {
            case eMonsterState.IDLE:
            case eMonsterState.SENSE:
            case eMonsterState.DIZZY:
                _animator.CrossFade("Idle",0.1f);
                break;

            case eMonsterState.PATROL:
            case eMonsterState.RETURN:
                _animator.CrossFade("Walk", 0.1f);
                break;
            case eMonsterState.CHASE:
                _animator.CrossFade("Run", 0.1f);
                break;
            case eMonsterState.ATTACK:
                AttackAction();
                break;
            case eMonsterState.GETHIT:
                _manager.AttackNavSetting();
                _animator.SetTrigger(GetAnimationParameter(AnimationIndex.GetHit));
                break;                            
            case eMonsterState.DIE:
                _animator.CrossFade("Die", 0.1f);
                break;
        }
    }

    public void AttackAction()
    {
        switch(_manager._attackType)
        {
            case eAttackType.MeleeAttack:
                _animator.SetTrigger(Utilitys.ConvertEnum(MeleeAttack.BaseAttack));
                break;
            case eAttackType.RangeAttack:
                _animator.SetTrigger(Utilitys.ConvertEnum(RangeAttack.ThrowStone));
                break;
            case eAttackType.Buff:
                break;
        }
    }
}
