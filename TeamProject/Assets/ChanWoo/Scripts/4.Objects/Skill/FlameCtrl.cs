using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameCtrl : BaseSkill
{
    [SerializeField] ParticleSystem _particle;
    [SerializeField] Collider _collider;

    bool _isInit = false;

    Transform _parent = null;
    Transform _target = null;
    Vector3 direction;
    bool isOn = false;

    private void FixedUpdate()
    {
        if (isOn && direction != Vector3.zero && _target != null)
        {
            transform.position = _parent.position;
            direction = GetDirection(_parent, _target);
            SetRotaion(direction);
        }
    }

    public void Init()
    {
        SetInformation();
        _isInit = true;        
    }

    void SetRotaion(Vector3 dir)
    {
        direction = dir;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void OnFlameEvent(Transform firePos,Transform target,float damage)
    {
        if (_isInit == false)
            Init();
        _parent = firePos;
        _target = target;
        direction = GetDirection(_parent,_target);
        isOn = true;
        Damage = damage;
        PlayParticle();
    }

    Vector3 GetDirection(Transform start, Transform end)
    {
        Vector3 dir = (end.position - start.position);
        dir = dir.normalized;
        return dir;
    }

    public void OffFlameEvent()
    {
        isOn = false;
        _parent = null;
        _target = null;
        Damage = 0;
        direction = Vector3.zero;
        StopParticle();
    }

    void PlayParticle()
    {
        StopParticle();

        _particle.Play(true);
        _collider.enabled = true;
    }

    void StopParticle()
    {
        if (_particle.isPlaying)
            _particle.Stop(true);
        _collider.enabled = false;
    }
}
