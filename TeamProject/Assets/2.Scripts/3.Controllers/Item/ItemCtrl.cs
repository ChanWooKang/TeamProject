using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class ItemCtrl : MonoBehaviour
{
    public int itemIndex;
    BaseItem item = null;

    private void Start()
    {

        
    }

    public bool Root()
    {
        if (item == null)
            item = InventoryManager._inst.GetItemData(itemIndex);


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
