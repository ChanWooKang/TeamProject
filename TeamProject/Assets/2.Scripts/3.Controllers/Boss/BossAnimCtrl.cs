using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class BossAnimCtrl : BaseAnimCtrl
{
    protected BossCtrl _manager;
    protected Animator _animator;
    [SerializeField] protected Transform _firePos;
    public eBossType _bossType;

    public virtual void Init(BossCtrl manager)
    {
        _manager = manager;
        _animator = GetComponent<Animator>();
        InitAnimData();
    }

    public abstract void SettingBossType();
    public abstract void ChangeAnimation(eBossState type);
    public abstract void SleepAction();

    public void AttackAction()
    {        
        if (nextSkill > 0)
        {
            if (Managers._data.Dict_Skill.ContainsKey(nextSkill))
            {
                string trigger = Managers._data.Dict_Skill[nextSkill].NameEn;                
                _animator.SetTrigger(trigger);
                nextSkill = 0;
                return;
            }
        }
        int pattern = PickBaseAttackPattern();
        _manager.State = eBossState.IDLE;
        _animator.SetInteger(_animIDAttackPattern, pattern);
        _animator.SetTrigger(_animIDAttack);
    }
    
    public void AttackEvent()
    {
        if(_manager._move.CheckCloseTarget(_manager.target.position,_manager.attackRange))
        {
            if (_manager.target.CompareTag("Player"))
                _manager.player.OnDamage(_manager.Stat.Damage);                
        }
    }

    public void AttackEnd()
    {
        _manager.Agent.avoidancePriority = 50;
        _manager.GetRangeByAttackType();
        _manager.isAttack = false;
    }

    public void GetHitEnd()
    {
        if(_manager.isAttack == true)
        {
            _manager.Agent.avoidancePriority = 50;
            _manager.isAttack = false;
        }        

        if (_manager.isDead == false)
            _manager.ChangeState(BossStateIdle._inst);
    }

    //불똥 발사
    public void FireBallAction()
    {
        Debug.Log("파이어볼 !");
    }

    //화염 방사
    public void FlameAction()
    {
        Debug.Log("부우우우우울바다");
    }

    public void FlameEnd()
    {
        Debug.Log("끝");
    }

    
}
