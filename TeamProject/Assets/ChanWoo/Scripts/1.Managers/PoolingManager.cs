using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

[AddComponentMenu("Custom/PoolingManager")]
public class PoolingManager : MonoBehaviour
{
    static PoolingManager _uniqueInstance;
    public PoolUnit[] _poolingUnits;
    public List<GameObject>[] _pooledUnitList;
    public int _defPoolAmount;
    public bool _canPoolExpand = true;

    const string objName = "@Pool";
    public static PoolingManager _inst { get { Init(); return _uniqueInstance; } }

    static void Init()
    {
        if(_uniqueInstance == null)
        {
            GameObject go = GameObject.Find(objName);
            if (go == null)
            {
                go = new GameObject { name = objName };
                go.AddComponent<PoolingManager>();
            }

            _uniqueInstance = go.GetComponent<PoolingManager>();
        }
    }

    public void Clear()
    {
        _uniqueInstance = null;
    }

    public void LoadObjectPool()
    {
        _pooledUnitList = new List<GameObject>[_poolingUnits.Length];
        for (int i = 0; i < _poolingUnits.Length; i++)
        {
            _pooledUnitList[i] = new List<GameObject>();
            if (_poolingUnits[i].amount > 0)
                _poolingUnits[i].CurAmount = _poolingUnits[i].amount;
            else
                _poolingUnits[i].CurAmount = _defPoolAmount;

            int index = 0;
            for (int j = 0; j < _poolingUnits[i].CurAmount; j++)
            {
                GameObject newItem = (GameObject)Instantiate(_poolingUnits[i].prefab);
                string suffix = "_" + index;
                AddToPooledUnitList(i, newItem, suffix);
                ++index;
            }
        }
    }

    void AddToPooledUnitList(int index, GameObject newItem, string suffix, Transform parent = null)
    {
        newItem.name += suffix;
        newItem.SetActive(false);
        if (parent == null)
            newItem.transform.SetParent(transform);
        else
            newItem.transform.SetParent(parent);
        _pooledUnitList[index].Add(newItem);
    }

    GameObject GetPooledItem(string value)
    {
        for (int unitIdx = 0; unitIdx < _pooledUnitList.Length; unitIdx++)
        {
            if (_poolingUnits[unitIdx].prefab.name == value)
            {
                int listIdx = 0;
                for (; listIdx < _pooledUnitList[unitIdx].Count; listIdx++)
                {
                    if (_pooledUnitList[unitIdx][listIdx] == null)
                        return null;
                    if (_pooledUnitList[unitIdx][listIdx].activeInHierarchy == false)
                        return _pooledUnitList[unitIdx][listIdx];
                }

                if (_canPoolExpand)
                {
                    GameObject tmpObj = (GameObject)Instantiate(_poolingUnits[unitIdx].prefab);
                    string suffix = $"_{listIdx}({(listIdx - _poolingUnits[unitIdx].CurAmount + 1)})";
                    AddToPooledUnitList(unitIdx, tmpObj, suffix);
                    return tmpObj;
                }
                break;
            }
        }
        return null;
    }

    GameObject GetPooledItem(int index)
    {
        for (int unitIdx = 0; unitIdx < _pooledUnitList.Length; unitIdx++)
        {
            if (_poolingUnits[unitIdx].index == index)
            {
                int listIdx = 0;
                for (; listIdx < _pooledUnitList[unitIdx].Count; listIdx++)
                {
                    if (_pooledUnitList[unitIdx][listIdx] == null)
                        return null;
                    if (_pooledUnitList[unitIdx][listIdx].activeInHierarchy == false)
                        return _pooledUnitList[unitIdx][listIdx];
                }

                if (_canPoolExpand)
                {
                    GameObject tmpObj = (GameObject)Instantiate(_poolingUnits[unitIdx].prefab);
                    string suffix = $"_{listIdx}({(listIdx - _poolingUnits[unitIdx].CurAmount + 1)})";
                    AddToPooledUnitList(unitIdx, tmpObj, suffix);
                    return tmpObj;
                }
                break;
            }
        }
        return null;
    }

    public GameObject InstantiateAPS(int index, Transform parent = null)
    {
        GameObject go = GetPooledItem(index);
        if(parent != null)
        {
            go.transform.SetParent(parent);
        }

        go.SetActive(true);
        return go;
    }

    public GameObject InstantiateAPS(
        int index, Vector3 pos, Quaternion rot, Vector3 scale, Transform parent = null
        )
    {
        GameObject go = GetPooledItem(index);
        if (go != null)
        {
            if (parent != null)
                go.transform.SetParent(parent);
            go.transform.position = pos;
            go.transform.rotation = rot;
            go.transform.localScale = scale;
            go.SetActive(true);
        }
        return go;
    }

    public GameObject InstantiateAPS(string pooledUnitName, Transform parent = null)
    {
        GameObject go = GetPooledItem(pooledUnitName);
        if (parent != null)
            go.transform.SetParent(parent);
        go.SetActive(true);
        return go;
    }

    public GameObject InstantiateAPS(
        string pooledUnitName, Vector3 pos, Quaternion rot, Vector3 scale, Transform parent = null
        )
    {
        GameObject go = GetPooledItem(pooledUnitName);
        if (go != null)
        {
            if (parent != null)
                go.transform.SetParent(parent);
            go.transform.position = pos;
            go.transform.rotation = rot;
            go.transform.localScale = scale;
            go.SetActive(true);
        }
        return go;
    }

    public static void DestroyAPS(GameObject go)
    {
        go.SetActive(false);
        if (go.transform.parent != _uniqueInstance.transform)
            go.transform.SetParent(_uniqueInstance.transform);
    }
}

