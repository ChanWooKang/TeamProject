using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
using static UnityEditor.Progress;

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

    public bool GetAnimation(ePlayerAnimParams parmas)
    {
        int ID = GetAnimationID(parmas);        
        return _animator.GetBool(ID);
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

    public void OnRootEvent()
    {
        if(_manager.RecognizeObject != null)
        {
            if(_manager.RecognizeObject.TryGetComponent(out ItemCtrl item))
            {
                if (item.isRootAble)
                {
                    if (item.Root())
                        _manager.SetRecognizeObject();
                }
            }
        }        
    }

    public void FixAnimation(bool isOn)
    {
        _manager._equip.ReadyToAnimAction(isOn);
        _manager._equip.HammerModel.SetActive(isOn);
        SetAnimation(ePlayerAnimParams.Fix, isOn);
    }

    public void OnEquip()
    {        
        _manager._equip.EquipEvent();
    }

    public void OnDisarm()
    {
        _manager._equip.DisarmEvent();        
    }
    
    public void OnActiveEquipObject()
    {        
        _manager._equip.EquipActiveWeapon();
    }

    public void OnActiveDisarmObject()
    {        
        _manager._equip.DisarmActiveWeapon();
    }

    public void ChargeStart()
    {
        _manager._equip.ChargeStart();
    }

    public void ChargeEnd()
    {
        _manager._equip.ChargeEnd();
    }

    public void AnimationStart()
    {
        SetAnimation(ePlayerAnimParams.AcivateAnimation, true);
    }

    public void AnimationEnd()
    {        
        SetAnimation(ePlayerAnimParams.AcivateAnimation, false);
    }

    public void Reload()
    {        
        _manager._equip.Reload();
        AnimationEnd();
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
        AnimationEnd();
    }
}
