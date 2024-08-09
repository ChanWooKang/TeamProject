using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChanWooDefineDatas;

public class ChanWooPlayerCtrl : MonoBehaviour
{    
    ChanWooTalkManager talkManager { get { return ChanWooTalkManager.Inst; } }

    public float MoveSpeed;
    float h, v;

    GameObject nearObject;

    void Start()
    {
        nearObject = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(nearObject != null)
            {
                if(nearObject.TryGetComponent(out ChanWooObjectData objData))
                {
                    talkManager.ShowText(nearObject, objData.objID, objData.krName, objData.interactType);
                }
                
            }
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

        transform.position += MoveSpeed * Time.deltaTime * new Vector3(h, 0, v);
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
                //대화 UI가 켜져있을경우 끄고 talkCounter = 0;
                talkManager.talkUI.SetOnOff(false);
                talkManager.TalkCounter = 0;
            }
        }
    }
}
