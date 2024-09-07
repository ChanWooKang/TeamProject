using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

[AddComponentMenu("Custom/PoolingManager")]
public class PoolingManager : TSingleton<PoolingManager>
{
    public PoolUnit[] _poolingUnits;
    //public List<GameObject>[] _pooledUnitList;
    public int _defPoolAmount;
    public bool _canPoolExpand = true;

    public Dictionary<string, PoolUnit> _poolingUnitByName;
    public Dictionary<int, PoolUnit> _poolingUnitByIndex;
    public Dictionary<string, Dictionary<int, GameObject>> _pooledUnitsByName;
    public Dictionary<int, Dictionary<int, GameObject>> _pooledUnitsByIndex;


    public Transform _hudRootTransform;


    public void LoadObjectPool()
    {
        _pooledUnitsByIndex = new Dictionary<int, Dictionary<int, GameObject>>();
        _pooledUnitsByName = new Dictionary<string, Dictionary<int, GameObject>>();
        _poolingUnitByIndex = new Dictionary<int, PoolUnit>();
        _poolingUnitByName = new Dictionary<string, PoolUnit>();
        for (int i = 0; i < _poolingUnits.Length; i++)
        {
            Dictionary<int, GameObject> objDatas = new Dictionary<int, GameObject>();

            SettingPoolingUnits(i);
            for (int index = 0; index < _poolingUnits[i].CurAmount; index++)
            {
                objDatas.Add(index, MakeObject(_poolingUnits[i].prefab));
            }
            _pooledUnitsByIndex.Add(_poolingUnits[i].index, objDatas);
            _pooledUnitsByName.Add(_poolingUnits[i].name, objDatas);
        }
    }
    public void AddPetPool(PetController pet)
    {
        int offsetNum = 100;
        Array.Resize(ref _poolingUnits, _poolingUnits.Length + 1);
        int lastIndex = _poolingUnits.Length - 1;
        _poolingUnits[lastIndex] = new PoolUnit();
        LowBase table = Managers._table.Get(LowDataType.PetTable);
        int petIndex = table.Find("NameKr", pet.PetInfo.NameKr);
        string nameKr = table.ToStr(petIndex, "NameKr");
        _poolingUnits[lastIndex].index = petIndex + offsetNum;
        _poolingUnits[lastIndex].amount = 1;
        _poolingUnits[lastIndex].name = nameKr;
        _poolingUnits[lastIndex].prefab = pet.gameObject;
        _poolingUnits[lastIndex].type = PoolType.Pet;

        GameObject hud = InstantiateAPS(1000000);
        hud.SetActive(true);
        HudController hudctrl = hud.GetComponent<HudController>();
        pet.SetHud(hudctrl, _hudRootTransform);
    }
    void SettingPoolingUnits(int index)
    {
        if (_poolingUnits[index].amount > 0)
            _poolingUnits[index].CurAmount = _poolingUnits[index].amount;
        else
            _poolingUnits[index].CurAmount = _defPoolAmount;

        if (_poolingUnitByName.ContainsKey(_poolingUnits[index].name) == false)
            _poolingUnitByName.Add(_poolingUnits[index].name, _poolingUnits[index]);


        if (_poolingUnitByIndex.ContainsKey(_poolingUnits[index].index) == false)
            _poolingUnitByIndex.Add(_poolingUnits[index].index, _poolingUnits[index]);
    }

    GameObject MakeObject(GameObject prefab, Transform parent = null)
    {
        GameObject newItem = (GameObject)Instantiate(prefab);

        SetActiveAndParent(newItem, parent);
        return newItem;
    }

    void SetActiveAndParent(GameObject newItem, Transform parent = null)
    {
        newItem.SetActive(false);
        if (parent == null)
            newItem.transform.SetParent(transform);
        else
            newItem.transform.SetParent(parent);

        if (newItem.layer == LayerMask.NameToLayer("UI") && newItem.transform.parent != _hudRootTransform)
        {
            newItem.transform.SetParent(_hudRootTransform);
        }
    }

    GameObject GetPooledItem(int index, Transform parent = null)
    {
        if (_pooledUnitsByIndex.ContainsKey(index))
        {
            foreach (var obj in _pooledUnitsByIndex[index])
            {
                if (obj.Value.TryGetComponent(out HudController hud))
                {
                    if (!hud.IsInit())
                        return obj.Value;
                }
                else
                {
                    if (obj.Value.activeInHierarchy == false)
                        return obj.Value;
                }
            }

            //眠啊 积己
            if (_canPoolExpand)
            {
                if (_poolingUnitByIndex.ContainsKey(index))
                {
                    GameObject tmpObj = MakeObject(_poolingUnitByIndex[index].prefab, parent);
                    int unitIndex = _pooledUnitsByIndex[index].Count;
                    string unitName = _poolingUnitByIndex[index].name;
                    _pooledUnitsByIndex[index].Add(unitIndex, tmpObj);
                    if (_pooledUnitsByName.ContainsKey(unitName))
                    {
                        unitIndex = _pooledUnitsByName[unitName].Count;
                        _pooledUnitsByName[unitName].Add(unitIndex, tmpObj);
                    }
                    return tmpObj;
                }
            }
        }
        return null;
    }

    GameObject GetPooledItem(string index, Transform parent = null)
    {
        if (_pooledUnitsByName.ContainsKey(index))
        {
            foreach (var obj in _pooledUnitsByName[index])
            {
                if (obj.Value.activeInHierarchy == false)
                    return obj.Value;
            }

            //眠啊 积己
            if (_canPoolExpand)
            {
                if (_poolingUnitByName.ContainsKey(index))
                {
                    GameObject tmpObj = MakeObject(_poolingUnitByName[index].prefab, parent);
                    int unitIndex = _pooledUnitsByName[index].Count;
                    int listIndex = _poolingUnitByName[index].index;
                    _pooledUnitsByName[index].Add(unitIndex, tmpObj);
                    if (_pooledUnitsByIndex.ContainsKey(listIndex))
                    {
                        unitIndex = _pooledUnitsByIndex[listIndex].Count;
                        _pooledUnitsByIndex[listIndex].Add(unitIndex, tmpObj);
                    }
                    return tmpObj;
                }
            }
        }
        return null;
    }


    public GameObject InstantiateAPS(int index, Transform parent = null)
    {
        GameObject go = GetPooledItem(index);
        if (parent != null)
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
        if (go.transform.parent != _inst.transform)
            go.transform.SetParent(_inst.transform);
    }
}

