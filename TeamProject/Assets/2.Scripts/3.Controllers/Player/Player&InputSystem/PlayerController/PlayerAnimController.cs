using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerAnimController : MonoBehaviour
{
    Animator _animator;

    //Animation IDs
    int _animIDxDir;
    int _animIDyDir;
    int _animIDSpeed;
    int _animIDGrounded;
    int _animIDJump;
    int _animIDFreeFall;
    int _animIDMotionSpeed;
    int _animIDAim;
    int _animIDAttack;
        

    public void Init(Animator animator)
    {
        _animator = animator;
        AssignAnimationIDs();
        

    }

    void AssignAnimationIDs()
    {
        _animIDxDir = Animator.StringToHash("xDir");
        _animIDyDir = Animator.StringToHash("yDir");
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        //_animIDAttack = Animator.StringToHash("Attack");
        _animIDAim = Animator.StringToHash("Aim");
    }


    public void SetAnimations(ePlayerAnimParams parameter, bool isOn)
    {
        switch (parameter)
        {
            case ePlayerAnimParams.Ground:
                    _animator.SetBool(_animIDGrounded, isOn);
                break;
            case ePlayerAnimParams.Jump:                
                    _animator.SetBool(_animIDJump, isOn);
                break;
            case ePlayerAnimParams.FreeFall:
                    _animator.SetBool(_animIDFreeFall, isOn);
                break;
            case ePlayerAnimParams.Aim:
                    _animator.SetBool(_animIDAim, isOn);
                break;
        }
    }

    public void SetAnimations(ePlayerAnimParams parameter, float value)
    {
        switch (parameter)
        {
            case ePlayerAnimParams.Speed:                
                    _animator.SetFloat(_animIDSpeed, value);
                break;
            case ePlayerAnimParams.MotionSpeed:                
                    _animator.SetFloat(_animIDMotionSpeed, value);
                break;
            case ePlayerAnimParams.xDir:
                _animator.SetFloat(_animIDxDir, value);
                break;
            case ePlayerAnimParams.yDir:
                _animator.SetFloat(_animIDyDir, value);
                break;
        }
    }

    public void SetAnimations(ePlayerAnimParams parameter)
    {
        
    }

    public void OnAttackEnd()
    {
        _animator.SetLayerWeight(1, 0);
    }
}
