using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectParticleSystemController : MonoBehaviour
{
    ParticleSystem m_particleSystem;
    Transform m_targetPos;
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
    public void SetTarget(Transform target)
    {
        m_targetPos = target;
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
            if(m_targetPos != null)
            {
                transform.position = m_targetPos.position;
            }
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
