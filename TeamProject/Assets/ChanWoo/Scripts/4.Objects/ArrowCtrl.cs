using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{    
    Transform _shooter;
    Rigidbody rigidBody;
    float _attackDamage;
   
    float _shootPower = 20.0f;    
    float gravity = 9.81f;
    float firingAngle = 20.0f;

    Coroutine ShootCoroutine = null;
    public Transform Shooter { get { return _shooter; } }
    public float Damage { get { return _attackDamage; } }

    public void Init(Transform shooter, float attackDamage)
    {
        rigidBody = GetComponent<Rigidbody>();        
        _shooter = shooter;
        _attackDamage = attackDamage;               
    }

    public void ShootArrow(Transform shooter, Transform _arrowPoint,float attackDamage)
    {
        Init(shooter, attackDamage);

        
        rigidBody.AddForce(_arrowPoint.forward * _shootPower,ForceMode.Impulse);        
        Invoke("DestoryAtTime", 3.0f);
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
        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }

       
        yield return new WaitForSeconds(2.0f);
        if (gameObject.activeInHierarchy)
        {
            gameObject.DestroyAPS();
        }
    }

    void DestoryAtTime()
    {
        if(gameObject.activeInHierarchy) 
            gameObject.DestroyAPS();
    }

}
