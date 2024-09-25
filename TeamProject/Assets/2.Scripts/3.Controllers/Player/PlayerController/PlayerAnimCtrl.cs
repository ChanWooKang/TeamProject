using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerAnimCtrl : MonoBehaviour
{
    PlayerCtrl _manager;
    Animator _animator;

    public void Init(PlayerCtrl manager)
    {
        _manager = manager;
        _animator = GetComponent<Animator>();
    }

    int GetAnimationID(ePlayerAnimParams parmas)
    {
        int animID = Animator.StringToHash(Utilitys.ConvertEnum(parmas));
        return animID;
    }

    //Trigger
    public void SetAnimation(ePlayerAnimParams parmas)
    {
        int ID = GetAnimationID(parmas);
        _animator.SetTrigger(ID);
    }

    //Bool
    public void SetAnimation(ePlayerAnimParams parmas, bool isOn)
    {
        int ID = GetAnimationID(parmas);
        _animator.SetBool(ID, isOn);
    }

    //Float
    public void SetAnimation(ePlayerAnimParams parmas, float value)
    {
        int ID = GetAnimationID(parmas);
        _animator.SetFloat(ID, value);
    }

    //Integer
    public void SetAnimation(ePlayerAnimParams parmas, int value)
    {
        int ID = GetAnimationID(parmas);
        _animator.SetInteger(ID, value);
    }

    public void SetAniLayerWeight(int layerIndex, float value)
    {
        _animator.SetLayerWeight(layerIndex, value);
    }

    //Animation Trigger Event
    public void OnAttack()
    {
        _manager._equip.AttackAction();

    }

    public void OnAttackEnd()
    {
        SetAnimation(ePlayerAnimParams.AttackEnd, true);
    }

    public void OnEquip()
    {
        _manager._equip.EquipEvent();
    }

    public void OnDisarm()
    {
        _manager._equip.DisarmEvent();

    }

    public void OnActiveObject(int value)
    {
        _manager._equip.SetOnOffWeapon(value > 0);
    }

    public void ChargeStart()
    {
        _manager._equip.ChargeStart();
    }

    public void ChargeEnd()
    {
        _manager._equip.ChargeEnd();
    }

    public void Reload()
    {
        _manager._equip.Reload();
    }

    public void ReadyToThrow()
    {
        SetAnimation(ePlayerAnimParams.AttackEnd, false);
        _manager._equip.GeneratePetBall();
    }

    public void ThrowEvent()
    {
        _manager._equip.ThrowBall();
    }

    public void ThrowEnd()
    {
        _manager._equip.ThrowEnd();
        SetAnimation(ePlayerAnimParams.AttackEnd, true);
    }
}
