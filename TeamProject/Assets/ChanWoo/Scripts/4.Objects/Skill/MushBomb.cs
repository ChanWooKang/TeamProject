using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushBomb : ObjectInParticle
{
    Animator _animator;
    Rigidbody _rigid;

    [SerializeField] GameObject _model;
    [SerializeField] ParticleCallBack _particle;
    MonsterController _targetMonster;

    int _animIDBomb;
    
    float gravity = 9.81f;
    float firingAngle = 45.0f;
    [SerializeField] float castRange = 15.0f;
    [SerializeField] LayerMask castMask;
    public float Damage;

    Coroutine ShootCoroutine = null;

    void Init(MonsterController target)
    {
        _targetMonster = target;
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _animIDBomb = Animator.StringToHash("Bomb");
        SetEnable(false);
    }

    void SetEnable(bool enable)
    {
        if(_model != null)
        {
            _model.SetActive(enable);
        }
    }

    public void BombEvent(MonsterController monster,Vector3 start, Vector3 target, float damage)
    {
        Init(monster);
        SetEnable(true);
        Damage = damage;
        if (ShootCoroutine != null)
            StopCoroutine(ShootCoroutine);
        ShootCoroutine = StartCoroutine(OnShootEvent(start,target));
    }

    IEnumerator OnShootEvent(Vector3 start, Vector3 target)
    {        
        float dist = Vector3.SqrMagnitude(target - start);        
        
        dist = Mathf.Sqrt(dist);        

        float velocity = dist / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float flightDuration = dist / Vx;

        transform.rotation = Quaternion.LookRotation(target - start);

        float elapse_time = 0;
        while(elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }

        _animator.SetTrigger(_animIDBomb);
    }

    public void BombEffect()
    {
        ShootRay();

        _particle.Play(gameObject);

        SetEnable(false);        
    }

    public void ShootRay()
    {        
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, castRange, Vector3.up, castMask);

        if(rayHits.Length > 0)
        {
            foreach (RaycastHit rhit in rayHits)
            {
                if (rhit.transform.CompareTag("Player"))
                {
                    if(rhit.transform.TryGetComponent(out PlayerManager player))
                    {
                        player.OnDamage(Damage);
                    }                    
                }

                if (rhit.transform.CompareTag("Monster"))
                {
                    if(rhit.transform.TryGetComponent(out MonsterController mc))
                    {
                        if(mc != _targetMonster)
                        {
                            mc.OnDamage(Damage, _targetMonster.transform);
                        }
                    }
                }
            }
        }               
    }

    public override void DestoryObject()
    {
        gameObject.DestroyAPS();
    }
}
