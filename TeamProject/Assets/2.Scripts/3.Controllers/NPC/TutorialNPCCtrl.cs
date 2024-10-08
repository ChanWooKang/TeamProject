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

    Transform target;

    bool isInside = false;
    NPCStates _state = NPCStates.Init;

    public override void TalkStart()
    {
        SetAnimation(NPCAnimationParams.Talking, true);
    }

    public override void TalkEnd()
    {
        SetAnimation(NPCAnimationParams.Talking, false);
    }
    public override void ActiveAction()
    {        
        if(target != null) 
            LookTarget(target);        

        switch (_state)
        {
            case NPCStates.Init:
                break;
            case NPCStates.TalkEnd:
                break;
            case NPCStates.NotAble:
                break;  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isInside) 
            {
                isInside = true;
                target = other.transform;
                LookTarget(target);
                SetAnimation(NPCAnimationParams.Recognize);
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            if(isInside)
            {
                isInside = false;
                target = null;
            }
        }
    }

   
}
