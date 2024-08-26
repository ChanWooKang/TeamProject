using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MonsterMovement))]
public class MonsterController : FSM<MonsterController>
{

    /** ����
     * INITIAL
     * IDLE         ///(�߰��� �߰���)?
     * PATROL
     * SENSE, PEEK
     * CHASE
     * ATTACK
     * RETURN
     * DIE
     * KNOCKBACK
     * DISABLE     
     */

    [Header("Component")]
    public NavMeshAgent _agent;
    public MonsterMovement _movement;


    

    private void Start()
    {
        InitComponent();
        InitState(this, MonsterStateInit._inst);
    }

    private void Update()
    {
        FSMUpdate();
    }

    private void FixedUpdate()
    {
        
    }

    public void InitComponent()
    {
        _agent = GetComponent<NavMeshAgent>();
        _movement = GetComponent<MonsterMovement>();
        
    }
}
