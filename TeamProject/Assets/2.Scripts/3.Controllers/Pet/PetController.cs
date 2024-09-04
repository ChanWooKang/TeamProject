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
    PetStat m_stat;
    NavMeshAgent m_agent;
    Animator m_animator;    
    CapsuleCollider m_collider;
    Rigidbody m_rigid;
    PetMovement _movement;
    MonsterInfo m_petInfo;
    HudController _hudCtrl;
    PetAnimController m_animCtrl;
    #endregion [Component]

    //몬스터 상태 체크 및 애니메이션 적용     
    [HideInInspector] public PlayerManager _player;
    [HideInInspector] public eAttackType _attackType;
    eMonsterState _nowState;

    //타겟 관련
    [HideInInspector] public MonsterController _targetMon;
    [HideInInspector] public Transform target;
    [HideInInspector] public Vector3 targetPos;

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
    Vector3? m_targetPos = null;

    private void Awake()
    {
        //임시
        InitPet(1000);
        InitState(this, PetStateInit._inst);

        //
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
        m_targetPos = targetPos;
        StartCoroutine(MovePetToTarget());
    }

    public void InitPet(int index)
    {
        m_petInfo = Managers._data.Dict_Monster[index];
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<CapsuleCollider>();
        m_rigid = GetComponent<Rigidbody>();
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
    public void SetTarget(Transform attacker, bool isPlayer = true)
    {
        target = attacker;

        if (isPlayer)
        {
            if (!GameManagerEx._inst.playeManager.isDead)
            {
                if (_player == null)
                    _player = GameManagerEx._inst.playeManager;
            }
        }
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





    IEnumerator MovePetToTarget()
    {
        while (Vector3.Distance(transform.position, m_targetPos.Value) >= 2)
        {
            if (m_targetPos != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_targetPos.Value, m_petInfo.Speed * Time.deltaTime);
            }
        }
        m_targetPos = null;
        yield return null;
    }
}
