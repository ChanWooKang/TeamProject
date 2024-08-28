using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineDatas
{
    public enum eScene
    {
        Unknown,
        SampleScene = 1,
        ChanWooScene = 2,
    }
    
    public enum eLayer
    {
        UI = 5,
        Ground = 6,
        Player = 7,
        Obstacle = 8,
        DisableObject = 9,
        Monster = 10,
        Item = 11
    }

    public enum MouseEvent
    {
        Click,
        Press,
        PointerDown,
        PointerUp
    }

    public enum PoolType
    {
        Monster,
        DropItem,
        Effect,
        UI
    }

    public enum eInteractType
    {
        Unknown = 0,
        NPC,
        Item,
    }

    public enum eNPCType
    {
        Unknown = 0,
        Tutorial,        
    }

    public enum eItemType
    {
        Unknown = 0,
        Material,
        Weapon,
        Equipment,
        Potion,        
        Gold,
        PetBall
    }

    public enum eEquipType
    {
        Unknown = 0,
        Weapon,
        Head,
        Armor,
        Max_Count
    }

    public enum WeaponType
    {
        None = 0,
        OneHand,
        Bow,
        Rifle,
    }

    public enum eMonsterCharacterType
    {
        PASSIVE = 0,
        AGGRESSIVE,
    }

    public enum eMonsterState
    {                
        IDLE,
        PATROL,
        SENSE,
        CHASE,
        RETURN,
        ATTACK,
        KNOCKBACK,
        DIZZY,
        GETHIT,
        DIE,
        DISABLE
    }

    public enum eAttackType
    {
        None    =0,
        MeleeAttack,
        RangeAttack,
        MaxCount
    }

    public enum ePlayerAnimParams
    {
        xDir,
        yDir,
        Speed,
        Ground,
        Jump,
        FreeFall,
        MotionSpeed,        
        Aim,
        Equip,
        Disarm,
        WeaponType,
        Fire,
        AttackEnd

    }


    public interface IFSMState<T>
    {
        void Enter(T m);
        void Execute(T m);
        void Exit(T m);
    }

    public interface ILoader<Key, Value>
    {
        Dictionary<Key, Value> Make();
    }

    public enum LowDataType
    {
        ArchitectureTable,
        MaterialTable,
        PetLevelTable,
        PetTable,
        WeaponTable,
        PetBallTable,
    }

    [System.Serializable]
    public struct ItemSprite
    {
        public int index;
        public Sprite icon;
    }

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform parent;
        public int Index;
        public int maxSpawnCount;
    }
}

// 데이터 관련 클래스 정리
namespace DefineDatas
{
    [System.Serializable]
    public class RequiredEXPByLevel
    {
        public int level;
        public float exp;
    }

    [System.Serializable]
    public class EXPData : ILoader<int, RequiredEXPByLevel>
    {
        public List<RequiredEXPByLevel> stats = new List<RequiredEXPByLevel>();
        public Dictionary<int, RequiredEXPByLevel> Make()
        {
            Dictionary<int, RequiredEXPByLevel> dict = new Dictionary<int, RequiredEXPByLevel>();
            foreach (RequiredEXPByLevel stat in stats)
            {
                dict.Add(stat.level, stat);
            }
            return dict;
        }
        public EXPData() { }
    }
}


//아이템 관련 클래스 정리
namespace DefineDatas
{
    [System.Serializable]
    public class RequiredItem
    {
        public SOItems items;
        public int cnt;
    }

    public class ItemSlotAndCount
    {
        public List<int> slotNumbers = new List<int>();
        public List<int> itemCounts = new List<int>();

        public ItemSlotAndCount(List<int> slots, List<int> items)
        {
            slotNumbers = slots;
            itemCounts = items;
        }
    }
}


namespace DefineDatas
{
    //Pooling
    [System.Serializable]
    public class PoolUnit
    {
        public int index;
        public string name;
        public PoolType type;
        public GameObject prefab;
        public int amount;
        int curAmount;
        public int CurAmount { get { return curAmount; } set { curAmount = value; } }
    }


   
}