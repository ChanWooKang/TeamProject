using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMovement : MonoBehaviour
{
    //몬스터 소환 기준 위치
    public Vector3 _defPos;
    //몬스터 소환 최초 위치
    public Vector3 _offsetPos;
    //회전 속도
    public float _rotateSpeed = 15.0f;

    //Test

    MonsterController _manager;
    NavMeshAgent _agent;

    public void Init(MonsterController manager, NavMeshAgent agent)
    {
        _manager = manager;
        _agent = agent;
    }

    //Patrol 상태에서 다음 이동 위치 계산
    public Vector3 GetRandomPos(float range = 5.0f)
    {
        Vector3 randPos = Random.onUnitSphere;
        randPos.y = transform.position.y;
        float r = Random.Range(1, range);
        randPos = _defPos + (randPos * r);

        NavMeshPath path = new NavMeshPath();
        if (_agent.CalculatePath(randPos, path))
            return randPos;
        else
            return GetRandomPos();
    }

    public bool CheckFarOffset(float range = 20.0f)
    {
        Vector3 goalPos = new Vector3(_offsetPos.x, transform.position.y, _offsetPos.z);
        float dist = Vector3.SqrMagnitude(goalPos - transform.position);
        if (dist > range * range)
            return true;
        return false;
    }

    public bool CheckCloseTarget(Vector3 pos, float range)
    {        
        Vector3 goalPos = new Vector3(pos.x, transform.position.y, pos.z);        
        float dist = Vector3.SqrMagnitude(goalPos - transform.position);
        if (dist < range * range)
            return true;
        return false;
    }

    public void MoveFunc(Vector3 pos)
    {
        _agent.SetDestination(pos);

        Vector3 dir = pos - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(dir), _rotateSpeed * Time.deltaTime);
    }


}
