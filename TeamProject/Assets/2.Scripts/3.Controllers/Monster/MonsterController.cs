﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineDatas;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MonsterStat))]
[RequireComponent(typeof(MonsterMovement))]
public class MonsterController : FSM<MonsterController>, IHitAble
{
    [Header("Monster Data")]
    public int Index;
    public int MonsterLevel = 1;

    [Header("Component")]
    public MonsterStat Stat;
    public MonsterMovement _movement;
    public MonsterAnimCtrl _animCtrl;
    public Transform _model;
    public Transform _captureModel;
    public Transform _hudTransform;


    //Componsnent
    NavMeshAgent _agent;
    Animator _animator;
    Renderer[] _meshs;
    CapsuleCollider _collider;
    Rigidbody _rigid;

    //OtherComponent
    [HideInInspector] public PlayerCtrl _player;
    [HideInInspector] public PetBallController Ball;
    DuringBuff buffEffect;
    HudController _hudCtrl;


    //몬스터 상태 체크 및 애니메이션 적용     
    [HideInInspector] public eAttackType _attackType;
    eMonsterState _nowState;
    public List<SkillInfo> _mySkills;
    //타겟 관련
    [HideInInspector] public Transform target;
    [HideInInspector] public Vector3 targetPos;
    
    public Vector3 _offSetHitPoint;

    [Header("Datas")]
    //floats
    public float delayTime = 2.0f;
    public float attackRange;
    public float dizzyRate = 0.05f;

    //Bools
    //몬스터가 최초로 생성됬을때 갖고있는다 초식만 갖고있는다
    public bool isStatic;
    public bool isDead;
    public bool isAttack;
    public bool isReturnHome;
    public bool isBuffed;
    public bool isPlayerTarget;

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
        //최초
        //PoolingManager._inst.InstantiateAPS("HP");        
        InitState(this, MonsterStateInit._inst);
        _mySkills = Managers._data.GetSkillList(Index);
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
        Stat = GetComponent<MonsterStat>();
        Stat.Init(Index);
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _meshs = GetComponentsInChildren<Renderer>();
        _collider = GetComponent<CapsuleCollider>();
        _rigid = GetComponent<Rigidbody>();

        _movement = GetComponent<MonsterMovement>();
        _animCtrl = GetComponent<MonsterAnimCtrl>();

        _movement.Init(_agent);
        _animCtrl.Init(this, _animator);
    }
    public void InitState()
    {
        InitState(this, MonsterStateInit._inst);
    }
    public void SetHud(HudController hud, Transform hudRoot)
    {
        _hudCtrl = hud;
        _hudCtrl.gameObject.transform.SetParent(hudRoot);
        _hudCtrl.InitHud(Stat.MonsterInfo.NameKr, MonsterLevel, _hudTransform, Color.red, false);
    }
    public void ShowHud()
    {       
        if (_hudCtrl != null)
            _hudCtrl.DisPlay(Stat.Level, Stat.HP / Stat.MaxHP);
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

        switch (Stat.CharacterType)
        {
            case (int)eMonsterCharacterType.PASSIVE:
                isStatic = true;
                break;
            case (int)eMonsterCharacterType.AGGRESSIVE:
                isStatic = false;
                break;
        }

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

    public void JumpNavSetting()
    {
        _agent.SetDestination(transform.position);
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.isStopped = true;
    }

    public void MakeJump()
    {
        _rigid.isKinematic = false;
        _rigid.useGravity = true;
        _rigid.AddForce(new Vector3(0, 150, 0), ForceMode.Impulse);
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

    public void ChangeModelByCapture(bool isCapture)
    {
        _model.gameObject.SetActive(!isCapture);
        _captureModel.gameObject.SetActive(isCapture);
    }

    public void SetTarget(Transform attacker, bool isPlayer = true)
    {
        target = attacker;

        if (isPlayer)
        {
            if (!GameManagerEx._inst.playerManager.isDead)
            {
                if (_player == null)
                    _player = GameManagerEx._inst.playerManager;
            }
        }

        isPlayerTarget = isPlayer;
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
        _attackType = _animCtrl.ChooseAttackType(Index);        
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


    public Vector3 GetRunAwayPos()
    {
        //Debug.Log(target.position);
        //Vector3 reverseNormalVect = transform.position - target.position;
        //Vector3 NormalVect = target.position - transform.position;
        ////타겟에서 몬스터까지 거리 값 (제곱근)
        //float distFromTarget = reverseNormalVect.sqrMagnitude;
        //distFromTarget = Mathf.Sqrt(distFromTarget);
        ////타겟에서 몬스터의 방향 벡터
        //reverseNormalVect = reverseNormalVect.normalized;
        ////이동 거리 구하기
        ////예시로 추격 거리 밖으로 이동
        Vector3 normalVec = Camera.main.transform.forward;
        normalVec.y = 0;
        transform.LookAt(normalVec);
        return transform.position + (normalVec * Stat.ChaseRange);
    }

    public void OnDamage(float damage, Transform attacker)
    {
        Vector3 hitPoint = transform.position + _offSetHitPoint;
        OnDamage(damage,attacker,hitPoint);
    }

    public void OnDamage(float damage, Transform attacker, Vector3 hitPoint)
    {
        if(isDead)
            return;
        isStatic = false;

        if (target != attacker)
            SetTarget(attacker);

        

        State = eMonsterState.GETHIT;
        isDead = Stat.CalculateDamage(damage);

        if (_hudCtrl != null)
            _hudCtrl.DisPlay(Stat.Level, Stat.HP / Stat.MaxHP);

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

        SoundManager._inst.PlaySfx("GetHit");   
        if (isDead)
        {
            _agent.SetDestination(transform.position);
            AttackNavSetting();
            yield return new WaitForSeconds(0.20f);
            //죽을 때 작업
            _collider.enabled = false;
            Stat.DeadFunc(_player);
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
        _hudCtrl.HideHud();
        SpawnManager._inst.MonsterDespawn(gameObject);
        int[] rewards = Stat.MonsterInfo.Rewards;
        int rewardCount = Stat.MonsterInfo.RewardCounts;        
        SpawnManager._inst.SpawnItemWithRate(rewards,rewardCount,transform);
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

    public void GetBuffEffect()
    {
        if (isBuffed == false)
        {
            GameObject go = PoolingManager._inst.InstantiateAPS("DuringBuff", transform.position, Quaternion.identity, Vector3.one, transform);
            if (go.TryGetComponent(out DuringBuff buff))
            {
                buffEffect = buff;
                isBuffed = true;
            }
        }
        else
        {
            if (buffEffect != null)
            {
                buffEffect.gameObject.DestroyAPS();
                buffEffect = null;
                isBuffed = false;
                //스탯 정상화

                //버프 다시 설정
                GetBuffEffect();
            }
        }
    }

    public void OnBuffed(bool isOn)
    {
        if (isOn)
        {
            //버프 설정
            //스탯 설정 
            if (buffEffect != null)
            {
                buffEffect.BuffOn(this);
                isBuffed = true;
            }
        }
        else
        {
            //버프 해제
            //스탯 정상화 
            if (buffEffect != null)
            {
                buffEffect = null;
                isBuffed = false;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {            
            if(other.TryGetComponent(out ArrowCtrl arrow))
            {
                OnDamage(arrow.Damage, arrow.Shooter,other.ClosestPoint(transform.position));
                arrow.ClearRigidBody();
            }
        }

        if (other.CompareTag("Weapon"))
        {
            if(other.TryGetComponent(out BaseWeaponCtrl weapon))
            {                
                OnDamage(weapon.TotalDamage, GameManagerEx._inst.playerManager.transform, other.ClosestPoint(transform.position));
            }
        }

        if(other.CompareTag("MonsterSkill"))
        {
            if(other.TryGetComponent(out BaseSkill skill))
            {
                if(skill._subject == eSkillSubject.Pet)
                {
                    OnDamage(skill.Damage, skill.PetCtrl.transform, other.ClosestPoint(transform.position));
                    other.gameObject.DestroyAPS();
                }
            }
        }
    }

    public bool CheckAttackType(WeaponType type)
    {
        return true;
    }
}
