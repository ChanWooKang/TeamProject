using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    #region [ 전역변수 ]
    public float moveSpeed;
    float h, v;
    GameObject nearObject = null;
    #endregion [ 전역변수 ]

    #region [ Property ]
    TalkManager talkManager { get { return TalkManager._talk; } }
    #endregion [ Property ]

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(nearObject != null)
            {
                if(nearObject.TryGetComponent(out ObjectData objData))
                {
                    if(objData.isNPC)
                    {
                        talkManager.ShowText(nearObject, objData.objID, objData.name);
                    }
                    else
                    {
                        if(nearObject.TryGetComponent(out BaseItem item))
                        {                            
                            if (item.Root())
                            {
                                nearObject = null;                                
                            }
                                
                        }
                    }
                    
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryManager._inst.invenUI.TryOpenInventory();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        transform.position += moveSpeed * Time.deltaTime * new Vector3(h, 0, v);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            nearObject = null;
            if (talkManager.talkUI.mainObject.activeSelf)
            {
                //대화 UI가 켜져있을경우 끄기
                talkManager.ResetData();
            }
        }
    }
}
