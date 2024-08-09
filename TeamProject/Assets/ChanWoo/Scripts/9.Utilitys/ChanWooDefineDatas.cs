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

    //��ȣ�ۿ� Ÿ�� ( NPC, ����, ������ )
    public enum eInteractiveType
    {
        Unknown = 0,
        NPC,        
    }

    //NPCŸ�� ( �׳� ��ȭ�ϴ� NPC, Quest�ִ� NPC, ���� NPC)
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
        //���� ����ġ ( + a �̰ų� * a ) ������ ó��
        public float statValue;
    }
    #endregion [ Struct ]
}