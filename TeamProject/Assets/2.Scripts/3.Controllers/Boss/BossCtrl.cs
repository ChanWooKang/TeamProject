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


    

    //보스는 플레이어만 인식?
    public void SetTarget()
    {
        if(GameManagerEx._inst.playerManager != null)
        {
            //Target 설정
        }
    }
}
