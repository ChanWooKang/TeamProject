using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChanWooDefineDatas
{
    #region [ Enum ] 
    public enum eItemType
    {
        Unknown = 0,
        Equipment,
        Useable,
        Ingredient,
        ETC
    }

    public enum eEquipmentType
    {
        Unknown = 0,
        Helm,
        Armor,
        Leg,
        Shoes
    }

    public enum eStatType
    {
        Unknown = 0,
        HP,        

    }

    //상호작용 타임 ( NPC, 상점, 아이템 )
    public enum eInteractiveType
    {
        Unknown = 0,
        NPC,        
    }

    //NPC타입 ( 그냥 대화하는 NPC, Quest주는 NPC, 상점 NPC)
    public enum eNPCType
    {
        Base    = 0,
        Tutorial,
        Quest,
        Shop,

    }

    #endregion [ Enum ]


    #region [ Struct ]
    [System.Serializable]
    public struct STAT
    {
        public eStatType statType;
        public string statName;
        public string descName;
        //스탯 증가치 ( + a 이거나 * a ) 값으로 처리
        public float statValue;
    }
    #endregion [ Struct ]
}