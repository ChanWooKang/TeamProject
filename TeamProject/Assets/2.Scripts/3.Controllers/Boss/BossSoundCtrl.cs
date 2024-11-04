using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundCtrl : MonoBehaviour
{
    BossCtrl _manager;
    AudioSource _source;
    
    public void Init(BossCtrl manager)
    {
        _manager = manager;
        _source = GetComponent<AudioSource>();
    }

    void PlaySound(string name, Vector3 pos)
    {
        SoundManager._inst.PlaySfx(name);
        //SoundManager._inst.PlaySfxAtPoint(name, pos);
    }

    public void GetHitSound()
    {
        var source = "GetHit";
        PlaySound(source, transform.position);
    }

    private void OnFireBallSound(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
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

    private void OnGrowlSound(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var source = "Boss_Growl";
            PlaySound(source, transform.position);
        }
    }

    private void OnFlameStartSound(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            var src = "Boss_Flame";
            SoundManager._inst.PlayLoopSfx(src, _source);            
        }
    }

    private void OnFlameEndSound(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            SoundManager._inst.StopAudio(_source);
        }
    }
}
