using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{    
    Transform _shooter;
    Rigidbody rigidBody;
    float _attackDamage;
   
    float _shootPower = 100.0f;    
    
    public Transform Shooter { get { return _shooter; } }
    public float Damage { get { return _attackDamage; } }

    public void Init(Transform shooter, float attackDamage)
    {
        rigidBody = GetComponent<Rigidbody>();        
        _shooter = shooter;
        _attackDamage = attackDamage;               
    }

    public void ShootArrow(Transform shooter,float attackDamage)
    {
        Init(shooter, attackDamage);


        //float radianAngle = firingAngle * Mathf.Deg2Rad;

        // »˚¿« ∫§≈Õ∏¶ ∞ËªÍ«’¥œ¥Ÿ.
        //Vector3 force = new Vector3(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle), 0) * _shootPower;
        //rigidBody.AddForce(force, ForceMode.Impulse);
        //rigidBody.AddForce(_arrowPoint.transform.forward * _shootPower, ForceMode.Impulse);
        rigidBody.AddForce(Camera.main.transform.forward * _shootPower, ForceMode.Impulse);
        Invoke("DestoryAtTime", 3.0f);
    }    

    void DestoryAtTime()
    {
        if(gameObject.activeInHierarchy)
        {
            ClearRigidBody();
        }
            
    }

    public void ClearRigidBody()
    {
        rigidBody.velocity = Vector3.zero;
        rigidBody.inertiaTensor = Vector3.zero;
        gameObject.DestroyAPS();
    }

}
