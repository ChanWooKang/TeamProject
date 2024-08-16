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

    // ��ü �ε�
    public void LoadTableAll()
    {
        // ���� �����(Ȥ�� �ٸ� �����)���� LowBase�� ��ӹ޴� ��� Ÿ���� ã���ϴ�.
        Assembly currentAssembly = Assembly.GetExecutingAssembly();// ���� ����� ��������
        Type baseType = typeof(LowBase);
        IEnumerable<Type> subTypes = currentAssembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

        // �� Ÿ���� m_gameTables�� �߰��մϴ�.
        foreach (Type subType in subTypes)
        {
            // LowDataType ���Ű��� ã�ų� �����մϴ�. (���⼭�� Type�� �̸��� ���)
            LowDataType dataType = (LowDataType)Enum.Parse(typeof(LowDataType), subType.Name);

            // Ÿ���� �ν��Ͻ�ȭ�Ͽ� m_gameTables�� �߰��մϴ�.
            LowBase instance = (LowBase)Activator.CreateInstance(subType); // ������ new ����
            instance.Load(dataType.ToString()); // ���̽� ���� �ε�
            m_gameTables.Add(dataType, instance);
        }
    }
    // get(���̺��)
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
