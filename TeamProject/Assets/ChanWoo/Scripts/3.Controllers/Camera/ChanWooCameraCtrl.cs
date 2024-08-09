using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanWooCameraCtrl : MonoBehaviour
{
    public Transform target;
    public Vector3 offSetPos;

    void LateUpdate()
    {
        transform.position = target.position + offSetPos;
    }
}
