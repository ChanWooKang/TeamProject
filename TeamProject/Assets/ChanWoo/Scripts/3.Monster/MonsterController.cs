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

    //몬스터 상태 체크 및 애니메이션 적용 
    eMonsterState _nowState;
    

    //타겟 관련
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

    //State = Init 최초 데이터 설정
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
                //나중에 수정 플레이어 죽었는지 확인하는 bool값 생성시 , 몬스터 성격이 공격적이면 설정
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
        //직적적으로 공격을 했을때 
        if(target == null)
            target = stat.transform;
    }
}
