using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class BaseNPC : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    public ObjectData objData;
    protected bool isInit = false;
    protected int baseIndex;
    protected int plusIndex;
    public eNPCType npcType;
    [SerializeField] protected List<ItemDatas> giveItems;

    public abstract void Talking();    
    public abstract void TalkEnd();        
    public abstract void ActiveAction();

    protected virtual void Init() 
    {
        baseIndex = objData.index;
        plusIndex = 0;        
        isInit = true;
    }

    protected int GetAnimationID<T>(T data)
    {
        int animID = Animator.StringToHash(Utilitys.ConvertEnum(data));
        return animID;
    }

    protected bool GetAnimationBool<T>(T data)
    {
        int ID = GetAnimationID(data);
        return _animator.GetBool(ID);
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

    protected void LookTarget(Transform target)
    {
        transform.LookAt(target);
    }

    protected bool GiveItem()
    {
        bool isOk = InventoryManager._inst.CheckSlotAndAddItem(giveItems);
        Debug.Log(isOk);
        return isOk;
    }
    protected void SetObjectDataIndex()
    {
        objData.index = baseIndex + plusIndex;
    }
}
