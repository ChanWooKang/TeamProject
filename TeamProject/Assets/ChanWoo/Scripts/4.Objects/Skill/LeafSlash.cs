using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSlash : BaseSkill
{
    Rigidbody _rigid;
    BoxCollider _collider;
    ParticleSystem _particle;
    Vector3 direction;
    float _offSetPosY;    
    [SerializeField] float power;
    bool isShoot = false;

    Coroutine SlashCoroutine = null;

    private void FixedUpdate()
    {
        if(isShoot && direction != Vector3.zero)
        {
            _rigid.velocity = Vector3.zero;
            _rigid.AddForce(transform.forward * power, ForceMode.Impulse);
            isShoot = false;
        }
    }

    void Init()
    {        
        _rigid = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _particle = GetComponentInChildren<ParticleSystem>();

        SetInformation();
        _offSetPosY = 0.8f;
        SetEnable(false);
    }

    void SetEnable(bool check)
    {
        _collider.enabled = check;
    }

    void SetPosition(Transform pos)
    {
        Vector3 position = pos.position;
        position.y += _offSetPosY;
        transform.position = pos.forward + position;
        transform.rotation = Quaternion.LookRotation(pos.forward);
    }

    public void SlashEvent(Transform shooter, float damage, Vector3 destination)
    {
        Init();
        SetPosition(shooter);
        direction = destination;
        direction.y = 0;


        if(Info != null)
        {
            Damage = damage * Info.DamageTimes;
        }
        else
        {
            Damage = damage;
        }        
        if (SlashCoroutine != null)
            StopCoroutine(SlashCoroutine);
        SlashCoroutine = StartCoroutine(OnSlashEvent());
        
    }

    IEnumerator OnSlashEvent()
    {
        isShoot = true;
        yield return new WaitForSeconds(0.1f);
        _particle.Play();
        SetEnable(true);
        yield return new WaitForSeconds(1.8f);
        SetEnable(false);
        gameObject.DestroyAPS();
    }
}
