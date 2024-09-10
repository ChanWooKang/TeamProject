using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineDatas;

public class PetController : FSM<PetController>
{
    [Header("Monster Data")]

    public int PetLevel = 1;


    #region [Component]
    [SerializeField] PetStat m_stat;
    [SerializeField] PetMovement _movement;
    [SerializeField] PetAnimController m_animCtrl;
    NavMeshAgent m_agent;
    Animator m_animator;
    CapsuleCollider m_collider;
    Rigidbody m_rigid;
    MonsterInfo m_petInfo;
    HudController _hudCtrl;
    #endregion [Component]

    //몬스터 상태 체크 및 애니메이션 적용     
    [HideInInspector] public PlayerManager _player;
    [HideInInspector] public eAttackType _attackType;
    [SerializeField] eMonsterState _nowState;
    public Transform _hudTransform;
    //타겟 관련
    [HideInInspector] public MonsterController _targetMon;
    [HideInInspector] public Transform target;
    [HideInInspector] public Transform player;

    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public Vector3? m_workPos = null;
    [HideInInspector] public bool isworkReady;
    [Header("Datas")]

    //floats
    public float delayTime = 2.0f;
    public float attackRange;
    public float dizzyRate = 0.05f;


    public bool isStatic;
    public bool isDead;
    public bool isAttack;
    public bool isReturnHome;
    public bool isBuffed;


    public MonsterInfo PetInfo { get { return m_petInfo; } }
    public PetMovement Movement { get { return _movement; } }
    public PetStat Stat { get { return m_stat; } }
    public NavMeshAgent Agent { get { return m_agent; } }


    private void Awake()
    {
        //임시
        InitPet(1000);
        InitState(this, PetStateInit._inst);
        player = GameManagerEx._inst.playeManager.transform;

        //
    }
    private void Start()
    {
        //임시
        PoolingManager._inst.AddPetPool(this);
    }
    private void Update()
    {
        FSMUpdate();
    }
    private void LateUpdate()
    {
        FreezeRotation();
    }
    public eMonsterState State
    {
        get { return _nowState; }
        set
        {
            _nowState = value;
            m_animCtrl.ChangeAnimation(_nowState);
        }
    }
    public void MoveToObject(Vector3 targetPos)
    {
        m_workPos = targetPos;
        ChangeState(PetStateWork._inst);
        // StartCoroutine(MovePetToTarget());
    }
    public void JobDone()
    {
        ChangeState(PetStateIdle._inst);
    }

    public void InitPet(int index)
    {
        m_petInfo = Managers._data.Dict_Monster[index];
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<CapsuleCollider>();
        m_rigid = GetComponent<Rigidbody>();
        m_animCtrl.Init(this, m_animator);
        _movement.Init(this, m_agent);
        Stat.Init(index);
    }
    public void SettingPetStatByLevel()
    {
        Stat.Level = PetLevel;
        Stat.SetByLevel();

    }
    public void GetRangeByAttackType()
    {
        _attackType = m_animCtrl.GetAttackTypeByWeight();
        switch (_attackType)
        {
            case eAttackType.MeleeAttack:
                attackRange = Stat.AttackRange * 0.5f;
                break;
            case eAttackType.RangeAttack:
                attackRange = Stat.AttackRange * 2;
                break;
            default:
                attackRange = Stat.AttackRange;
                break;
        }
    }
    public void SetTarget(Transform attacker)
    {
        target = attacker;
        if (target != null)
            _targetMon = target.GetComponent<MonsterController>();
    }
    public void SetHud(HudController hud, Transform hudRoot)
    {
        _hudCtrl = hud;
        _hudCtrl.gameObject.transform.SetParent(hudRoot);
        _hudCtrl.InitHud(m_petInfo.NameKr, Stat.Level, _hudTransform, Color.green);
    }
    public void ShowHud()
    {
        if (_hudCtrl != null)
            _hudCtrl.DisPlay(Stat.HP / Stat.MaxHP);
    }
    public void AttackFunc()
    {
        //플레이어 상태 확인 
        if (_player.isDead)
            return;



        State = eMonsterState.ATTACK;
        isAttack = true;
    }
    public void WorkFunc()
    {
        if (m_workPos == null)
            return;
        m_agent.avoidancePriority = 51;
        State = eMonsterState.WORK;
        isworkReady = true;
    }
    public void BaseNavSetting()
    {
        m_agent.ResetPath();
        m_agent.isStopped = false;
        m_agent.updatePosition = true;
        m_agent.updateRotation = false;
    }
    public void AttackNavSetting()
    {
        m_agent.isStopped = true;
        m_agent.updatePosition = true;
        m_agent.updateRotation = false;
        m_agent.velocity = Vector3.zero;
    }

    void FreezeRotation()
    {
        m_rigid.velocity = Vector3.zero;
        m_rigid.angularVelocity = Vector3.zero;
    }



}
