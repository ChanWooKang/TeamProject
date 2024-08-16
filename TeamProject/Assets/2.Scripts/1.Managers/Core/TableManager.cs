using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
using System.Linq;
using System.Reflection;


public class TableManager 
{
    Dictionary<LowDataType, LowBase> m_gameTables = new Dictionary<LowDataType, LowBase>();

    public void Init()
    {        
        m_gameTables = new Dictionary<LowDataType, LowBase>();

        LoadTableAll();
    }

    // 전체 로드
    public void LoadTableAll()
    {
        // 현재 어셈블리(혹은 다른 어셈블리)에서 LowBase를 상속받는 모든 타입을 찾습니다.
        Assembly currentAssembly = Assembly.GetExecutingAssembly();// 현재 어셈블리 가져오기
        Type baseType = typeof(LowBase);
        IEnumerable<Type> subTypes = currentAssembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

        // 각 타입을 m_gameTables에 추가합니다.
        foreach (Type subType in subTypes)
        {
            // LowDataType 열거값을 찾거나 생성합니다. (여기서는 Type의 이름을 사용)
            LowDataType dataType = (LowDataType)Enum.Parse(typeof(LowDataType), subType.Name);

            // 타입을 인스턴스화하여 m_gameTables에 추가합니다.
            LowBase instance = (LowBase)Activator.CreateInstance(subType); // 기존의 new 선언
            instance.Load(dataType.ToString()); // 제이슨 파일 로드
            m_gameTables.Add(dataType, instance);
        }
    }
    // get(테이블명)
    public LowBase Get(LowDataType dataType)
    {
        if (m_gameTables.ContainsKey(dataType))
        {
            return m_gameTables[dataType];
        }
        else
        {
            Debug.LogError($"Table '{dataType}' not found!");
            return null;
        }
    }
}
