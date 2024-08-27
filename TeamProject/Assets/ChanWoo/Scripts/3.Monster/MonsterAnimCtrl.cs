using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class MonsterAnimCtrl : MonoBehaviour
{
    MonsterController _manager;
    Animator _animator;
    
    public void Init(MonsterController manager, Animator animator)
    {
        _manager = manager;
        _animator = animator;
    }

    public void ChangeAnimation(eMonsterState type)
    {
        switch (type)
        {
            case eMonsterState.IDLE:
                break;
        }
    }
}
