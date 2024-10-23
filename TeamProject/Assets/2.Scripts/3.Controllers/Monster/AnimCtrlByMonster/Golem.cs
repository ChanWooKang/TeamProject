using DefineDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonsterAnimCtrl
{        
    public override void Init(MonsterController manager, Animator animator)
    {
        base.Init(manager, animator);
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
                //_manager.AttackNavSetting();
                //_animator.SetTrigger(_animIDGetHit);
                break;                            
            case eMonsterState.DIE:
                _animator.CrossFade("Die", 0.1f);
                break;
        }
    }
}
