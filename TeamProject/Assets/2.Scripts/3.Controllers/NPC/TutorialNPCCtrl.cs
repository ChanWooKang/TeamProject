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
 
    public override void Talking()
    {
        if (!isInit)
            Init();

        NPCAnimationParams talkState = NPCAnimationParams.Talking;
        if(!GetAnimationBool(talkState))
            SetAnimation(talkState, true);
    }

    public override void TalkEnd()
    {
        SetAnimation(NPCAnimationParams.Talking, false);
        TalkManager._inst.TalkingEnd();
    }
    public override void ActiveAction()
    {
        if (!isInit)
            Init();

        if (target != null) 
            LookTarget(target);
        
        switch (_state)
        {
            case NPCStates.Init:
                if (GiveItem())
                {
                    _state = NPCStates.TalkEnd;
                    plusIndex = (int)_state;
                    SetObjectDataIndex();
                    TalkEnd();                    
                }
                else
                {
                    _state = NPCStates.NotAble;
                    plusIndex = (int)_state;
                    SetObjectDataIndex();
                    TalkManager._inst.isTalking = false;
                    TalkManager._inst.ShowText(gameObject, objData.index);
                }
                break;
            case NPCStates.NotAble:
                _state = NPCStates.Init;
                plusIndex = (int)_state;
                SetObjectDataIndex();
                TalkEnd();
                break;

            case NPCStates.TalkEnd:
                TalkEnd();
                break;              
        }

        TalkEnd();
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isInit)
                Init();

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
