using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class TutorialNPC : BaseNPC
{
    //Ʃ�丮�� ������ �ֱ� �� �ְ� ����
    public SOItem testItem;
    enum State
    {
        Before  = 0,
        Finish  = 1,
        Heavy   = 2
    }

    State NPCState = State.Before;

    //NPC �ൿ �޼ҵ�
    public override void ActiveAction()
    {
        switch (NPCState)
        {
            case State.Before:
                if(InventoryManager._inst.CheckSlot(testItem) == false)
                {
                    //���� 
                    Debug.Log("������ ���� ���� �߽��ϴ�.");
                    InventoryManager._inst.AddInvenItem(testItem);
                    NPCState = State.Finish;
                    objData.objID += (int)NPCState;
                    TalkManager._talk.talkUI.SetOnOff(false);
                }
                else
                {
                    //�Ұ�
                    NPCState = State.Heavy;
                    objData.objID += (int)NPCState;
                    TalkManager._talk.ShowText(this.gameObject, objData.objID, objData.name);
                }
                break;
            case State.Heavy:
                NPCState = State.Before;
                objData.objID += (int)NPCState;
                TalkManager._talk.talkUI.SetOnOff(false);
                break;
            case State.Finish:
                TalkManager._talk.talkUI.SetOnOff(false);
                break;
        }

        
        
    }
}
