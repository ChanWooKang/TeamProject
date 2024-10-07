using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCallBack : MonoBehaviour
{
    enum ParticleType
    {
        Destroy,
        NextParticle,
        BuffEnd
    }

    GameObject parentObject;
    [SerializeField] ParticleType psType;
    ParticleSystem _myParticle;

    public void Init(GameObject parent)
    {
        parentObject = parent;
        _myParticle = GetComponent<ParticleSystem>();
    }

    public void Play(GameObject parent ,bool isTogether = true)
    {
        Init(parent);
        _myParticle.Play(isTogether);
    }

    public void StopParticle(bool isTogether = true)
    {
        if (_myParticle != null)
            _myParticle.Stop(isTogether);
    }

    public void OnParticleSystemStopped()
    {
        if(parentObject != null)
        {            
            if (parentObject.TryGetComponent(out ObjectInParticle parent))
            {
                switch (psType)
                {
                    case ParticleType.Destroy:                        
                        parent.DestoryObject();
                        break;
                    case ParticleType.NextParticle:
                        parent.PlayNextParticle();
                        break;
                    case ParticleType.BuffEnd:
                        parent.BuffEffectEnd();
                        break;
                }
            }            
        }        
    }
}
