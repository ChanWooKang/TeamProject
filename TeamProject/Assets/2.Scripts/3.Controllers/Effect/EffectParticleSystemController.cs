using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParticleSystemController : MonoBehaviour
{
    ParticleSystem m_particleSystem;
    [Header("Add Duration If Looping")]
    [SerializeField] float m_duration;
    float m_timer;
    private void Awake()
    {
        InitEffect();
    }
    private void Update()
    {
        DestroyAps();
    }
    void InitEffect()
    {      
        m_particleSystem = GetComponent<ParticleSystem>();
    }

    void DestroyAps()
    {
        if (m_particleSystem == null)
            return;
        if(m_particleSystem.main.loop)
        {
            m_timer += Time.deltaTime;
            {
                if (m_timer >= m_duration)
                {
                    m_timer = 0f;
                    PoolingManager.DestroyAPS(gameObject);
                }
            }
        }
        else
        {
            if (!m_particleSystem.IsAlive())
                PoolingManager.DestroyAPS(gameObject);
        }      
    }
}
