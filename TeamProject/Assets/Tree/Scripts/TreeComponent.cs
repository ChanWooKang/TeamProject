using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour
{
    TreeCtrl _ctrl;
    //나무가 쓰러질때 미는 힘
    [SerializeField]
    private float _pushPower;
    //쓰러진 나무 비활성화 시간
    [SerializeField]
    private float _destroyTime;    

    //쓰러지는 작업을 할 나무 윗 부분
    [SerializeField]
    private GameObject _childTree;
    [SerializeField]
    private CapsuleCollider _parentCollider;
    [SerializeField]
    private CapsuleCollider _childCollider;
    [SerializeField]
    private Rigidbody _childRigidBody;    
   
    public void Init(TreeCtrl ctrl)
    {
        _ctrl = ctrl;
        _parentCollider.enabled = true;
        _childCollider.enabled = false;
        _childRigidBody.useGravity = false;
        
    }

    public void FallDownTree()
    {
        _parentCollider.enabled = false;
        _childCollider.enabled = true;
        _childRigidBody.useGravity = true;

        Vector3 dir;

        //랜덤한 방향
        //dir = new Vector3(Random.Range(-_pushPower, _pushPower), 0, Random.Range(-_pushPower, _pushPower));
        //플레이어 바라보는 방향
        dir = GameManagerEx._inst.playerManager.transform.forward * _pushPower;

        _childRigidBody.AddForce(dir);

        StartCoroutine(OnFallenEvent());
    }

    IEnumerator OnFallenEvent()
    {        
        yield return new WaitForSeconds(_destroyTime);

        //아이템 생성
        _ctrl.OnDeadEvent();
        _childTree.gameObject.SetActive(false);
    }
}
