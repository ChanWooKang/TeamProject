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
    Dictionary<PlayerSoundState, string> dictSound;

    public void Init(PlayerCtrl manager, CharacterController control)
    {
        _manager = manager;
        _control = control;

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
            var landStep = dictSound[PlayerSoundState.Land];
            SoundManager._inst.PlaySfxAtPoint(landStep, transform.position);
        }
    }
}
