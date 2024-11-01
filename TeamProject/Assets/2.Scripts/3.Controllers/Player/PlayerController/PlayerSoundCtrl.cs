using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerSoundCtrl : MonoBehaviour
{
    [SerializeField]
    List<string> footSteps;    

    PlayerCtrl _manager;
    CharacterController _control;

    public List<PlayerSoundInfo> playerSoundInfos;
    Dictionary<eSoundState, string> dictSound;

    public void Init(PlayerCtrl manager, CharacterController control)
    {
        _manager = manager;
        _control = control;
        dictSound = new Dictionary<eSoundState, string>();
        foreach(var info in playerSoundInfos)
        {
            if (!dictSound.ContainsKey(info.State))
                dictSound.Add(info.State,info.Name);
        }
    }

    void PlaySound(string name, Vector3 pos)
    {
        SoundManager._inst.PlaySfxAtPoint(name, pos);
    }

    public void GetHitSound()
    {
        var gethit = dictSound[eSoundState.GetHit];
        PlaySound(gethit, transform.position);
    }

    #region [Sound By Animation]
    private void OnFootStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (footSteps.Count > 0)
            {
                var index = Random.Range(0, footSteps.Count); ;
                PlaySound(footSteps[index], transform.position);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var landStep = dictSound[eSoundState.Land];
            PlaySound(landStep, transform.position);
        }
    }

    private void OnJumpEvent(AnimationEvent animationEvent)
    {
        if(animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var jump = dictSound[eSoundState.Jump];
            PlaySound(jump, transform.position);
        }
    }

    private void OnRootSound(AnimationEvent animationEvent)
    {
        if(animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var root = dictSound[eSoundState.PickUp];
            PlaySound(root, transform.position);
        }
    }

    private void OnEquipSound(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var root = dictSound[eSoundState.Equip];
            PlaySound(root, transform.position);
        }
    }

    private void OnFireSound(AnimationEvent animationEvent)
    {
        var animName = string.Empty;
        switch (_manager._equip.CurrentWeaponType)
        {
            case WeaponType.None:
                animName = dictSound[eSoundState.AttackUnArmed];
                break;
            case WeaponType.OneHand:
                animName = dictSound[eSoundState.AttackOneHand];
                break;
            case WeaponType.Bow:
                animName = dictSound[eSoundState.AttackBow];
                break;
            case WeaponType.Rifle:
                animName = dictSound[eSoundState.AttackRifle];
                break;
            case WeaponType.Pickaxe:
                animName = dictSound[eSoundState.AttackOneHand];
                break;
        }
        PlaySound(animName, transform.position);
    }
    #endregion [Sound By Animation]

}
