using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region [ Singleton ]
    static InventoryManager _uniqueInstance;
    public static InventoryManager _inst { get { return _uniqueInstance; } }
    #endregion [ Singleton ]

    //전체 아이템
    public SOItem[] items;    
    public float MaxItemWeights;

    public UI_Inventory invenUI;
    public float InvenWeight { get { return invenUI.GetItemWeights(); } }
    void Awake()
    {
        _uniqueInstance = this;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        invenUI.Init();
    }

    public bool CheckSlot(SOItem newItem, int cnt = 1)
    {
        if (invenUI.CheckSlotFull(newItem, cnt))
        {
            Debug.Log(MaxItemWeights);
            Debug.Log(InvenWeight + (newItem.itemWeight) * cnt);
            if (MaxItemWeights < InvenWeight + (newItem.itemWeight) * cnt)
                return true;
            else
                return false;
        }
        else
        {
            if (MaxItemWeights < InvenWeight + (newItem.itemWeight) * cnt)
                return true;
            else
                return false;
        }
            
    }

    public void AddInvenItem(SOItem newItem, int cnt = 1)
    {
        invenUI.AcquireItem(newItem, cnt);
    }
}
