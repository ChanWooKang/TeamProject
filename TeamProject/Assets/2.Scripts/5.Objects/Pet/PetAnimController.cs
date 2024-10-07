using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class PetAnimController : BaseAnimCtrl
{
    protected PetController _manager;
    protected Animator _animator;


    public virtual void Init(PetController manager, Animator animator)
    {
        _manager = manager;
        _animator = animator;
        InitAnimData();
    }

    public abstract void ChangeAnimation(eMonsterState type);
    

    

    #region [ Animation CallEvent ]
    //����
    public void WorkAction()
    {
        string trigger = "Attack";
        _animator.SetTrigger(trigger);
        _animator.SetInteger("Pattern", 1);
    }
    //���� �⺻ ����
    public void AttackEvent()
    {
        if (_manager._targetMon != null)
        {
            if (_manager.Movement.CheckCloseTarget(_manager.target.position, _manager.attackRange))
            {
                if (_manager.target.CompareTag("Monster"))
                {
                    _manager.target.GetComponent<MonsterController>().OnDamage(_manager.Stat.Damage, _manager.transform);
                }
            }
        }
    }

    //���� �ִϸ��̼� ���� �� ȣ��
    public void AttackEnd()
    {
        _manager.Agent.avoidancePriority = 50;
        _manager.GetRangeByAttackType();
        _manager.isAttack = false;
    }
    public void AttackAction()
    {
        // nextSkill�� 0�̻��� ��� ��ų ����
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
        _animator.SetInteger(_animIDAttackPattern, pattern);
        _animator.SetTrigger(_animIDAttack);
    }
    //�ǰ� �ִϸ��̼� ���� �� ȣ��
    public void GetHitEnd()
    {
        if (_manager.isAttack == true)
        {
            _manager.Agent.avoidancePriority = 50;
            _manager.isAttack = false;
        }
        _manager.BaseNavSetting();


        //if (_manager.isDead == false)
        //    _manager.ChangeState(MonsterStateIdle._inst);
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
        //if (go.TryGetComponent(out MushBomb bomb))
        //{
        //    bomb.BombEvent(_manager, transform.position, _manager.target.position, _manager.Stat.Damage * 2);
        //}
        //else
        //    Destroy(go);
    }

    public void BuffAction()
    {
        GameObject go = PoolingManager._inst.InstantiateAPS("AuraBuff", transform.position, Quaternion.identity, Vector3.one, transform);
        if (go.TryGetComponent(out AuraBuff aura))
            aura.BuffEvent();
        else
            Destroy(go);
    }
    #endregion [ Animation CallEvent ]
}
