using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushBomb : MonoBehaviour
{
    Animator _animator;
    Rigidbody _rigid;    

    int _animIDBomb;
    
    float gravity = 9.81f;
    float firingAngle = 45.0f;
    public float Damage;

    Coroutine ShootCoroutine = null;

    void Init()
    {
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody>();
        _animIDBomb = Animator.StringToHash("Bomb");
        
    }


    public void BombEvent(Vector3 start, Vector3 target, float damage)
    {
        Init();
        Damage = damage;
        if (ShootCoroutine != null)
            StopCoroutine(ShootCoroutine);
        ShootCoroutine = StartCoroutine(OnShootEvent(start,target));
    }

    IEnumerator OnShootEvent(Vector3 start, Vector3 target)
    {
        
        float dist = Vector3.SqrMagnitude(target - start);
        //float dist = Vector3.Distance(start,target);
        
        dist = Mathf.Sqrt(dist);
        Debug.Log(dist);

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
        Debug.Log("BombEffect");
        GameObject go =PoolingManager._inst.InstantiateAPS("ExplosionEffect",transform.position,Quaternion.identity,Vector3.one * 0.5f);
        if(go.TryGetComponent(out ExplosionEffect effect))
        {
            effect.Play();
        }
        gameObject.DestroyAPS();
    }
}
