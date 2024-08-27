using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineDatas;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MonsterMovement))]
[RequireComponent(typeof(MonsterAnimCtrl))]
public class MonsterController : FSM<MonsterController>
{
    [Header("Monster Index")]
    public int Index;    

    [Header("Component")]    
    public MonsterStat Stat;
    
    public MonsterMovement _movement;
    public MonsterAnimCtrl _animCtrl;

    //Componsnent
    NavMeshAgent _agent;
    Animator _animator;
    Renderer[] _meshs;
    CapsuleCollider _collider;
    Rigidbody _rigid;

    //���� ���� üũ �� �ִϸ��̼� ���� 
    eMonsterState _nowState;
    

    //Ÿ�� ����
    public Transform target;
    public Vector3 targetPos;

    //Bools
    public bool isDead;
    public bool isAttack;
    public bool isReturnHome;

    public NavMeshAgent Agent { get { return _agent; } }
    

    public eMonsterState State
    {
        get { return _nowState; }
        set
        {
            _nowState = value;
            _animCtrl.ChangeAnimation(_nowState);
        }
    }

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
        FreezeRotation();
    }

    public void InitComponent()
    {
        Stat.Init(Index);
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _meshs = GetComponentsInChildren<Renderer>();
        _collider = GetComponent<CapsuleCollider>();
        _rigid = GetComponent<Rigidbody>();        
        
        _movement = GetComponent<MonsterMovement>();
        _animCtrl = GetComponent<MonsterAnimCtrl>();

        _movement.Init(this, _agent);
        _animCtrl.Init(this, _animator);
    }

    //State = Init ���� ������ ����
    public void InitData()
    {                
        target = null;
        targetPos = Vector3.zero;

        isDead = false;
        isAttack = false;
        isReturnHome = false;
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

    void FreezeRotation()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }

    void ChangeColor(Color color)
    {
        if (_meshs.Length > 0)
        {
            foreach (Renderer mesh in _meshs)
                mesh.material.color = color;
        }
    }

    public void ChangeLayer(eLayer layer)
    {        
        gameObject.layer = (int)layer;
    }

    public void SetTarget()
    {
        if(GameManagerEx._inst.Player != null)
        {
            if(GameManagerEx._inst.Player.TryGetComponent(out PlayerManager player))
            {
                //���߿� ���� �÷��̾� �׾����� Ȯ���ϴ� bool�� ������ , ���� ������ �������̸� ����
                if (player.isActiveAndEnabled && Stat.CharacterType > 0)
                {
                    target = GameManagerEx._inst.Player;
                    return;
                }
            }
        }

        target = null;
    }

    public void SetTarget(PlayerStat stat)
    {
        //���������� ������ ������ 
        if(target == null)
            target = stat.transform;
    }
}
