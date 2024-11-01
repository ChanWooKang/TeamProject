using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundCtrl : MonoBehaviour
{
    [SerializeField]
    List<string> footSteps;

    PlayerCtrl _manager;
    CharacterController _control;


    public void Init(PlayerCtrl manager, CharacterController control)
    {
        _manager = manager;
        _control = control;
    }


    private void OnFootStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (footSteps.Count > 0)
            {
                var index = Random.Range(0, footSteps.Count); ;
                SoundManager._inst.PlaySfxAtPoint(footSteps[index], _control.center);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            SoundManager._inst.PlaySfxAtPoint("Player_Land", transform.position);
        }
    }
}
