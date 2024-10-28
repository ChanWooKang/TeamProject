using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CollisionEvent : MonoBehaviour
{
    [System.Serializable]
    public class myCollisionEvent : UnityEvent<Collision> { }
    public myCollisionEvent onCollisionEnter;    
    
    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter.Invoke(collision);
    }    

}
