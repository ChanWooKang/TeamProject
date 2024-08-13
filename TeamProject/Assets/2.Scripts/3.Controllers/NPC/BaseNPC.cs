using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class BaseNPC : MonoBehaviour
{
    public ObjectData objData;
    public eNPCType npcType;

    //NPC 행동 메소드
    public abstract void ActiveAction();
}
