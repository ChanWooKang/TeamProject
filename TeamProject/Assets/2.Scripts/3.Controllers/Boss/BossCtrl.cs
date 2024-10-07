using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineDatas;


public class BossCtrl : FSM<BossCtrl>, IHitAble
{
    [Header("Boss Data")]
    public int Index;
    public int MonsterLevel = 1;

    [Header("Components")]
    public MonsterStat Stat;
    public BossMoveCtrl _move;
    public BossAnimCtrl _anim;
    public BossRenderCtrl _render;
    public BossColliderCtrl _collider;
    public HudController _hudCtrl;

    Rigidbody _rigid;

    [Header("Transforms")]    
    public Transform _hudTransform;

    [Header("Datas")]
    public float delayTime = 2.0f;
    public float attackRange;    

    [Header("OtherComponent")]
    [HideInInspector]public PlayerCtrl player;

    public eAttackType _attackType;
    eBossState _nowState;
    public List<SkillInfo> _mySkills;
    [HideInInspector] public Transform target;
    [HideInInspector] public Vector3 targetPos;
    
    public Vector3 _offSetHitPoint;

    public bool isDead;
    public bool isAttack;
    public bool isReturn;
    public bool isBuffed;
    public bool isSettingTarget;

    Coroutine DamageCoroutine = null;
    Coroutine RegenerateCoroutine = null;

    public int _patrolCount = 0;
    public int _maxPatrolCount = 5;

    public NavMeshAgent Agent { get { return _move.Agent; } }    

    public eBossState State
    {
        get { return _nowState; }
        set
        {
            _nowState = value;
            _anim.ChangeAnimation(_nowState);
        }
    }

    private void Awake()
    {
        InitComponent();
    }

    private void Start()
    {
        InitState(this, BossStateInit._inst);
        _mySkills = Managers._data.GetSkillList(Index);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            State = eBossState.DIE;
        }        
        FSMUpdate();
    }

    private void FixedUpdate()
    {
        FreezeRotation();
    }

    void InitComponent()
    {
        Stat = GetComponent<MonsterStat>();               
        _move = GetComponent<BossMoveCtrl>();
        _anim = GetComponent<BossAnimCtrl>();
        _render = GetComponent<BossRenderCtrl>();
        _collider = GetComponent<BossColliderCtrl>();
        _rigid = GetComponent<Rigidbody>();

        Stat.Init(Index);
        _move.Init(this);
        _anim.Init(this);
        _render.Init();
        _collider.Init(this);
    }

    public void SetHud(HudController hud, Transform hudRoot)
    {
        _hudCtrl = hud;
        _hudCtrl.gameObject.transform.SetParent(hudRoot);
        _hudCtrl.InitHud(Stat.MonsterInfo.NameKr, Stat.Level, _hudTransform, Color.red, false);
    }

    public void ShowHud()
    {
        if (_hudCtrl != null)
            _hudCtrl.DisPlay(Stat.HP / Stat.MaxHP);
    }

    public void InitData()
    {
        target = null;
        targetPos = Vector3.zero;
        _attackType = eAttackType.None;
        isDead = false;
        isAttack = false;
        isReturn = false;
        isBuffed = false;
        isSettingTarget = false;
    }

    void FreezeRotation()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
    }

    //보스는 플레이어만 인식?
    public void SetTarget(bool Return = false)
    {
        if (player == null)
        {
            if (GameManagerEx._inst.playerManager != null)
            {
                //Target 설정
                player = GameManagerEx._inst.playerManager;
            }
        }        

        if(target == null)
        {
            ChangeState(BossStateGrowl._inst);
        }

        target = Return ? null : player.transform;        
    }

    public void SettingMonsterStatByLevel()
    {
        Stat.Level = MonsterLevel;
        Stat.SetByLevel();
    }

    public void AttackFunc()
    {
        Agent.avoidancePriority = 51;
        State = eBossState.ATTACK;
        isAttack = true;
    }

    public void GetRangeByAttackType()
    {
        _attackType = _anim.ChooseAttackType(Index);
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

    public void OnDamage(float damage, Transform attacker)
    {
        Vector3 hitPoint = transform.position + _offSetHitPoint;
        OnDamage(damage, attacker, hitPoint);
    }

    public void OnDamage(float damage, Transform attacker, Vector3 hitPoint)
    {
        if (isDead) return;

        if (target == null)
            SetTarget();

        if (_hudCtrl != null)
            _hudCtrl.DisPlay(Stat.HP / Stat.MaxHP);

        State = eBossState.GETHIT;
        isDead = Stat.CalculateDamage(damage);
        DamageTextManager._inst.ShowDamageText(hitPoint, damage);

        if (DamageCoroutine != null)
            StopCoroutine(DamageCoroutine);
        DamageCoroutine = StartCoroutine(OnDamageEvent());
    }

    IEnumerator OnDamageEvent()
    {
        if (isDead)
        {
            Agent.SetDestination(transform.position);
            _move.AttackNavSetting();            
            yield return new WaitForSeconds(0.2f);

            _collider.ManageColider(false);
            Stat.DeadFunc(player._stat);
            _render.ChangeColor(Color.gray);
            ChangeState(BossStateDie._inst);
            yield break;
        }

        _render.ChangeColor(Color.red);
        yield return new WaitForSeconds(0.3f);
        _render.ChangeColor(Color.white);
        
    }

    public void OnRegenerate()
    {
        if (RegenerateCoroutine != null)
            StopCoroutine(RegenerateCoroutine);
        RegenerateCoroutine = StartCoroutine(RegenerateHP());
    }

    public void OffRegenerate()
    {
        if (RegenerateCoroutine != null)
            StopCoroutine(RegenerateCoroutine);
    }

    IEnumerator RegenerateHP()
    {
        float hpRate = Stat.MaxHP * 0.15f;
        while(Stat.HP <= Stat.MaxHP)
        {
            Stat.HP += hpRate;
            yield return new WaitForSeconds(1.0f);
        }

        Stat.HP = Stat.MaxHP;
    }

    public void OnDeadEvent()
    {
        SpawnManager._inst.MonsterDespawn(gameObject);
        _render.ChangeColor(Color.white);
        ChangeState(BossStateDisable._inst);        
    }

    public void OnResurrectEvent()
    {
        _collider.ManageColider(true);
        _render.ChangeLayer(eLayer.Monster);
        ChangeState(BossStateInit._inst);
    }

    public bool CheckAttackType(WeaponType type)
    {
        return true;
    }
}
