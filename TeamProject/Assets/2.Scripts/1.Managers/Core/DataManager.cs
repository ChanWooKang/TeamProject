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
    public Dictionary<int, List<SkillInfo>> Dict_MonsterSkill;
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

        string countsIndexStr = table.ToStr(index, "RewardCount");
        strArray = countsIndexStr.Split('/');
        int[] counts = new int[strArray.Length];
        for(int i = 0; i < counts.Length; i++)
        {
            if(int.TryParse(strArray[i],out int number))
            {
                counts[i] = number;
            }
        }

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
            range, damage, attackDelay, sight, workAbility, type, rate, rewards,chaseRange,counts,skills);
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
        float requiredExp = table.ToFloat(index, "NextExp");
        float rewardAbility = table.ToFloat(index, "RewardAbility");
        float rewardExp = table.ToFloat(index, "RewardExp");

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
}
