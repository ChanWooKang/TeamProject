using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChanWooDefineDatas;

public abstract class ChanWooBaseNPC : MonoBehaviour
{    
    public eNPCType npcType;

    //NPC 대화 종료 후 행동 메소드
    public abstract void ActiveAction();
}


