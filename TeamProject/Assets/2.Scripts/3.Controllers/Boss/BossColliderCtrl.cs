using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossColliderCtrl : MonoBehaviour
{
    BossCtrl _manager;
    [SerializeField] Transform _colidersParent;
    Collider _bodyColider;
    
    public float _offsetTime;
    public float _offsetY;

    Coroutine _moveCoroutine;

    public void Init(BossCtrl manager)
    {
        _manager = manager;
        _bodyColider = _colidersParent.GetComponentInChildren<Collider>();
        _moveCoroutine = null;
    }
    
    public void ManageColider(bool isOn)
    {
        _bodyColider.enabled = isOn;
    }

    public void MoveTransformByTakeOff(bool isTakeOff)
    {
        Vector3 startPoint = _colidersParent.position;
        Vector3 endPoint;

        if (isTakeOff)
        {
            //ÀÌ·úÇÒ¶§
            endPoint = new Vector3(startPoint.x,startPoint.y + _offsetY, startPoint.z);
        }
        else
        {
            endPoint = new Vector3(startPoint.x, startPoint.y - _offsetY, startPoint.z);
        }

        if(_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        _moveCoroutine = StartCoroutine(OnMoveParentTransform(startPoint, endPoint));
    }

    IEnumerator OnMoveParentTransform(Vector3 startPos,Vector3 endPos)
    {
        float elapsedTime = 0;

        while(elapsedTime < _offsetTime)
        {
            _colidersParent.position = Vector3.Lerp(startPos, endPos, elapsedTime / _offsetTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _colidersParent.position = endPos;        
    }

    public void OnDamage(float damage, Transform attacker, Vector3 hitPoint) 
    {
        _manager.OnDamage(damage,attacker,hitPoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            if (other.TryGetComponent(out ArrowCtrl arrow))
            {
                OnDamage(arrow.Damage, arrow.Shooter, other.ClosestPoint(transform.position));
                arrow.ClearRigidBody();
            }
        }
    }
}
