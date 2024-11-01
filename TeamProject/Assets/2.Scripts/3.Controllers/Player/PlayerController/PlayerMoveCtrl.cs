using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DefineDatas;

public class PlayerMoveCtrl : MonoBehaviour
{
    [Header("Players")]
    [Tooltip("Accel & Decel Rate")]
    public float SpeedChangeRate = 10.0f;
    [Tooltip("Smooth Rotate SpeedTime")]
    [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.12f;
    [Tooltip("Jump Height")]
    public float JumpHeight = 1.2f;
    [Tooltip("Gravity")]
    public float Gravity = -15.0f;
    [Tooltip("Jump Cooltime")]
    public float JumpTimeOut = 0.5f;
    [Tooltip("Fall State Required Time")]
    public float FallTimeOut = 0.15f;
    [Header("Player Grounded")]
    [Tooltip("Player Grounded Check")]
    public bool Grounded = true;
    [Tooltip("Userful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("Radius of Grounded Check")]
    public float GroundedRadius = 0.28f;
    [Tooltip("Layers Ground")]
    public LayerMask GroundLayers;

    //Player
    float _speed;
    float _animationBlend;
    float _targetRotation = 0.0f;
    float _rotationVelocity;
    float _verticalVelocity;
    float _terminalVelocity = 53.0f;
    
    //Stamina 사용 가능 여부
    bool _canUseStamina = true;
    bool _isRegenStamina = false;
    float _regenDelay = 1.0f;
    float _staminaDrainRate = 10.0f;
    float _staminaRegenRate = 10.0f;
    float _jumpUseStamina = 10.0f;
    Coroutine _regenCoroutine = null;

    //Timeout deltaTime
    float _jumpTimeOutDelta;
    float _fallTimeOutDelta;

    //Player Components
    PlayerCtrl _manager;
    PlayerAssetsInputs _input;
    CharacterController _control;

    //Other Components
    GameObject _mainCam;    

    public void Init(PlayerCtrl manager, PlayerAssetsInputs input, CharacterController control)
    {
        _manager = manager;
        _input = input;
        _control = control;

        //Other
        _mainCam = Camera.main.gameObject;
    }

    public void OnUpdate()
    {
        JumpInGravity();
        GroundedCheck();
        if (GameManagerEx._inst.CheckIsMoveAble() && _manager.StopMove == false)
        {            
            
            
            CheckStaminaUse();
            MoveAcion();            
            LookAtAim();
        }
        else
        {
            if (Grounded)
                StopMove();
            else
                MoveAcion();
        }
    }

    //플레이어가 위치가 공중 혹은 지상인지 확인
    void GroundedCheck()
    {
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePos, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        //애니메이션 Ground Bool Set
        _manager._anim.SetAnimation(ePlayerAnimParams.Ground,Grounded);
    }

    //점프 작업 및 중력 설계
    void JumpInGravity()
    {
        if (Grounded)
        {
            _fallTimeOutDelta = FallTimeOut;

            //애니메이션 Jump Bool Set , FreeFall Bool Set
            _manager._anim.SetAnimation(ePlayerAnimParams.Jump, false);
            _manager._anim.SetAnimation(ePlayerAnimParams.FreeFall, false);

            if (_verticalVelocity < 0)
                _verticalVelocity = -2.0f;

            if (_input.jump && _jumpTimeOutDelta <= 0.0f && _manager._stat.CheckUseStamina(_jumpUseStamina)&& GameManagerEx._inst.CheckIsMoveAble())
            {
                _manager._stat.CanUseStamina(_jumpUseStamina);
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);

                //애니메이션 Jump Bool Set
                _manager._anim.SetAnimation(ePlayerAnimParams.Jump, true);
                _input.jump = false;
            }
            else
            {
                _input.jump = false;
            }

            if (_jumpTimeOutDelta >= 0.0f)
                _jumpTimeOutDelta -= Time.deltaTime;
        }
        else
        {
            _jumpTimeOutDelta = JumpTimeOut;

            if (_fallTimeOutDelta >= 0.0f)
                _fallTimeOutDelta -= Time.deltaTime;
            else                            
                _manager._anim.SetAnimation(ePlayerAnimParams.FreeFall, true);

            _input.jump = false;
        }

        if (_verticalVelocity < _terminalVelocity)
            _verticalVelocity += Gravity * Time.deltaTime;
    }

    //움직임 제어
    public void StopMove()
    {
        if (_speed >= 0.0f)
        {
            _speed = 0.0f;

            //애니메이션 Speed, MotionSpeed Float Set
            _manager._anim.SetAnimation(ePlayerAnimParams.Speed, _speed);
            _manager._anim.SetAnimation(ePlayerAnimParams.MotionSpeed,_speed);
        }
    }

    float GetSpeed()
    {
        float targetSpeed;

        if (_input.aim) targetSpeed = _manager._stat.MoveSpeed;
        else targetSpeed = _input.sprint ? _manager._stat.RunSpeed : _manager._stat.MoveSpeed;
        if (_input.move == Vector2.zero) targetSpeed = 0;
        else if (!_canUseStamina) targetSpeed = _manager._stat.MoveSpeed;
        
        return targetSpeed;
    }    

    //움직임 작업
    void MoveAcion()
    {
        float targetSpeed = GetSpeed();
        
        float currHorzonSpeed = new Vector3(_control.velocity.x, 0.0f, _control.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1.0f;
        if (currHorzonSpeed < targetSpeed - speedOffset || currHorzonSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currHorzonSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
            _speed = targetSpeed;

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0.0f;

        Vector3 inputDir = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        if (inputDir != Vector3.zero)
        {
            _targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg
                                + _mainCam.transform.eulerAngles.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDir = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        _control.Move(targetDir.normalized * (_speed * Time.deltaTime)
                        + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);        
        //애니메이션 xDir, yDir, Speed, MotionSpeed Float
        _manager._anim.SetAnimation(ePlayerAnimParams.xDir, _input.move.x);
        _manager._anim.SetAnimation(ePlayerAnimParams.yDir, _input.move.y);
        _manager._anim.SetAnimation(ePlayerAnimParams.Speed, _animationBlend);
        _manager._anim.SetAnimation(ePlayerAnimParams.MotionSpeed, inputMagnitude);
    }

    //Aim 상태 시 플레이어 회전값 고정
    void LookAtAim()
    {
        if (_input.aim || _input.throws)
        {
            Transform target = _mainCam.transform;
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.Euler(0.0f, target.eulerAngles.y, 0.0f), Time.deltaTime * 100.0f);
        }
    }

    

    void CheckStaminaUse()
    {
        if (_input.move != Vector2.zero)
        {
            if (_input.sprint && _input.aim == false)
            {
                _canUseStamina = _manager._stat.UseStaminaByTime(_staminaDrainRate);
                _isRegenStamina = false;
                if (_regenCoroutine != null)
                    StopCoroutine(_regenCoroutine);
            }
            else
            {
                if (_input.fire)
                {
                    _isRegenStamina = false;
                    if (_regenCoroutine != null)
                        StopCoroutine(_regenCoroutine);
                }
                else
                {
                    if (!_isRegenStamina && _manager._stat.Stamina < _manager._stat.MaxStamina)
                    {
                        _regenCoroutine = StartCoroutine(RegenStaminaEvent());
                    }
                }                
            }            
        }
        else
        {
            if (!_isRegenStamina && _manager._stat.Stamina < _manager._stat.MaxStamina)
            {
                _regenCoroutine = StartCoroutine(RegenStaminaEvent());
            }
        }

    }

    IEnumerator RegenStaminaEvent()
    {
        _isRegenStamina = true;
        yield return new WaitForSeconds(_regenDelay);

        while(_manager._stat.Stamina < _manager._stat.MaxStamina)
        {
            if(_input.move != Vector2.zero)
                _manager._stat.RegenStaminaByTime(_staminaRegenRate);
            else
                _manager._stat.RegenStaminaByTime(_staminaRegenRate * 3);
            yield return null;
        }
        _isRegenStamina = false;
    }
}
