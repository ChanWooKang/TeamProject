using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BaseItem : MonoBehaviour
{
    public SOItem item;
    
    public bool Root()
    {
        if (InventoryManager._inst.CheckSlot(item) == false)
        {
            InventoryManager._inst.AddInvenItem(item, 1);
            Despawn();
            return true;
        }        
        return false;
    }

    
    void Despawn()
    {
        Destroy(gameObject);
    }
}
