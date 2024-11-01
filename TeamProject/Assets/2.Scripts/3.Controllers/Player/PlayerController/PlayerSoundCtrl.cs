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


    private void OnFootStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (footSteps.Count > 0)
            {
                var index = Random.Range(0, footSteps.Count); ;
                SoundManager._inst.PlaySfxAtPoint(footSteps[index], transform.position);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var landStep = dictSound[eSoundState.Land];
            SoundManager._inst.PlaySfxAtPoint(landStep, transform.position);
        }
    }

    private void OnJumpEvent(AnimationEvent animationEvent)
    {
        if(animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var jump = dictSound[eSoundState.Jump];
            SoundManager._inst.PlaySfxAtPoint(jump, transform.position);
        }
    }

    
}
