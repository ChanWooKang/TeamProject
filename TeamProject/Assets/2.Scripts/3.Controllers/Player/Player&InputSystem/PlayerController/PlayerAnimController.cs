using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerAnimController : MonoBehaviour
{
    Animator _animator;
   
    //Animation IDs
    int _animIDSpeed;
    int _animIDGrounded;
    int _animIDJump;
    int _animIDFreeFall;
    int _animIDMotionSpeed;
    int _animIDAttack;
        

    public void Init(Animator animator)
    {
        _animator = animator;
        AssignAnimationIDs();
        

    }

    void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDAttack = Animator.StringToHash("Attack");
    }


    public void SetAnimations(ePlayerAnimParams parameter, bool isOn)
    {
        switch (parameter)
        {
            case ePlayerAnimParams.Ground:
                if (_animator.GetBool(_animIDGrounded) != isOn)
                    _animator.SetBool(_animIDGrounded, isOn);
                break;
            case ePlayerAnimParams.Jump:
                if (_animator.GetBool(_animIDJump) != isOn)
                    _animator.SetBool(_animIDJump, isOn);
                break;
            case ePlayerAnimParams.FreeFall:
                if (_animator.GetBool(_animIDFreeFall) != isOn)
                    _animator.SetBool(_animIDFreeFall, isOn);
                break;
            case ePlayerAnimParams.Attack:
                if (isOn)
                    _animator.SetLayerWeight(1, 1);

                if (_animator.GetBool(_animIDAttack) != isOn)
                    _animator.SetBool(_animIDAttack, isOn);
                break;
        }
    }

    public void SetAnimations(ePlayerAnimParams parameter, float value)
    {
        switch (parameter)
        {
            case ePlayerAnimParams.Speed:
                if(_animator.GetFloat(_animIDSpeed) != value)
                    _animator.SetFloat(_animIDSpeed, value);
                break;
            case ePlayerAnimParams.MotionSpeed:
                if (_animator.GetFloat(_animIDMotionSpeed) != value)
                    _animator.SetFloat(_animIDMotionSpeed, value);
                break;            
        }
    }

    public void SetAnimations(ePlayerAnimParams parameter)
    {
        
    }

    public void OnAttackEnd()
    {
       
    }
}
