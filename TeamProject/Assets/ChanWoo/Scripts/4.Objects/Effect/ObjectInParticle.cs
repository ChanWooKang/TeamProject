using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInParticle : MonoBehaviour
{
    public virtual void PlayNextParticle()
    {

    }

    public virtual void BuffEffectEnd()
    {

    }

    public virtual void DestoryObject()
    {
        gameObject.DestroyAPS();
    }
}
