using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MushBoy : MonsterAnimCtrl
{
    //피격 애니메이션 후 돌아갈 스테이트 저장
    protected eMonsterState _beforeState = eMonsterState.IDLE;
    int _animIDGetHit;
    int _animIDAttack;

    public override void Init(MonsterController manager, Animator animator)
    {
        base.Init(manager, animator);
        _animIDGetHit = Animator.StringToHash("GetHit");
        _animIDAttack = Animator.StringToHash("BaseAttack");
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
                _animator.SetTrigger(_animIDAttack);
                break;
            case eMonsterState.GETHIT:
                _animator.SetTrigger(_animIDGetHit);
                break;
            case eMonsterState.DIE:
                _animator.CrossFade("Die", 0.1f);
                break;                        
        }
    }

    
}
