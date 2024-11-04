using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineDatas
{
    public enum eScene
    {
        Unknown,   
        MainScene = 0,        
        GameScene = 1,
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
        Pet,
        DropItem,
        Effect,                
        UI,
        BGM,
        SFX,
    }
    public enum IconType
    {
        Pet,
        Item,
        Craft
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
        PetBall,
        Usable
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
        None = 1,        
        OneHand,
        Bow,
        Rifle,
        Pickaxe
    }
    public enum eUsableType
    {
        Shot,
        Food,
        Dinner
    }
    public enum eStatType
    {
        HP,
        Statmina,
        Attack,
        Defense,
        WorkAblity,
        CarryWeight
    }

    public enum eMonsterCharacterType
    {
        PASSIVE = 0,
        AGGRESSIVE,
    }

    public enum eSkillSubject
    {
        None = 0,
        Monster,
        Pet,
        Boss
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
        WORK,
        DIE,
        SLEEP,
        DISABLE
    }        

    public enum eBossType
    {
        Grounded,
        Flying,
        Dragon,
    }

    public enum eBossState
    {
        IDLE,
        SLEEP,
        GROWL,
        PATROL,
        RETURN,
        CHASE,        
        GETHIT,
        ATTACK,
        DIE,        
        DISABLE,
    }

    public enum eAttackType
    {
        None    =0,
        MeleeAttack,
        RangeAttack,
        Buff,
        HealBuff,
        AttackBuff,
        MaxCount
    }
    public enum ePlayerAnimLayers
    {
        BaseLayer,
        UnArmedLayer,
        OneHandLayer,
        BowLayer,
        RifleLayer,
        PickaxeLayer,

    }
    public enum ePlayerAnimParams
    {
        xDir     = 0,
        yDir,
        Speed,
        MotionSpeed,
        Ground,
        Jump,
        FreeFall,              
        Aim,
        Fire,
        WeaponType,
        Equip,
        Disarm,
        Charge,
        AttackEnd,
        Throw,
        Recall,
        Putin,
        Root,
        Reload,
        AttackAble,
        AcivateAnimation,
        Fix
    }
    public enum InteractionType
    {
        Craft,
        EnforceAnvil,
        PetBall,
        Brazier,
    }    

    public enum EquipState
    {
        None,
        Hat,
        Armor,        
    }

    public enum eSoundState
    {
        Jump,
        Land,
        AttackUnArmed,
        AttackOneHand,
        AttackBow,
        AttackRifle,        
        PickUp,
        GetHit,
        Equip
    }    

    [System.Serializable]
    public struct PlayerSoundInfo
    {
        public eSoundState State;
        public string Name;
    }
  
    public struct StartSoundSettingInfo
    {
        public bool m_isLoopBGM;
        public bool m_isMuteBGM;
        public float m_volumeBGM;
        public bool m_isLoopSFX;
        public bool m_isMuteSFX;
        public float m_volumeSFX;

        public StartSoundSettingInfo(bool loopB, bool muteB, float volB, bool loopS, bool muteS, float volS)
        {
            m_isLoopBGM = loopB;
            m_isMuteBGM = muteB;
            m_volumeBGM = volB;
            m_isLoopSFX = loopS;
            m_isMuteSFX = muteS;
            m_volumeSFX = volS;
        }
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
    
    public interface IHitAble
    {
        bool CheckAttackType(WeaponType type);
        void OnDamage(float damage, Transform attacker);
        void OnDamage(float damage, Transform attacker, Vector3 hitPoint);        
    }

    public enum LowDataType
    {
        ArchitectureTable,
        MaterialTable,
        PetLevelTable,
        PetTable,
        WeaponTable,
        PetBallTable,
        SkillTable,
        HitObjectTable,
        EquipmentTable,
        UsableTable,
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

    [System.Serializable]
    public class ItemDatas
    {
        public int index;
        public int count;
        public int level;

        public ItemDatas(int itemIndex, int cnt,int itemLevel)
        {
            index = itemIndex;
            count = cnt;
            level = itemLevel;
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

    [System.Serializable]
    public class PoolIcon
    {
        public int index;
        public string name;
        public IconType type;
        public Sprite prefab;              
    }
    [System.Serializable]
    public class PoolEffect
    {
        public string name;
        public PoolType type;
        public GameObject prefab;
        public int amount;
        int curAmount;
        public int CurAmount { get { return curAmount; } set { curAmount = value; } }
    }
   [System.Serializable]
   public class PoolSoundClip
    {
        public string name;
       
        public AudioClip m_clip;
        
    }
}