using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BaseStoneCtrl : HitObjectCtrl, IHitAble
{
    [SerializeField] float _destoryTime;
    Coroutine DeadCoroutine = null;

    void MakeStoneEffect(Vector3 hitPoint)
    {
        GameObject go = PoolingManager._inst.InstantiateAPS("StoneHitEffect");
        StoneEffect effect = go.GetComponent<StoneEffect>();
        effect.HitEvent(hitPoint);
    }

    public override void OnDamage(float damage, Transform attacker, Vector3 hitPoint)
    {
        if (!isInit)
            Init();
        base.OnDamage(damage, attacker, hitPoint);
        MakeStoneEffect(hitPoint);
    }

    public override void OnDamage()
    {
        if (isDead)
        {
            if(DeadCoroutine != null)            
                StopCoroutine(DeadCoroutine);

            DeadCoroutine = StartCoroutine(DeadCoroutineEvent());
        }
    }

    public override void OnDeadEvent()
    {
        base.OnDeadEvent();
        gameObject.DestroyAPS();
    }

    IEnumerator DeadCoroutineEvent()
    {
        yield return new WaitForSeconds(_destoryTime);
        OnDeadEvent();        
    }
}
