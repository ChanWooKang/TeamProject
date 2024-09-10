using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class DataManager
{    
    public Dictionary<int, RequiredEXPByLevel> Dict_RequiredExp { get; private set; }
    public Dictionary<int, MonsterInfo> Dict_Monster;
    public Dictionary<int, MonsterLevelInfo> Dict_MonsterLevel;    
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

        Dict_Monster = new Dictionary<int, MonsterInfo>();
        AddMonster();
        Dict_MonsterLevel = new Dictionary<int, MonsterLevelInfo>();
        AddMonsterLevel();
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
        

        MonsterInfo monster = new MonsterInfo(index, nameEn, nameKr, desc, hp, speed, runSpeed,
            range, damage, attackDelay, sight, workAbility, type, rate, rewards,chaseRange,counts);
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
}
