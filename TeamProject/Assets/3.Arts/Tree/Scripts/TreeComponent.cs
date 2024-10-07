using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeComponent : MonoBehaviour
{
    TreeCtrl _ctrl;
    //������ �������� �̴� ��
    [SerializeField]
    private float _pushPower;
    //������ ���� ��Ȱ��ȭ �ð�
    [SerializeField]
    private float _destroyTime;    

    //�������� �۾��� �� ���� �� �κ�
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

        //������ ����
        //dir = new Vector3(Random.Range(-_pushPower, _pushPower), 0, Random.Range(-_pushPower, _pushPower));
        //�÷��̾� �ٶ󺸴� ����
        dir = GameManagerEx._inst.playerManager.transform.forward * _pushPower;

        _childRigidBody.AddForce(dir);

        StartCoroutine(OnFallenEvent());
    }

    IEnumerator OnFallenEvent()
    {        
        yield return new WaitForSeconds(_destroyTime);

        //������ ����
        _ctrl.OnDeadEvent();
        _childTree.gameObject.SetActive(false);
    }
}
