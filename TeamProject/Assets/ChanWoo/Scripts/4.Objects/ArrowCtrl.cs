using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCtrl : MonoBehaviour
{
    Rigidbody _rigid;

    float _power = 5.0f;

    public void Init()
    {
        _rigid = GetComponent<Rigidbody>();
        _rigid.isKinematic = true;
    }

    public void FireArrow(Transform player)
    {
        _rigid.isKinematic = false;
        _rigid.AddForce(Camera.main.transform.forward * _power, ForceMode.Impulse);
    }
}
