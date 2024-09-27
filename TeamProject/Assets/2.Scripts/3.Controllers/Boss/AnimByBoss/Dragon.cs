using DefineDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : BossAnimCtrl
{
    bool isSleep;
    bool isFlying;
    bool isActing;

    //Bools Type
    int _animIDWalk;
    int _animIDChase;
    int _animIDFlying;
    int _animIDSleep;

    //TriggerTypes
    int _animIDGrowl;
    int _animIDDie;
    int _animIDTakeOff;
    int _animIDLanding;

    public override void Init(BossCtrl manager)
    {
        base.Init(manager);
        SettingBossType();
        AddAnimIDSet();
    }

    void AddAnimIDSet()
    {
        _animIDWalk = Animator.StringToHash("Walk");
        _animIDChase = Animator.StringToHash("Chase");
        _animIDFlying = Animator.StringToHash("Flying");
        _animIDSleep = Animator.StringToHash("Sleep");
        _animIDGrowl = Animator.StringToHash("Growl");
        _animIDDie = Animator.StringToHash("Die");
        _animIDTakeOff = Animator.StringToHash("TakeOff");
        _animIDLanding = Animator.StringToHash("Landing");
    }

    public override void SettingBossType()
    {
        _bossType = eBossType.Dragon;
        isFlying = false;
        isSleep = true;
        isActing = false;
    }

    public override void ChangeAnimation(eBossState type)
    {
        switch (type)
        {
            case eBossState.IDLE:
                _animator.SetBool(_animIDWalk, false);
                _animator.SetBool(_animIDChase, false);

                if (isFlying)
                    _animator.CrossFade("FlyIdle", 0.1f);
                else
                    _animator.CrossFade("Idle", 0.1f);
                break;
            case eBossState.SLEEP:

                break;
            case eBossState.GROWL:
                _animator.SetTrigger(_animIDGrowl);
                break;
            case eBossState.PATROL:
                _animator.SetBool(_animIDWalk, true);
                _animator.SetBool(_animIDChase, false);
                break;
            case eBossState.CHASE:
                _animator.SetBool(_animIDWalk, false);
                _animator.SetBool(_animIDChase, true);
                break;
            case eBossState.GETHIT:
                
                break;
            case eBossState.ATTACK:                
                AttackAction();
                break;
            case eBossState.DIE:                
                DieAction();                                
                break;
        }
    }
    
    void DieAction()
    {
        _animator.SetBool(_animIDWalk, false);
        _animator.SetBool(_animIDChase, false);
        _animator.SetBool(_animIDFlying, false);
        isFlying = false;
        _manager._anim.FlameEnd();
        _animator.SetTrigger(_animIDDie);


    }

    public override void SleepAction()
    {
        if (isFlying)
        {
            if(isActing == false)
            {
                isActing = true;
                _animator.SetTrigger(_animIDLanding);
                _manager._collider.MoveTransformByTakeOff(false);
            }            
        }
        else
        {
            Sleep(true);
        }
    }   

    void Sleep(bool checkSleep)
    {
        isSleep = checkSleep;
        _animator.SetBool(_animIDSleep, isSleep);
        if (isSleep)
        {            
            _manager.ChangeState(BossStateSleep._inst);
        }                
    }

    public void GrowlEnd()
    {
        //플레이어 인식 후 날건지 걸을건지 선택
        int randValue = Random.Range(0, 5);
        bool checkFly = randValue > 0 ? true : false;
        Sleep(false);
        if (checkFly)
        {
            if(isActing == false)
            {
                isActing = true;
                _animator.SetTrigger(_animIDTakeOff);
                _manager._collider.MoveTransformByTakeOff(true);
            }            
        }
        else
        {
            _manager.ChangeState(BossStateChase._inst);
        }
    }

    public void ChangeFlying(int value)
    {
        bool isFly = value > 0 ? true : false;
        isFlying = isFly;
        _animator.SetBool(_animIDFlying, isFlying);
        isActing = false;
        if (isFlying)
        {
            _manager.ChangeState(BossStateChase._inst);
        }
        else
        {
            Sleep(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;        
        Gizmos.DrawRay(_firePos.position, _firePos.right * -5f);        
    }

}
