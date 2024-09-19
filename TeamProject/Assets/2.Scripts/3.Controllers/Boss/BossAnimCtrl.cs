using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossAnimCtrl : BaseAnimCtrl
{
    protected BossCtrl _manager;
    protected Animator _animator;

    public virtual void Init(BossCtrl manager)
    {
        _manager = manager;
        _animator = GetComponent<Animator>();
        InitAnimData();
    }

    public virtual void ChangeAnimation(eMonsterState type)
    {
        switch (type)
        {
            
        }
    }

    public void AttackAction()
    {        
        if (nextSkill > 0)
        {
            if (Managers._data.Dict_Skill.ContainsKey(nextSkill))
            {
                string trigger = Managers._data.Dict_Skill[nextSkill].NameEn;
                Debug.Log(trigger);
                _animator.SetTrigger(trigger);
                nextSkill = 0;
                return;
            }
        }
        int pattern = PickBaseAttackPattern();
        _animator.SetInteger(_animIDAttackPattern, pattern);
        _animator.SetTrigger(_animIDAttack);
    }

}
