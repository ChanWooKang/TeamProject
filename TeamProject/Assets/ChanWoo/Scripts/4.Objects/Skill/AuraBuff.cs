using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraBuff : ObjectInParticle
{
    [SerializeField] ParticleCallBack startParticle;    
    List<MonsterController> targetMonsters;
    float _range = 5.0f;
    [SerializeField] LayerMask _targetLayer;

    public void BuffEvent()
    {
        targetMonsters = new List<MonsterController>();
        startParticle.Play(gameObject);
        //1초뒤 레이 쏠거임
        Invoke("ShootRay", 1.0f);
    }

    public void ShootRay()
    {
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, _range, Vector3.up, _targetLayer);
        if (rayHits.Length > 0)
        {
            foreach(RaycastHit rhit in rayHits)
            {
                if (rhit.transform.CompareTag("Monster"))
                {
                    if(rhit.transform.TryGetComponent(out MonsterController mc))
                    {
                        targetMonsters.Add(mc);
                        mc.GetBuffEffect();
                    }
                }
            }
        }
    }

    public override void PlayNextParticle()
    {
        for (int i = 0; i < targetMonsters.Count; i++)
        {
            targetMonsters[i].OnBuffed(true);
        }

        gameObject.DestroyAPS();
    }
}
