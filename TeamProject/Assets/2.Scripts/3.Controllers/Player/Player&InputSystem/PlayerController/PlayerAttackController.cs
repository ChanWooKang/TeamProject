using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerAttackController : MonoBehaviour
{
    PlayerManager _manager;

    Vector3 punchPos = new Vector3(0, 1.0f, 0.8f);
    float punchRange = 1.0f;
    [SerializeField] LayerMask TargetLayer;
    public void Init(PlayerManager manager)
    {
        _manager = manager;
        
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position + punchPos, transform.forward);
        Gizmos.DrawRay(ray);
    }

    //근접 기본 공격 
    public void PunchAction()
    {
        
    }
}
