using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class MonsterAnimCtrl : BaseAnimCtrl
{
    protected MonsterController _manager;
    protected Animator _animator;
    [SerializeField] protected Transform _firePos;
    public virtual void Init(MonsterController manager, Animator animator)
    {
        _manager = manager;
        _animator = animator;
        InitAnimData();
    }

    public abstract void ChangeAnimation(eMonsterState type);
    
    public void AttackAction()
    {                
        // nextSkill이 0이상일 경우 스킬 실행
        if(nextSkill > 0)
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
        _animator.SetInteger(_animIDAttackPattern, pattern);
        _animator.SetTrigger(_animIDAttack);
    }

    #region [ Animation CallEvent ]
    
    //근접 기본 공격
    public void AttackEvent()
    {
        if (_manager._movement.CheckCloseTarget(_manager.target.position, _manager.attackRange))
        {
            if (_manager.target.CompareTag("Player"))
            {
                _manager._player.OnDamage(_manager.Stat.Damage);
            }
            //else if (_manager.target.CompareTag("Monster"))
            //{
            //    _manager.target.GetComponent<MonsterController>().OnDamage(_manager.Stat.Damage,_manager.target);
            //}
            
        }
        
    }

    //공격 애니메이션 종료 시 호출
    public void AttackEnd()
    {        
        _manager.Agent.avoidancePriority = 50;
        _manager.GetRangeByAttackType();        
        _manager.isAttack = false;
        
    }

    //피격 애니메이션 종료 시 호출
    public void GetHitEnd()
    {
        if (_manager.isAttack == true)
        {
            _manager.Agent.avoidancePriority = 50;
            _manager.isAttack = false;
        }
        _manager.BaseNavSetting();
        

        if (_manager.isDead == false)            
            _manager.ChangeState(MonsterStateIdle._inst);
    }
    
    public void LeafSlashAction()
    {

        GameObject go = PoolingManager._inst.InstantiateAPS("LeafSlash");
        if (go.TryGetComponent(out LeafSlash leaf))
        {
            leaf.SlashEvent(transform, _manager.Stat.Damage, transform.forward);
        }
        else
        {
            Destroy(go);
        }
    }

    public void MushBombAction()
    {
        GameObject go = PoolingManager._inst.InstantiateAPS("MushBomb", transform.position, Quaternion.identity, Vector3.one);
        if (go.TryGetComponent(out MushBomb bomb))
        {
            bomb.BombEvent(_manager,transform.position, _manager.target.position, _manager.Stat.Damage * 2);            
        }
        else
            Destroy(go);
    }

    public void BuffAction()
    {                
        GameObject go = PoolingManager._inst.InstantiateAPS("AuraBuff", transform.position, Quaternion.identity, Vector3.one, transform);
        if (go.TryGetComponent(out AuraBuff aura))
            aura.BuffEvent();
        else
            Destroy(go);
    }

    public void ThrowRockAction()
    {
        GameObject go = PoolingManager._inst.InstantiateAPS("ThrowStone",_firePos.position, Quaternion.identity,Vector3.one * 0.01f);
        if(go.TryGetComponent(out ThrowStone stone))
        {            
            stone.ThrowEvent(_manager, _firePos.position, _manager.target.position, _manager.Stat.Damage * 3.0f);            
        }
        else 
        { 
            Destroy(go);
        }
    }
    #endregion [ Animation CallEvent ]
}
