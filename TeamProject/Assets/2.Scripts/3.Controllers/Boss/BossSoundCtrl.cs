using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundCtrl : MonoBehaviour
{
    BossCtrl _manager;
    public void Init(BossCtrl manager)
    {
        _manager = manager;
    }

    void PlaySound(string name, Vector3 pos)
    {
        SoundManager._inst.PlaySfxAtPoint(name, pos);
    }

    public void GrowlSound()
    {
        var source = "Boss_Growl";
        PlaySound(source,transform.position);
    }

    public void GetHitSound()
    {
        var source = "GetHit";
        PlaySound(source, transform.position);
    }

    private void OnFireBallSound(AnimationEvent animationEvent)
    {
        if(animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var source = "Boss_FireBall";
            PlaySound(source, transform.position);
        }
    }

    private void OnAttackSound(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var source = "Boss_Bite";
            PlaySound(source, transform.position);
        }
    }
}
