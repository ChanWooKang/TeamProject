using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class BaseNPC : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    public ObjectData objData;
    public eNPCType npcType;

    //NPC 행동 메소드
    public abstract void ActiveAction();

    protected int GetAnimationID<T>(T data)
    {
        int animID = Animator.StringToHash(Utilitys.ConvertEnum(data));
        return animID;
    }

    protected void SetAnimation<T>(T data)
    {
        int ID = GetAnimationID(data);
        _animator.SetTrigger(ID);
    }

    protected void SetAnimation<T>(T data, bool isOn)
    {
        int ID = GetAnimationID(data);
        _animator.SetBool(ID, isOn);
    }
}
