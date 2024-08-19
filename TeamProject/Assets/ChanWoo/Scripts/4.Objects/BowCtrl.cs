using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowCtrl : MonoBehaviour
{
    Animator _animator;
    public PlayerAssetsInputs _input;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Fire();
    }

    void Fire()
    {
        if (_input.fire)
        {
            _animator.SetBool(Animator.StringToHash("Fire"), true);
        }
        else
        {
            _animator.SetBool(Animator.StringToHash("Fire"), false);
        }
    }

}
