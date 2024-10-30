using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

[AddComponentMenu("Custom/PoolingManager")]
public class PoolingManager : TSingleton<PoolingManager>
{
    #region [UnitPool]
    public PoolUnit[] _poolingUnits;

    //public List<GameObject>[] _pooledUnitList;
    public int _defPoolAmount;
    public bool _canPoolExpand = true;

    public Dictionary<string, PoolUnit> _poolingUnitByName;
    public Dictionary<int, PoolUnit> _poolingUnitByIndex;
    public Dictionary<string, Dictionary<int, GameObject>> _pooledUnitsByName;
    public Dictionary<int, Dictionary<int, GameObject>> _pooledUnitsByIndex;
    #endregion [UnitPool]

    #region [IconPool]
    public PoolIcon[] _poolingIcons;

    public Dictionary<string, PoolIcon> _poolingIconByName;
    public Dictionary<int, PoolIcon> _poolingIconByIndex;

    #endregion [IconPool]
    public Transform _hudRootTransform;

    #region [EffectPool]
    public PoolEffect[] _poolingEffect;

    public Dictionary<string, Dictionary<int, GameObject>> _pooledEffectByName;
    public Dictionary<string, PoolEffect> _poolingEffectByName;
    #endregion [EffectPool]

    public void LoadObjectPool()
    {
        _pooledUnitsByIndex = new Dictionary<int, Dictionary<int, GameObject>>();
        _pooledUnitsByName = new Dictionary<string, Dictionary<int, GameObject>>();
        _poolingUnitByIndex = new Dictionary<int, PoolUnit>();
        _poolingUnitByName = new Dictionary<string, PoolUnit>();
        _poolingIconByIndex = new Dictionary<int, PoolIcon>();
        _poolingIconByName = new Dictionary<string, PoolIcon>();
        _pooledEffectByName = new Dictionary<string, Dictionary<int, GameObject>>();
        _poolingEffectByName = new Dictionary<string, PoolEffect>();

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

        for (int i = 0; i < _poolingIcons.Length; i++)
        {
            _poolingIconByIndex.Add(_poolingIcons[i].index, _poolingIcons[i]);
            _poolingIconByName.Add(_poolingIcons[i].name, _poolingIcons[i]);
        }
        for (int i = 0; i < _poolingEffect.Length; i++)
        {
            Dictionary<int, GameObject> vfxDatas = new Dictionary<int, GameObject>();

            SettingPoolingEffect(i);
            for (int index = 0; index < _poolingEffect[i].CurAmount; index++)
            {
                vfxDatas.Add(index, MakeObject(_poolingEffect[i].prefab));
            }
            _pooledEffectByName.Add(_poolingEffect[i].name, vfxDatas);
        }
    }

    public PetController AddPetPool(GameObject prefab, int index, int uniqueID)
    {
        int offsetNum = 100;
        Array.Resize(ref _poolingUnits, _poolingUnits.Length + 1);
        int lastIndex = _poolingUnits.Length - 1;
        _poolingUnits[lastIndex] = new PoolUnit();
        LowBase table = Managers._table.Get(LowDataType.PetTable);
        GameObject makePet = MakeObject(prefab);
        PetController pet = makePet.GetComponent<PetController>();
        pet.InitPet(index);
        pet.Stat.UniqueID = uniqueID;

        int petIndex = table.Find("NameKr", pet.PetInfo.NameKr);
        string nameKr = table.ToStr(petIndex, "NameKr");
        _poolingUnits[lastIndex].index = petIndex + offsetNum;
        _poolingUnits[lastIndex].amount = 1;
        _poolingUnits[lastIndex].name = nameKr;
        _poolingUnits[lastIndex].prefab = pet.gameObject;
        _poolingUnits[lastIndex].type = PoolType.Pet;
        Dictionary<int, GameObject> objDatas = new Dictionary<int, GameObject>();
        objDatas.Add(0, makePet);
        pet.InitPet(petIndex);
        pet.Stat.UniqueID = uniqueID;
        if (!_pooledUnitsByIndex.ContainsKey(uniqueID))
            _pooledUnitsByIndex.Add(uniqueID, objDatas);
        GameObject hud = InstantiateAPS(1000000);
        HudController hudctrl = hud.GetComponent<HudController>();
        pet.SetHud(hudctrl, _hudRootTransform);
        hudctrl.HideHud();

        return pet;
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
    void SettingPoolingEffect(int index)
    {
        if (_poolingEffect[index].amount > 0)
            _poolingEffect[index].CurAmount = _poolingEffect[index].amount;
        else
            _poolingEffect[index].CurAmount = _defPoolAmount;

        if (_poolingEffectByName.ContainsKey(_poolingEffect[index].name) == false)
            _poolingEffectByName.Add(_poolingEffect[index].name, _poolingEffect[index]);


    }
    GameObject MakeObject(GameObject prefab, Transform parent = null)
    {
        GameObject newItem = Instantiate(prefab);

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

    public GameObject GetPooledItem(int index, Transform parent = null)
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

    GameObject GetPooledItem(string name, Transform parent = null)
    {
        if (_pooledUnitsByName.ContainsKey(name))
        {
            foreach (var obj in _pooledUnitsByName[name])
            {
                if (obj.Value.activeInHierarchy == false)
                    return obj.Value;
            }

            //眠啊 积己
            if (_canPoolExpand)
            {
                if (_poolingUnitByName.ContainsKey(name))
                {
                    GameObject tmpObj = MakeObject(_poolingUnitByName[name].prefab, parent);
                    int unitIndex = _pooledUnitsByName[name].Count;
                    int listIndex = _poolingUnitByName[name].index;
                    _pooledUnitsByName[name].Add(unitIndex, tmpObj);
                    if (_pooledUnitsByIndex.ContainsKey(listIndex))
                    {
                        unitIndex = _pooledUnitsByIndex[listIndex].Count;
                        _pooledUnitsByIndex[listIndex].Add(unitIndex, tmpObj);
                    }
                    return tmpObj;
                }
            }
        }
        else if (_pooledEffectByName.ContainsKey(name))
        {
            foreach (var obj in _pooledEffectByName[name])
            {
                if (obj.Value.activeInHierarchy == false)
                    return obj.Value;
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

