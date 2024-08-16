using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LowBase : MonoBehaviour
{
    Dictionary<string, Dictionary<string, string>> m_sheetDatas = new Dictionary<string, Dictionary<string, string>>();

    public abstract void Load(string jsonData);

    public void Add(string key, string subKey, string val)
    {
        if (!m_sheetDatas.ContainsKey(key))
        {
            m_sheetDatas[key] = new Dictionary<string, string>();
        }
        m_sheetDatas[key][subKey] = val;
    }

    public int MaxCount()
    {
        return m_sheetDatas.Count;
    }

    // ToStr, ToInt
    public string ToStr(string index, string columnName)
    {
        if (m_sheetDatas.ContainsKey(index) && m_sheetDatas[index].ContainsKey(columnName))
        {
            return m_sheetDatas[index][columnName];
        }
        return "";
    }
    public string ToStr(int index, string columnName)
    {
        if (m_sheetDatas.ContainsKey(index.ToString()) && m_sheetDatas[index.ToString()].ContainsKey(columnName))
        {
            return m_sheetDatas[index.ToString()][columnName];
        }
        return "";
    }

    public int ToInt(string index, string columnName)
    {
        if (m_sheetDatas.ContainsKey(index) && m_sheetDatas[index].ContainsKey(columnName))
        {
            if (int.TryParse(m_sheetDatas[index][columnName], out int result))
            {
                return result;
            }
            else
                return -1;
        }
        return 0;
    }
    public int ToInt(int index, string columnName)
    {
        if (m_sheetDatas.ContainsKey(index.ToString()) && m_sheetDatas[index.ToString()].ContainsKey(columnName))
        {
            if (int.TryParse(m_sheetDatas[index.ToString()][columnName], out int result))
            {
                return result;
            }
            else
                return -1;
        }
        return 0;
    }

    public float ToFloat(string index, string columnName)
    {
        if (m_sheetDatas.ContainsKey(index) && m_sheetDatas[index].ContainsKey(columnName))
        {
            if (float.TryParse(m_sheetDatas[index][columnName], out float result))
            {
                return result;
            }
            else
                return -1;
        }
        return 0f;
    }
    public float ToFloat(int index, string columnName)
    {
        if (m_sheetDatas.ContainsKey(index.ToString()) && m_sheetDatas[index.ToString()].ContainsKey(columnName))
        {
            if (float.TryParse(m_sheetDatas[index.ToString()][columnName], out float result))
            {
                return result;
            }
            else
                return -1;
        }
        return 0f;
    }
    public bool ToBool(string index, string columnName)
    {
        if (m_sheetDatas.ContainsKey(index) && m_sheetDatas[index].ContainsKey(columnName))
        {
            if (bool.TryParse(m_sheetDatas[index][columnName], out bool result))
            {
                return result;
            }
        }
        return false;
    }
    public bool ToBool(int index, string columnName)
    {
        if (m_sheetDatas.ContainsKey(index.ToString()) && m_sheetDatas[index.ToString()].ContainsKey(columnName))
        {
            if (bool.TryParse(m_sheetDatas[index.ToString()][columnName], out bool result))
            {
                return result;
            }
        }
        return false;
    }

    //Finde Index
    public int Find(string column, string value)
    {
        int index = -1;
        foreach (string record in m_sheetDatas.Keys) // �̷������δ� �ᱹ ������ �ȿ� �����͸� ã�� ���̹Ƿ� ȿ�������� ���ϴ�. ���� ����� �޾Ƽ� ���ϴ� ���� ���� ��
        {
            if (m_sheetDatas[record].ContainsKey(column)) // �� �� if���� ��� �Ǵ��� �����غ���
                if (m_sheetDatas[record][column].CompareTo(value) == 0)
                {
                    index = int.Parse(record);
                    break;
                }
        }
        if (index == -1)
            Debug.LogError("Can't Find Index of Column");
        return index;
    }
    public int[] Finds(string column, string value)
    {
        List<int> m_findIndex = new List<int>();
        int index = 0;
        foreach (string record in m_sheetDatas.Keys)
        {
            if (m_sheetDatas[record].ContainsKey(column))
            {
                if (m_sheetDatas[record][column].CompareTo(value) == 0)
                {
                    index = int.Parse(record);
                    m_findIndex.Add(index);
                }
            }
            else
            {
                Debug.LogError("�÷��� ���̺� �����ϴ�.");
                return null;
            }
        }
        if (m_findIndex.Count == 0)
        {
            Debug.LogError("�ش��ϴ� �÷��� �ε����� �����ϴ�.");
        }

        return m_findIndex.ToArray();

    }

}
