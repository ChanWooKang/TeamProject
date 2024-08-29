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
    [Header("Monster Data")]
    public int Index;
    public int MonsterLevel = 1;

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

    //OtherComponent
    public PlayerManager _player;

    //몬스터 상태 체크 및 애니메이션 적용 
    eMonsterState _nowState;
    public eAttackType _attackType;

    //타겟 관련
    public Transform target;
    public Vector3 targetPos;

    //floats
    public float delayTime = 2.0f;
    public float attackRange;
    public float dizzyRate = 0.05f;

    //Bools
    //몬스터가 최초로 생성됬을때 갖고있는다 초식만 갖고있는다
    public bool isStatic = true;
    public bool isDead;
    public bool isAttack;
    public bool isReturnHome;

    Coroutine DamageCoroutine = null;

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

    private void Awake()
    {
        InitComponent();
    }

    private void Start()
    {
        InitComponent();
        InitState(this, MonsterStateInit._inst);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnDamage(5);            
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ChangeState(MonsterStateDizzy._inst);
        }
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
        _attackType = eAttackType.None;
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
        if (target == null)
        {
            if (GameManagerEx._inst.playeManager != null)
            {
                if (GameManagerEx._inst.playeManager.isDead == false)
                {
                    target = GameManagerEx._inst.playeManager.transform;
                    _player = GameManagerEx._inst.playeManager;
                }
                else
                {
                    target = null;
                }
            }
            else
            {
                target = null;
            }
        }
    }

    public void SettingMonsterStatByLevel()
    {
        Stat.Level = MonsterLevel;
        Stat.SetByLevel();
    }

    public void AttackFunc()
    {
        //플레이어 상태 확인 
        if (_player.isDead)
            return;

        _agent.avoidancePriority = 51;
        State = eMonsterState.ATTACK;
        isAttack = true;
    }

    public void GetRangeByAttackType()
    {
        _attackType = _animCtrl.GetAttackTypeByWeight();
        switch (_attackType)
        {
            case eAttackType.MeleeAttack:
                attackRange = Stat.AttackRange;
                break;
            case eAttackType.RangeAttack:
                attackRange = Stat.AttackRange * 2;
                break;
        }

            
    }


    public void OnDamage()
    {
        if (DamageCoroutine != null)
            StopCoroutine(DamageCoroutine);
        DamageCoroutine = StartCoroutine(OnDamageEvent());
    }
    

    public void OnDamage(float damage)
    {
        if (isDead)
            return;

        isStatic = false;
        State = eMonsterState.GETHIT;        
        isDead = Stat.CalculateDamage(damage);        
        OnDamage();
    }

    IEnumerator OnDamageEvent()
    {           
        float randValue = Random.Range(0.0f, 1.0f);
        bool isDizzy = randValue <= dizzyRate;        
        if (isDead)
        {
            yield return new WaitForSeconds(0.20f);
            //죽을 때 작업
            _collider.enabled = false;
            Stat.DeadFunc(_player.Stat);
            ChangeColor(Color.gray);
            ChangeState(MonsterStateDie._inst);
            yield break;
        }
        if (isDizzy)
            ChangeState(MonsterStateDizzy._inst);

        ChangeColor(Color.red);
        yield return new WaitForSeconds(0.3f);
        ChangeColor(Color.white);
    }

    public void OnDeadEvent()
    {
        SpawnManager._inst.MonsterDespawn(gameObject);
        ChangeColor(Color.white);
        ChangeState(MonsterStateDisable._inst);
    }

    public void OnResurrectEvent()
    {
        if (_collider.enabled == false)
            _collider.enabled = true;

        ChangeLayer(eLayer.Monster);
        ChangeState(MonsterStateInit._inst);
    }
}
