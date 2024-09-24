using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PetMushBoy : PetAnimController
{
    enum MeleeAttack
    {
        HeadAttack = 0,
        KickAttack,
    }

    enum RangeAttack
    {
        LeafAttack = 0,
        BombAttack,
    }

    int _animIDGetHit;
    int _animIDDizzy;
    int _animIDBuff;

    public override void Init(PetController manager, Animator animator)
    {
        base.Init(manager, animator);
        _animIDGetHit = Animator.StringToHash("GetHit");
        _animIDDizzy = Animator.StringToHash("Dizzy");
        _animIDBuff = Animator.StringToHash("Buff");
    }

    public override void ChangeAnimation(eMonsterState type)
    {
        switch (type)
        {
            case eMonsterState.IDLE:
                if (_manager.isStatic)
                    _animator.CrossFade("Idle_Plant", 0.1f);
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
            case eMonsterState.WORK:
                if (!_manager.isworkReady)
                    _animator.CrossFade("Walk", 0.1f);
                else
                    WorkAction();
                break;
            case eMonsterState.DIE:
                _animator.CrossFade("Die", 0.1f);
                break;
        }
    }


    void AttackAction()
    {
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
            case eAttackType.Buff:
                _animator.SetTrigger(_animIDBuff);
                break;
        }
    }
   

}
