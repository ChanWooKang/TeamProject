using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineDatas;

public class MonsterCtrl : FSM<MonsterCtrl>
{
    public MonsterStat Stat;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Renderer _render;
    [SerializeField] Rigidbody _rigid;

    //나중엔 게임매니저로 연동
    public PlayerCtrl playerController;
    public Transform target = null;
    [SerializeField] eMonsterState _nowState = eMonsterState.IDLE;    
    public Vector3 _offSet = Vector3.zero;    
    public Vector3 targetPos;
    public float delayTime;
    [SerializeField, Range(8, 15)] float rotateSpeed;
    public bool isDead;
    public bool isAttack;

    public NavMeshAgent Agent { get { return _agent; } }
    public eMonsterState State 
    {
        get { return _nowState; }
        set
        {
            _nowState = value;
            //애니메이션 변경
        }
    }

    void Start()
    {
        Init();
        InitState(this, MonsterStateInitial._inst);
    }

    void Update()
    {
        FSMUpdate();
    }

    void FixedUpdate()
    {
        FreezeRotate();
    }

    void Init()
    {        
        isDead = false;
    }

    void FreezeRotate()
    {
        _rigid.velocity = Vector3.zero;
        _rigid.angularVelocity = Vector3.zero;
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
        _agent.updatePosition = false;
        _agent.updateRotation = false;
        _agent.velocity = Vector3.zero;
    }

    public void SetTarget()
    {
        //if (playerController.isDead == false)
        //    target = playerController.transform;
        //else
        //    target = null;            
    }

    void ChangeColor(Color color)
    {
        _render.material.color = color;
    }

    public Vector3 GetRandomPos(float range = 5.0f)
    {
        Vector3 pos = Random.onUnitSphere;
        pos.y = 0;
        float r = Random.Range(1, range);
        pos = _offSet + (pos * r);

        NavMeshPath path = new NavMeshPath();
        if (_agent.CalculatePath(pos, path))
            return pos;
        else
            return GetRandomPos();
    }

    public bool IsCloseTarget(Vector3 pos, float range)
    {
        Vector3 goalPos = new Vector3(pos.x, transform.position.y, pos.z);
        float dist = Vector3.SqrMagnitude(goalPos - transform.position);
        if (dist < range * range)
            return true;        
        return false;
    }

    public void MoveEvent(Vector3 goalPos)
    {
        Vector3 dir = goalPos - transform.position;
        _agent.SetDestination(goalPos);
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(dir), rotateSpeed * Time.deltaTime);
    }

    public void LookPlayer()
    {
        if(playerController != null)
        {
            Vector3 dir = playerController.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                Quaternion.LookRotation(dir),rotateSpeed * Time.deltaTime);
        }
    }

    public void AttackEvent()
    {
        //if (target == null || playerController.isDead)
        //    return;
        //_agent.avoidancePriority = 51;
        //State = eMonsterState.ATTACK;
        //isAttack = true;
        //StopCoroutine(AttackCoroutine(Stat.Damage));
        //StartCoroutine(AttackCoroutine(Stat.Damage));
    }

    public void OffAttackEvent()
    {
        _agent.avoidancePriority = 50;
        isAttack = false;
    }


    //애니메이션 대체 코루틴
    IEnumerator AttackCoroutine(float damage)
    {
        yield return new WaitForSeconds(0.25f);
        //if (target == null || playerController.isDead)
        //    yield break;

        //if (IsCloseTarget(target.position, Stat.AttackRange))
        //    playerController.OnDamage(damage);

        yield return new WaitForSeconds(0.25f);
        OffAttackEvent();
    }

    public void OnDamage(float damage)
    {
        if (isDead)
            return;

        isDead = Stat.GetHit(damage);
        StopCoroutine(OnDamageEvent());
        StartCoroutine(OnDamageEvent());
    }

    IEnumerator OnDamageEvent()
    {
        if (isDead)
        {
            //죽었을때 행동
            ChangeColor(Color.black);
            ChangeState(MonsterStateDie._inst);
            yield break;
        }
        ChangeColor(Color.gray);
        yield return new WaitForSeconds(0.3f);
        ChangeColor(Color.magenta);
    }

    public void OnDeadEvent()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            //if(other.TryGetComponent(out WeaponCtrl wc))
            //{
            //    OnDamage(wc.weaponDamage);
            //    wc.OnAttack();
            //}
        }
    }
   
}
