using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineDatas
{
    public enum MouseEvent
    {
        Click,
        Press,
        PointerDown,
        PointerUp
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
        Equipment,
        Potion,
        ETC,
        Gold
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
