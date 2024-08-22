using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DefineDatas;

public class PlayerMovementController : MonoBehaviour
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

    //Timeout deltaTime
    float _jumpTimeOutDelta;
    float _fallTimeOutDelta;

    PlayerManager manager;
    CharacterController _controller;
    PlayerAssetsInputs _input;
    GameObject _mainCamera;

    public void Init(PlayerManager main, CharacterController controller, PlayerAssetsInputs input,GameObject mainCam)
    {
        manager = main;
        _controller = controller;
        _input = input;
        _mainCamera = mainCam;
    }

    public void OnUpdate()
    {
        if (GameManagerEx._inst.CheckIsMoveAble())
        {
            JumpAndGravity();
            GroundedCheck();
            Move();
            LookAtCrossHair();
        }
        else
            Stop();
    }

    void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (manager._hasAnimator)
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Ground, Grounded);
    }

    void JumpAndGravity()
    {
        if (Grounded)
        {
            _fallTimeOutDelta = FallTimeOut;

            if (manager._hasAnimator)
            {
                manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Jump, false);
                manager.AnimCtrl.SetAnimations(ePlayerAnimParams.FreeFall, false);
            }

            if (_verticalVelocity < 0.0f)
                _verticalVelocity = -2.0f;

            //JumpAction
            if (_input.jump && _jumpTimeOutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);

                if (manager._hasAnimator)
                    manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Jump, true);

            }

            //Jump timeout
            if (_jumpTimeOutDelta >= 0.0f)
                _jumpTimeOutDelta -= Time.deltaTime;
        }
        else
        {
            _jumpTimeOutDelta = JumpTimeOut;

            if (_fallTimeOutDelta >= 0.0f)
            {
                _fallTimeOutDelta -= Time.deltaTime;
            }
            else
            {
                if (manager._hasAnimator)
                    manager.AnimCtrl.SetAnimations(ePlayerAnimParams.FreeFall, true);
            }

            _input.jump = false;
        }

        if (_verticalVelocity < _terminalVelocity)
            _verticalVelocity += Gravity * Time.deltaTime;
    }

    void Stop()
    {
        if (manager._hasAnimator && _speed != 0)
        {
            _speed = 0;
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Speed, 0);
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.MotionSpeed, 0);
        }
    }

    void Move()
    {
        float targetSpeed = 0;

        if (_input.aim)        
            targetSpeed = manager.Stat.MoveSpeed;        
        else        
            targetSpeed = _input.sprint ? manager.Stat.RunSpeed : manager.Stat.MoveSpeed;
                
        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;            
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0.0f;

        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                            new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        if (manager._hasAnimator)
        {
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.xDir, _input.move.x);
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.yDir, _input.move.y);
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.Speed, _animationBlend);
            manager.AnimCtrl.SetAnimations(ePlayerAnimParams.MotionSpeed, inputMagnitude);
        }
    }

    public void LookAtCrossHair()
    {
        if (_input.aim)
        {
            Transform target = manager.InputCtrl.CinemachineCameraTarget.transform;

            //transform.rotation = Quaternion.Euler(0, target.eulerAngles.y, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.Euler(0,target.eulerAngles.y,0), Time.deltaTime * 100f);
        }
    }
    

}
