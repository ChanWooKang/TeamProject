using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineDatas;

public class PetController : FSM<PetController>
{

    
    int Index;

    #region [Component]
    [SerializeField] PetStat m_stat;
    [SerializeField] PetMovement _movement;
    [SerializeField] PetAnimController m_animCtrl;
    NavMeshAgent m_agent;
    Animator m_animator;
    CapsuleCollider m_collider;
    Rigidbody m_rigid;
    MonsterInfo m_petInfo;
    public HudController _hudCtrl;
    Coroutine DamageCoroutine = null;
    Renderer[] _meshs;
    Vector3 _offSetHitPoint;
    #endregion [Component]

    //몬스터 상태 체크 및 애니메이션 적용     
    [HideInInspector] public PlayerCtrl _player;
    [HideInInspector] public eAttackType _attackType;
    [SerializeField] eMonsterState _nowState;
    public Transform _hudTransform;
    //타겟 관련
    [HideInInspector] public MonsterController _targetMon;
    [HideInInspector] public BossCtrl _targetBoss;
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
        player = GameManagerEx._inst.playerManager.transform;
        //
    }
    private void Start()
    {
        InitState(this, PetStateInit._inst);
    }
    private void Update()
    {
        if ((_targetMon != null && _targetMon.isDead))
        {
            target = null;
            _targetMon = null;
        }
        if (_targetBoss != null && _targetBoss.isDead)
        {
            target = null;
            _targetBoss = null;
        }

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
        Index = index;
        m_petInfo = Managers._data.Dict_Monster[index];
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<CapsuleCollider>();
        m_rigid = GetComponent<Rigidbody>();
        _meshs = GetComponentsInChildren<Renderer>();
        m_animCtrl.Init(this, m_animator);
        _movement.Init(this, m_agent);
        Stat.Init(index);
        SettingPetStatByLevel();
    }
    public void SettingPetStatByLevel()
    {
        Stat.Level = 1;
        Stat.SetByLevel();
        if (_hudCtrl != null)
        {
            _hudCtrl.InitHud(m_petInfo.NameKr, Stat.Level, _hudTransform, Color.green, true, this);
            SetHudHp();
        }
    }
    public void InitPetHud()
    {
        if (_hudCtrl != null)
        {
            _hudCtrl.InitHud(m_petInfo.NameKr, Stat.Level, _hudTransform, Color.green, true, this);
            SetHudHp();
        }
    }
    public void GetRangeByAttackType()
    {
        _attackType = m_animCtrl.ChooseAttackType(Index);
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
        if (target != null)
            return;


        if (attacker != null)
            if (attacker.TryGetComponent(out MonsterController mCtrl))
            {
                _targetMon = mCtrl;
                target = attacker;
            }
            else if (attacker.TryGetComponent(out BossCtrl bCtrl))
            {
                _targetBoss = bCtrl;
                target = attacker;
            }
            else if (attacker.TryGetComponent(out BaseSkill bSkill))
            {
                if (bSkill.MonCtrl != null)
                {
                    _targetMon = bSkill.MonCtrl;
                    target = _targetMon.transform;
                }
                else if (bSkill.BossCtrl != null)
                {
                    _targetBoss = bSkill.BossCtrl;
                    target = _targetBoss.transform;
                }
            }


    }
    public void SetHud(HudController hud, Transform hudRoot)
    {
        _hudCtrl = hud;
        _hudCtrl.gameObject.transform.SetParent(hudRoot);
        SettingPetStatByLevel();
    }
    public void SetHudHp()
    {
        if (_hudCtrl != null && gameObject.activeSelf == true)
            _hudCtrl.DisPlay(Stat.Level, Stat.HP / Stat.MaxHP);
    }
    public void AttackFunc()
    {
        //플레이어 상태 확인 
        //if (_player.isDead)
        //    return;



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
    public void ReCall()
    {
        InitState(this, PetStateInit._inst);
    }
    public void OnDamage(float damage, Transform attacker)
    {
        Vector3 hitPoint = transform.position + _offSetHitPoint;
        OnDamage(damage, attacker, hitPoint);
    }
    public void OnDamage(float damage, Transform attacker, Vector3 hitPoint)
    {
        if (isDead)
            return;
        isStatic = false;

        if (target != attacker)
            SetTarget(attacker);

        State = eMonsterState.GETHIT;
        isDead = Stat.CalculateDamage(damage);
        if (_hudCtrl != null)
        {
            _hudCtrl.DisPlay(Stat.Level, Stat.HP / Stat.MaxHP);
            UIManager._inst.UIPetEntry.SetRecalledPetHud();
        }
        DamageTextManager._inst.ShowDamageText(hitPoint, damage);
        OnDamage();
    }
    public void OnDamage()
    {
        if (DamageCoroutine != null)
            StopCoroutine(DamageCoroutine);
        DamageCoroutine = StartCoroutine(OnDamageEvent());
    }
    IEnumerator OnDamageEvent()
    {
        float randValue = Random.Range(0.0f, 1.0f);
        bool isDizzy = randValue <= dizzyRate;

        if (isDead)
        {
            m_agent.SetDestination(transform.position);
            AttackNavSetting();
            yield return new WaitForSeconds(0.20f);
            //죽을 때 작업
            m_collider.enabled = false;
            ChangeColor(Color.gray);
            ChangeState(PetStateDie._inst);
            yield break;
        }
        if (isDizzy)
            ChangeState(PetStateDizzy._inst);

        ChangeColor(Color.red);
        yield return new WaitForSeconds(0.3f);
        ChangeColor(Color.white);
    }

    public void ChangeLayer(eLayer layer)
    {
        gameObject.layer = (int)layer;
    }
    public void OnDeadEvent()
    {
        UIManager._inst.UIPetEntry.RecallOrPutIn();
    }
    public void SetExp(int exp)
    {      
        int next = m_stat.NextExp;
        m_stat.CurrentExp += exp;
        if (m_stat.CurrentExp >= next)
        {
            m_stat.CurrentExp -= next;
            m_stat.Level += 1;
            m_stat.SetByLevel();
            UIManager._inst.UIPetEntry.SetHudInfoBox(this);
            _hudCtrl.DisPlay(m_stat.Level, m_stat.MaxHP);
        }
        if (m_stat.CurrentExp >= next)
            SetExp(exp);
    }
    void ChangeColor(Color color)
    {
        if (_meshs.Length > 0)
        {
            foreach (Renderer mesh in _meshs)
                mesh.material.color = color;
        }
    }
    void FreezeRotation()
    {
        m_rigid.velocity = Vector3.zero;
        m_rigid.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("MonsterSkill"))
        {
            if (other.TryGetComponent(out BaseSkill skill))
            {
                if (skill._subject == eSkillSubject.Monster)
                {
                    OnDamage(skill.Damage, skill.MonCtrl.transform, other.ClosestPoint(transform.position));
                    other.gameObject.DestroyAPS();
                }
                else if (skill._subject == eSkillSubject.Boss)
                {
                    OnDamage(skill.Damage, skill.BossCtrl.transform, other.ClosestPoint(transform.position));
                    other.gameObject.DestroyAPS();
                }
            }
        }
    }

}
