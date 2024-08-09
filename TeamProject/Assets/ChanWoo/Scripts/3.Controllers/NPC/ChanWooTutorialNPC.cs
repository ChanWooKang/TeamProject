using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChanWooTutorialNPC : ChanWooBaseNPC
{
    //Test ���� ������
    public ChanWooSOItem tutorialItem;
    ChanWooObjectData myObjData;
    bool hasGiveItem = false;
    public ChanWooObjectData ObjData { get { if (myObjData == null) myObjData = GetComponent<ChanWooObjectData>(); return myObjData; } }
    public override void ActiveAction()
    {
        //������ ������ ���ٸ� ����
        if (hasGiveItem == false)
        {
            //���� Ȯ��
            Debug.Log("���� ����");
            //���� ���� ���

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
