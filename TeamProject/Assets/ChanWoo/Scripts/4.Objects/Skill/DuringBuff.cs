using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuringBuff : ObjectInParticle
{
    [SerializeField] ParticleCallBack _myParticle;
    MonsterController _myTarget;


    public void BuffOn(MonsterController target)
    {
        _myTarget = target;
        _myParticle.Play(gameObject, true);
    }

    public override void BuffEffectEnd()
    {
        if(_myTarget != null)
        {
            _myTarget.OnBuffed(false);
        }

        gameObject.DestroyAPS();
    }
}
