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

    FlameCtrl _flame;

    public virtual void Init(BossCtrl manager)
    {
        _manager = manager;
        _animator = GetComponent<Animator>();
        _flame = null;
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

    //�Ҷ� �߻�
    public void FireBallAction()
    {
        GameObject go = PoolingManager._inst.InstantiateAPS("FireBall",_firePos.position,_firePos.rotation,Vector3.one);
        if(go.TryGetComponent(out FireBall fireBall))
        {
            float damageTime = GetDamageTime(fireBall.skillID);
            fireBall.ShootEvent(_firePos.right * -1, _manager.Stat.Damage * damageTime);
        }
        else
        {
            Destroy(go);
        }
        
    }

    //ȭ�� ���
    public void FlameAction()
    {
        if(_flame == null)
        {
            GameObject go = PoolingManager._inst.InstantiateAPS("Flame",transform);
            if (go.TryGetComponent(out _flame))
            {
                float damageTime = GetDamageTime(_flame.skillID);
                _flame.OnFlameEvent(_firePos,_manager.target,_manager.Stat.Damage * damageTime);                
            }
            else
            {
                Destroy(go);
                _flame = null;
            }
        }
        else
        {
            float damageTime = GetDamageTime(_flame.skillID);
            _flame.OnFlameEvent(_firePos, _manager.target,_manager.Stat.Damage * damageTime);
        }
        
    }

    public void FlameEnd()
    {
        if(_flame != null)
        {
            _flame.OffFlameEvent();
        }
    }

    
    float GetDamageTime(int skillID)
    {
        float times = 1.0f;
        if (Managers._data.Dict_Skill.ContainsKey(skillID))
        {
            times = Managers._data.Dict_Skill[skillID].DamageTimes;
        }
        return times;
    }
}
