using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStone : BaseSkill
{
    MonsterController _targetMonster;    
    float gravity = 9.81f;
    float firingAngle = 15.0f;    
    Coroutine ShootCoroutine = null;
    void Init(MonsterController monster)
    {
        _targetMonster = monster;
        SetInformation();
    }

    public void ThrowEvent(MonsterController monster, Vector3 start, Vector3 target, float damage)
    {
        Init(monster);
        if (Info != null)
        {
            Damage = damage * Info.DamageTimes;
        }
        else
        {
            Damage = damage;
        }
        if (ShootCoroutine != null)
            StopCoroutine(ShootCoroutine);
        ShootCoroutine = StartCoroutine(OnShootEvent(start, target));
    }

    IEnumerator OnShootEvent(Vector3 start, Vector3 target)
    {
        float dist = Vector3.SqrMagnitude(target - start);

        dist = Mathf.Sqrt(dist);

        float velocity = dist / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        float Vx = Mathf.Sqrt(velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        float flightDuration = dist / Vx;

        transform.rotation = Quaternion.LookRotation(target - start);

        float elapse_time = 0;
        while (elapse_time < flightDuration)
        {
            transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            elapse_time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(3.0f);
        if (gameObject.activeSelf)
        {
            DestoryObject();
        }
    }


    public override void DestoryObject()
    {
        if(ShootCoroutine != null)
        {
            StopCoroutine(ShootCoroutine);
        }        


        base.DestoryObject();
    }

}
