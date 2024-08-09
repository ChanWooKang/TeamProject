using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChanWooTutorialNPC : ChanWooBaseNPC
{
    //Test 제공 아이템
    public ChanWooSOItem tutorialItem;
    ChanWooObjectData myObjData;
    bool hasGiveItem = false;
    public ChanWooObjectData ObjData { get { if (myObjData == null) myObjData = GetComponent<ChanWooObjectData>(); return myObjData; } }
    public override void ActiveAction()
    {
        //보상을 준적이 없다면 실행
        if (hasGiveItem == false)
        {
            //보상 확인
            Debug.Log("보상 진행");
            //보상 사운드 재생

            hasGiveItem = true;
            ChanWooTalkManager.Inst.talkUI.SetOnOff(false);
        }
        else
        {
            ChanWooTalkManager.Inst.OnTalk(ObjData.objID + 1, ObjData.krName);
            ChanWooTalkManager.Inst.isTalkEnd = true;
        }
    }
}
