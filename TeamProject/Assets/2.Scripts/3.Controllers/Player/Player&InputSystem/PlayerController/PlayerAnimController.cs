using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerAnimController : MonoBehaviour
{
    PlayerManager manager;
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
    int _animIDFire;
    int _animIDAttackEnd;
    int _animIDEquip;
    int _animIDDisarm;
    int _animIDWeaponType;


    public bool isCharging = false;    

    public void Init(PlayerManager _manager ,Animator animator)
    {
        manager = _manager;
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
        _animIDFire = Animator.StringToHash("Fire");
        _animIDAttackEnd = Animator.StringToHash("AttackEnd");
        _animIDAim = Animator.StringToHash("Aim");
        _animIDEquip = Animator.StringToHash("Equip");
        _animIDDisarm = Animator.StringToHash("Disarm");
        _animIDWeaponType = Animator.StringToHash("WeaponType");
    }

    #region [ Animation Parameter Setting ]
    // Bool
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
            case ePlayerAnimParams.AttackEnd:
                _animator.SetBool(_animIDAttackEnd, isOn);
                break;
        }
    }

    // Float
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

    // Trigger
    public void SetAnimations(ePlayerAnimParams parameter)
    {
        switch (parameter)
        {
            case ePlayerAnimParams.Equip:
                _animator.SetTrigger(_animIDEquip);
                break;
            case ePlayerAnimParams.Disarm:
                _animator.SetTrigger(_animIDDisarm);
                break;
            case ePlayerAnimParams.Fire:
                _animator.SetTrigger(_animIDFire);
                break;
        }
    }

    public void SetAnimations(ePlayerAnimParams parameter,int value)
    {
        switch (parameter)
        {
            case ePlayerAnimParams.WeaponType:
                _animator.SetInteger(_animIDWeaponType, value);
                break;
        }
    }

    #endregion [ Animation Parameter Setting ]

    public void SetAnimationLayerWeight(WeaponType type, float weight)
    {
        int layer = (int)type;                    
        _animator.SetLayerWeight(layer, weight);                        
    }

    public void EquipEvent() 
    {
        manager.EquipCtrl.EquipWeapon();
    }

    public void DisarmEvent()
    {        
        manager.EquipCtrl.DisarmWeapon();
    }

    public void OnAttackEnd()
    {
        SetAnimations(ePlayerAnimParams.AttackEnd, true);
    }



    #region [ Bow Animation ]

    public void DrawArrow()
    {
        manager.EquipCtrl.GetBow().DrawArrow();        
    }

    public void FireEvent()
    {
        Debug.Log("½¹");
        isCharging = false;
        manager.EquipCtrl.GetBow().Fire();
    }

    public void OnChargeEvent()
    {
        SetAnimations(ePlayerAnimParams.AttackEnd, false);
        manager.EquipCtrl.GetBow().AimStart();
        isCharging = true;
    }

    public void OnChargeEnd()
    {        
        SetAnimations(ePlayerAnimParams.AttackEnd, false);
    }

    #endregion [ Bow Animation ]
}
