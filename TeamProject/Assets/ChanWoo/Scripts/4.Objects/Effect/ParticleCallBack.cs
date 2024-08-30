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
            Debug.Log(1);
            if (parentObject.TryGetComponent(out ObjectInParticle parent))
            {
                switch (psType)
                {
                    case ParticleType.Destroy:
                        Debug.Log(2);
                        parent.DestoryObject();
                        break;
                    case ParticleType.NextParticle:

                        break;
                    case ParticleType.BuffEnd:

                        break;
                }
            }
            else
            {
                Debug.Log(3);
            }
        }
        else
        {
            Debug.Log(4);
        }
    }
}
