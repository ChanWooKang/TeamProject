using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineDatas;


public class BossCtrl : FSM<BossCtrl>
{
    [Header("Boss Data")]
    public int Index;
    public int MonsterLevel = 1;

    [Header("Components")]
    public MonsterStat Stat;
    public BossMoveCtrl _move;
    public BossAnimCtrl _anim;
    public BossRenderCtrl _render;

    [Header("Transforms")]
    public Transform _model;
    public Transform _captureModel;
    public Transform _hudTransform;

    [Header("Datas")]
    public float delayTime = 2.0f;
    public float attackRange;
    public float dizzyRate = 0.05f;

    public eAttackType _attackType;
    eMonsterState _nowState;
    public List<SkillInfo> _mySkills;
    [HideInInspector] public Transform target;
    [HideInInspector] public Vector3 targetPos;

    public bool isDead;
    public bool isAttack;
    public bool isReturn;
    public bool isBuffed;

    Coroutine DamageCoroutine = null;

    public NavMeshAgent Agent { get { return _move.Agent; } }    

    public eMonsterState State
    {
        get { return _nowState; }
        set
        {
            _nowState = value;
            _anim.ChangeAnimation(_nowState);
        }
    }

    //보스는 플레이어만 인식?
    public void SetTarget()
    {
        if(GameManagerEx._inst.playerManager != null)
        {
            //Target 설정
        }
    }
}
