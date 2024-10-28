using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFallEffect : ObjectInParticle
{
    ParticleCallBack _particle;
    void Init()
    {
        if(_particle == null)
            _particle = GetComponentInChildren<ParticleCallBack>();        
    }

    public void FallEvent()
    {
        Init();
        _particle.Play(gameObject);
    }

    public override void DestoryObject()
    {
        _particle.StopParticle();
        base.DestoryObject();
    }
}
