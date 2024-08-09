using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChanWooDefineDatas;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable/Equipment")]
public class ChanWooSOEquipment : ChanWooSOItem
{
    public eEquipmentType equipType;
    public List<STAT> statList = new List<STAT>();

}
