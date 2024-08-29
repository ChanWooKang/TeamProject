using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCallBack : MonoBehaviour
{
    GameObject parentObject;
    
    void Init()
    {
        parentObject = transform.parent.gameObject;
    }

    public void OnParticleSystemStopped()
    {
        if (parentObject == null)
            Init();

        parentObject.DestroyAPS();        
    }
}
