using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChanWooDefineDatas;

public abstract class ChanWooBaseNPC : MonoBehaviour
{    
    public eNPCType npcType;

    //NPC ��ȭ ���� �� �ൿ �޼ҵ�
    public abstract void ActiveAction();
}


