using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class TutorialNPC : BaseNPC
{
    //튜토리얼 아이템 주기 전 주고 나서
    public int testItemIndex;
    public BaseItem testItem = null;
    enum State
    {
        Before  = 0,
        Finish  = 1,
        Heavy   = 2
    }

    State NPCState = State.Before;

    //NPC 행동 메소드
    public override void ActiveAction()
    {
        if (testItem == null)
            testItem = InventoryManager._inst.GetItemData(testItemIndex);

        switch (NPCState)
        {
            case State.Before:
                if (InventoryManager._inst.CheckSlot(testItem) == false)
                {
                    //가능 
                    Debug.Log("아이템 보상 지급 했습니다.");
                    InventoryManager._inst.AddInvenItem(testItem);
                    NPCState = State.Finish;
                    objData.objID += (int)NPCState;
                    TalkManager._inst.talkUI.SetOnOff(false);
                }
                else
                {
                    //불가
                    NPCState = State.Heavy;
                    objData.objID += (int)NPCState;
                    TalkManager._inst.ShowText(this.gameObject, objData.objID, objData.name);
                }
                break;
            case State.Heavy:
                NPCState = State.Before;
                objData.objID += (int)NPCState;
                TalkManager._inst.talkUI.SetOnOff(false);
                break;
            case State.Finish:
                TalkManager._inst.talkUI.SetOnOff(false);
                break;
        }



    }

    public override void TalkStart()
    {
        
    }

    public override void TalkEnd()
    {
        
    }
}
