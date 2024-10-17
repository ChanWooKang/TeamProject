using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class ObjectSettings : MonoBehaviour
{    
    [SerializeField] GameObject _hair;
    [SerializeField] GameObject _legs;
    [SerializeField] GameObject _underarmor;

    [SerializeField] List<EquipmentData> EquipmentObjects;
    [SerializeField] GameObject _armor;
    [SerializeField] GameObject _hat;

    Dictionary<int, EquipmentData> Dict_EquipmentData;
    public void Init()
    {
        Dict_EquipmentData = new Dictionary<int, EquipmentData>();

        foreach(var data in EquipmentObjects)
        {
            if (!Dict_EquipmentData.ContainsKey(data.Index))
                Dict_EquipmentData.Add(data.Index, data);
        }
        
    }

    public void Setting(EquipState state, bool isOn, int itemIndex = 0)
    {
        switch (state)
        {
            case EquipState.None:
                Setting(EquipState.Hat, !isOn, itemIndex);
                Setting(EquipState.Armor, !isOn, itemIndex);
                break;
            case EquipState.Hat:
                if (Dict_EquipmentData.ContainsKey(itemIndex))
                {
                    _hair.SetActive(!isOn);
                    Dict_EquipmentData[itemIndex].go.SetActive(isOn);                    
                }                                                
                break;
            case EquipState.Armor:
                if (Dict_EquipmentData.ContainsKey(itemIndex))
                {
                    _underarmor.SetActive(!isOn);
                    _legs.SetActive(!isOn);
                    Dict_EquipmentData[itemIndex].go.SetActive(isOn);
                }                                
                break;
        }
    }    
}

[System.Serializable]
public struct EquipmentData
{
    public int Index;
    public GameObject go;
}