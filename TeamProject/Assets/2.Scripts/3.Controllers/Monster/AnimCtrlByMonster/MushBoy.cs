using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MushBoy : MonsterAnimCtrl
{
    
    int _animIDDizzy;
    int _animIDBuff;

    public override void Init(MonsterController manager, Animator animator)
    {
        base.Init(manager, animator);
        
        _animIDDizzy = Animator.StringToHash("Dizzy");
        _animIDBuff = Animator.StringToHash("Buff");
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
}
