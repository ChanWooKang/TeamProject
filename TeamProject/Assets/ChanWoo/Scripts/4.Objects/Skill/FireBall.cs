using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : BaseSkill
{

    Rigidbody _rigidbody;
    Collider _collider;
    ParticleSystem _particle;

    [SerializeField] float _shootPower;
    [SerializeField] float _remainTime;
    bool isShoot = false;

    Vector3 direction;
    Coroutine ShootCoroutine = null;

    private void FixedUpdate()
    {
        if(isShoot && direction != Vector3.zero)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.AddForce(transform.forward * _shootPower, ForceMode.Impulse);
            isShoot = false;
        }
    }

    void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _particle = GetComponentInChildren<ParticleSystem>();
        SetInformation();
    }

    public override void DestoryObject()
    {
        if (ShootCoroutine != null)
            StopCoroutine(ShootCoroutine);
        _particle.Stop();
        SetEnable(false);
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.inertiaTensor = Vector3.zero;
        base.DestoryObject();
    }

    void SetEnable(bool isEnable)
    {
        _collider.enabled = isEnable;
    }

    public void SetRotaiton(Vector3 dir)
    {
        direction = dir;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void ShootEvent(Vector3 shootDirect,float damage, BossCtrl bCtrl)
    {
        Init();
        SetRotaiton(shootDirect);
        Damage = damage;
        
        if (ShootCoroutine != null)
            StopCoroutine(ShootCoroutine);
        ShootCoroutine = StartCoroutine(OnShootEvent());
        SetBoss(bCtrl);
    }

    IEnumerator OnShootEvent()
    {
        isShoot = true;
        _particle.Play();
        SetEnable(true);
        yield return new WaitForSeconds(_remainTime);
        _particle.Stop();
        SetEnable(false);
        DestoryObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Block") || other.CompareTag("Ground"))
        {
            DestoryObject();
        }
    }
}
