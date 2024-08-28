using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DefineDatas;

public class SpawnManager : MonoBehaviour
{
    static SpawnManager _uniqueInstance;
    public static SpawnManager _inst { get { return _uniqueInstance; } }

    public Action<int, int> OnSpawnEvent;
    public List<SpawnPoint> spawnPoints;

    public int monsterOffsetNumber = 1000;
    PoolingManager pool;

    private void Awake()
    {
        _uniqueInstance = this;
    }

    private void Start()
    {
        pool = PoolingManager._inst;
    }

    int GetMonsterIndex(GameObject go)
    {
        int index = 0;
        if(go.TryGetComponent(out MonsterController monster))
        {
            index = monster.Index;
        }

        return index;
    }

    public GameObject SpawnMonster(int index)
    {
        Transform spawnTransform = null;

        for(int i = 0; i < spawnPoints.Count; i++)
        {
            if(index == spawnPoints[i].Index)
            {
                spawnTransform = spawnPoints[i].parent;
            }
        }
        if (spawnPoints == null)
        {
            Debug.Log($"Spawn Point�� ��ġ�ϴ� Index ���� �ƴմϴ�. : {index}");
            return null;
        }

        GameObject go = pool.InstantiateAPS(index, spawnTransform.position, spawnTransform.rotation, Vector3.one);

        if(go.TryGetComponent(out MonsterController monster) == false)
        {
            Debug.Log($"�ش��ϴ� Index({index})���� MonsterController Script�� �������� �ʽ��ϴ�.");
            return null;
        }
        monster._movement._defPos = spawnTransform.position;

        OnSpawnEvent?.Invoke(index, 1);
        return go;
    }

    public void MonsterDespawn(GameObject go)
    {
        int index = GetMonsterIndex(go);

        if (index == 0)
            return;

        OnSpawnEvent?.Invoke(index, -1);
        go.DestroyAPS();
    }
}
