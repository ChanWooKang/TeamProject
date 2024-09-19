using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class BaseNPC : MonoBehaviour
{
    public ObjectData objData;
    public eNPCType npcType;

    //NPC �ൿ �޼ҵ�
    public abstract void ActiveAction();
}
