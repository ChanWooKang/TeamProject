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

    float fixedYAngle = 0;

    private void FixedUpdate()
    {
        if (isOn && direction != Vector3.zero && _target != null)
        {
            transform.position = _parent.position;
            direction = GetDirection();
            //direction = _parent.right * -1;
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
        fixedYAngle = GetyValue(_parent.position, _target.position);
        direction = GetDirection();        
        //direction = _parent.right * -1;
        isOn = true;
        Damage = damage;
        PlayParticle();
    }

    Vector3 GetDirection()
    {        
        //이놈이 대가리따라 움직이니 
        Vector3 vec = _parent.right * -1;        
        return new Vector3(vec.x, fixedYAngle, vec.z);
    }

    float GetyValue(Vector3 start, Vector3 end)
    {
        Vector3 xVec = end - start;
        xVec = xVec.normalized;
        return xVec.y;
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
