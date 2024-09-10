using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMoveCtrl : MonsterMovement
{
    BossCtrl _manager;
    NavMeshAgent _agent;

    public void Init(BossCtrl manager)
    {
        _manager = manager;
        _agent = GetComponent<NavMeshAgent>();
        _defPos = transform.position;
    }

    public void BaseNavSetting()
    {
        _agent.ResetPath();
        _agent.isStopped = false;
        _agent.updatePosition = true;
        _agent.updateRotation = false;
    }

    public void AttackNavSetting()
    {
        _agent.isStopped = true;
        _agent.updatePosition = true;
        _agent.updateRotation = false;
        _agent.velocity = Vector3.zero;
    }
       
}
