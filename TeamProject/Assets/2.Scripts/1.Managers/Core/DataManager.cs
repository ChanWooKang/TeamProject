using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class DataManager
{    
    public Dictionary<int, RequiredEXPByLevel> Dict_RequiredExp { get; private set; }
    public Dictionary<int, MonsterInfo> Dict_Monster;
    public Dictionary<int, MonsterLevelInfo> Dict_MonsterLevel;
    public Dictionary<int, SkillInfo> Dict_Skill;
    public Dictionary<int, HitObjectInfo> Dict_HitObject;
    
    public Dictionary<int, List<SkillInfo>> Dict_MonsterSkill;
    public Dictionary<int, int> Dict_UniqueID;
    const string DRE = "RequiredEXPByLevel";

    public void Init()
    {
        LoadData();                
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        string text = Managers._file.LoadJsonFile(path);
        if (!string.IsNullOrEmpty(text))
            return JsonUtility.FromJson<Loader>(text);
        else
        {
            TextAsset data = Resources.Load<TextAsset>($"Json/{path}");
            Loader datas = JsonUtility.FromJson<Loader>(data.ToString());
            Managers._file.SaveJsonFile(datas, path);
            return datas;
        }
    }

    void LoadData()
    {
        Dict_RequiredExp = new Dictionary<int, RequiredEXPByLevel>();
        Dict_RequiredExp = LoadJson<EXPData, int, RequiredEXPByLevel>(DRE).Make();
        Dict_Skill = new Dictionary<int, SkillInfo>();
        AddSkillTable();
        Dict_Monster = new Dictionary<int, MonsterInfo>();
        AddMonster();
        Dict_MonsterLevel = new Dictionary<int, MonsterLevelInfo>();
        AddMonsterLevel();
        Dict_MonsterSkill = new Dictionary<int, List<SkillInfo>>();
        AddMonsterSkill();
        Dict_UniqueID = new Dictionary<int, int>();
        Dict_HitObject = new Dictionary<int, HitObjectInfo>();
        AddHitObject();
    }

    public bool AddUniqueID(int uniqueID, int index)
    {
        if (Dict_UniqueID.ContainsKey(uniqueID))
        {
            //고유 번호가 존재합니다.
            return false;
        }
        else
        {
            Dict_UniqueID.Add(uniqueID, index);
            return true;
        }
    }

    void AddMonster()
    {
        LowBase table = Managers._table.Get(LowDataType.PetTable);
        int maxCount = table.MaxCount();
        int offSetNum = 1000;
        for (int i = 0; i < maxCount; i++)
        {
            MakeMonsterClass(table, i, offSetNum);
        }
    }    

    void MakeMonsterClass(LowBase table, int num, int offSetNumber)
    {
        int index = offSetNumber + num;
        string nameEn = table.ToStr(index,"NameEn");
        string nameKr = table.ToStr(index, "NameKr");
        string desc = table.ToStr(index, "Desc");
        float hp = table.ToFloat(index, "Hp"); 
        float speed = table.ToFloat(index, "Speed"); 
        float runSpeed = table.ToFloat(index, "RunSpeed"); 
        float range = table.ToFloat(index, "Range"); 
        float damage = table.ToFloat(index, "Damage"); 
        float attackDelay = table.ToFloat(index, "AttackDelay"); 
        float sight = table.ToFloat(index, "Sight"); 
        float workAbility = table.ToFloat(index, "WorkAbility"); 
        int type = table.ToInt(index, "Type"); 
        int rate = table.ToInt(index, "Rate");
        float chaseRange = table.ToFloat(index, "ChaseRange");        
        string rewardsIndexStr = table.ToStr(index, "Reward");
        string[] strArray = rewardsIndexStr.Split('/');
        int[] rewards = new int[strArray.Length];
        for(int i = 0; i < rewards.Length; i++)
        {
            if (int.TryParse(strArray[i], out int number))
                rewards[i] = number;
        }

        int rewardCount = table.ToInt(index, "RewardCount");        

        string skillsIndexStr = table.ToStr(index, "Skill");
        strArray = skillsIndexStr.Split('/');
        int[] skills = new int[strArray.Length];
        for(int i = 0; i < skills.Length; i++)
        {
            if (int.TryParse(strArray[i],out int number))
            {
                skills[i] = number;
            }
        }
        
        MonsterInfo monster = new MonsterInfo(index, nameEn, nameKr, desc, hp, speed, runSpeed,
            range, damage, attackDelay, sight, workAbility, type, rate, rewards,chaseRange,rewardCount,skills);
        if (Dict_Monster.ContainsKey(index) == false)
            Dict_Monster.Add(index, monster);
        
    }

    void AddMonsterLevel()
    {
        LowBase table = Managers._table.Get(LowDataType.PetLevelTable);
        int maxCount = table.MaxCount();
        int offSetNum = 0;
        for( int i = 0; i < maxCount; i++)
        {
            MakeMonsterLevelClass(table, i, offSetNum);
        }
    }

    void MakeMonsterLevelClass(LowBase table, int num, int offSetNum)
    {
        int index = num + offSetNum;
        int requiredExp = table.ToInt(index, "NextExp");
        int rewardExp = table.ToInt(index, "RewardExp");
        float rewardAbility = table.ToFloat(index, "RewardAbility");

        MonsterLevelInfo level = new MonsterLevelInfo(index, requiredExp, rewardAbility, rewardExp);
        if (Dict_MonsterLevel.ContainsKey(index) == false)
            Dict_MonsterLevel.Add(index, level);
    }
    
    void AddSkillTable()
    {
        LowBase table = Managers._table.Get(LowDataType.SkillTable);
        int maxCount = table.MaxCount();
        int offSetNum = 1100000;
        for(int i = 0;i < maxCount; i++)
        {
            MakeSkillClass(table, i, offSetNum);
        }
    }

    void MakeSkillClass(LowBase table, int num, int offSetNum)
    {
        int index = offSetNum + num;
        string nameKr = table.ToStr(index, "NameKr");
        string nameEn = table.ToStr(index, "NameEn");
        float damageTimes = table.ToFloat(index, "DamageTimes");        
        eAttackType attackType = (eAttackType)table.ToInt(index, "Type");
        string desc = table.ToStr(index, "Desc");
        int rate = table.ToInt(index, "Rate");
        SkillInfo skill = new SkillInfo(index, nameKr, nameEn, damageTimes, attackType, desc, rate);
        if(Dict_Skill.ContainsKey(index) == false)
            Dict_Skill.Add(index, skill);
    }

    void AddMonsterSkill()
    {
        if (Dict_Monster != null)
        {
            foreach (var values in Dict_Monster)
            {
                if (Dict_MonsterSkill.ContainsKey(values.Key) == false)
                {
                    Dict_MonsterSkill.Add(values.Key, GetSkillList(values.Key));
                }
            }
        }
    }

    public List<SkillInfo> GetSkillList(int index)
    {
        List<SkillInfo> skills = new List<SkillInfo>();
        if (Dict_Monster.ContainsKey(index))
        {
            int[] skillIndexs = Dict_Monster[index].Skills;
            for (int i = 0; i < skillIndexs.Length; i++)
            {
                if (Dict_Skill.ContainsKey(skillIndexs[i]))
                {
                    skills.Add(Dict_Skill[skillIndexs[i]]);
                }
            }
        }
        return skills;
    }

    void AddHitObject()
    {
        LowBase table = Managers._table.Get(LowDataType.HitObjectTable);
        int maxCount = table.MaxCount();
        int offSetNum = 900;
        for (int i = 0; i < maxCount; i++)
            MakeHitObjectClass(table, i, offSetNum);
    }

    void MakeHitObjectClass(LowBase table, int num, int offSetNumber)
    {
        int index = offSetNumber + num;
        string nameEn = table.ToStr(index, "NameEN");
        string nameKr = table.ToStr(index, "NameKr");
        float hp = table.ToFloat(index, "HP");
        int reward = table.ToInt(index, "Reward");
        int rewardCount = table.ToInt(index, "RewardCount");
        HitObjectInfo info = new HitObjectInfo(index, nameKr, nameEn, hp, reward, rewardCount);
        if (Dict_HitObject.ContainsKey(index) == false)
            Dict_HitObject.Add(index, info);
    }
}
