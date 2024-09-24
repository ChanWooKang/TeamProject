using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DefineDatas;

public class SpawnManager : TSingleton<SpawnManager>
{    
    public Action<int, int> OnSpawnEvent;
    public List<SpawnPoint> spawnPoints;
    
    PoolingManager pool;

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
        else if (go.TryGetComponent(out BossCtrl boss))
        {
            index = boss.Index;                        
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
            Debug.Log($"Spawn Point에 일치하는 Index 값이 아닙니다. : {index}");
            return null;
        }

        GameObject go = pool.InstantiateAPS(index, spawnTransform.position, spawnTransform.rotation, Vector3.one);
        if(go.TryGetComponent(out MonsterController monster))
        {
            monster._movement._defPos = spawnTransform.position;
        }
        else
        {
            if(go.TryGetComponent(out BossCtrl boss))
            {
                boss._move._defPos = spawnTransform.position;
            }
            else
            {
                Debug.Log($"해당하는 Index({index})값에 MonsterController Script가 존재하지 않습니다.");
                return null;
            }            
        }        

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
