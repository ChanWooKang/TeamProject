using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEffectCtrl : MonoBehaviour
{
    
    public int currIndex;
    public List<ParticleSystem> particles;
    Dictionary<int, ParticleSystem> _particleDatas;


    public void Init()
    {
         currIndex = 0;
        _particleDatas = new Dictionary<int, ParticleSystem>();
        for(int i = 0; i < particles.Count; i++)
        {
            _particleDatas.Add(i + 1, particles[i]);
            particles[i].gameObject.SetActive(false);
        }
    }

    public void ChangeIndex(int newIndex)
    {
        StopParticle(currIndex);
        PlayParticle(newIndex);
        currIndex = newIndex;               
    }


    void PlayParticle(int index)
    {
        if(index > 0)
        {
            if (_particleDatas.TryGetValue(index, out ParticleSystem ps))
            {
                ps.gameObject.SetActive(true);
                ps.Play(true);
            }
        }
        
    }

    void StopParticle(int index)
    {
        if (index > 0)
        {
            if (_particleDatas.TryGetValue(index, out ParticleSystem ps))
            {
                ps.Stop(true);
                ps.gameObject.SetActive(false);
            }
        }
    }
}
