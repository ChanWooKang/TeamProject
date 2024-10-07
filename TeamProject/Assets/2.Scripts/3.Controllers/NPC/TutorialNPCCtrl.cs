using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNPCCtrl : BaseNPC
{
    enum NPCStates
    {
        Init     = 0,
        TalkEnd,
        NotAble
    }

    enum NPCAnimationParams
    {
        Recognize,
        Talking
    }



    public override void ActiveAction()
    {
        
    }


}
