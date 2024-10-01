using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_Drop : UI_Base, IDropHandler
{
    public override void Init()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot._inst.SlotInven != null)
        {
            DragSlot._inst.SlotInven.ClearSlot();
        }
    }
}
