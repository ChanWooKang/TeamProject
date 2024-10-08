using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class ItemCtrl : MonoBehaviour
{
    public int itemIndex;
    public int itemCount;
    public float itemWeight;
    BaseItem item = null;
    Rigidbody _rigid;
    SphereCollider _colider;

    public float power = 2.5f;
    public float turnSpeed = 30.0f;

    public bool isRootAble = false;
    bool isShoot;
    bool isCall;
    bool isStop;

    private void OnCollisionStay(Collision collision)
    {
        if (isShoot)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _rigid.isKinematic = true;
                _colider.enabled = false;
                if (!isCall)
                {
                    StartCoroutine(TurnObject());                    
                }
            }
        }
    }

    void Init()
    {
        _rigid = GetComponent<Rigidbody>();
        _colider = GetComponent<SphereCollider>();
        _rigid.isKinematic = true;
        isShoot = false;
        isCall = false;
        isStop = false;
        isRootAble = false;

        if (item == null)
            item = InventoryManager._inst.GetItemData(itemIndex);
    }

    Vector3 GetRandomPoint()
    {
        Vector3 pos = Random.onUnitSphere;
        pos.y = 1;
        pos = pos.normalized;
        return pos;
    }

    public void Spawn(int cnt = 1)
    {
        Init();
        itemCount = cnt;
        itemWeight = item.Weight * itemCount;
        Vector3 dir = GetRandomPoint();        
        _rigid.isKinematic = false;
        _rigid.AddForce(dir * power, ForceMode.Impulse);
        isShoot = true;
    }

    IEnumerator TurnObject()
    {
        yield return new WaitForSeconds(1.0f);
        isRootAble = true;
        while (!isStop)
        {
            transform.Rotate(turnSpeed * Vector3.up * Time.deltaTime);
            yield return null;
        }
    }


    public bool Root()
    {
        if (item == null)
            item = InventoryManager._inst.GetItemData(itemIndex);


        if (InventoryManager._inst.CheckSlot(item, itemCount))
        {            
            InventoryManager._inst.AddInvenItem(item, itemCount);
            Despawn();
            return true;
        }
        else
        {
            Debug.Log("¿À·ù");
        }

        return false;
    }
      
    void Despawn()
    {
        gameObject.DestroyAPS();
        _rigid.isKinematic = false;
        _colider.enabled = true;
        isStop = false;
        isCall = false;
        isShoot = false;
        isRootAble = false;
    }
}
