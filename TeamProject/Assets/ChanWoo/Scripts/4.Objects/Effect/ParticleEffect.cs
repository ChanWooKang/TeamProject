using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : ObjectInParticle
{
    ParticleCallBack _particle;

    void Init()
    {
        if(_particle == null)
            _particle = GetComponentInChildren<ParticleCallBack>();
    }

    public void HitEvent(Vector3 hitPoint)
    {
        Init();
        transform.position = hitPoint;
        _particle.Play(gameObject);
    }

    public override void DestoryObject()
    {
        _particle.StopParticle();
        base.DestoryObject();
    }
}
