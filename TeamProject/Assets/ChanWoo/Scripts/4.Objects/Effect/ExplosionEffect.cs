using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    ParticleSystem ps;

    public void Init()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        if(ps == null)
            Init();
        ps.Play(true);
    }

    public void OnParticleSystemStopped()
    {
        gameObject.DestroyAPS();
    }
}
