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

    

    GameObject SpawnObject(int index, Transform spawnTr)
    {
        Vector3 pos = spawnTr.position;
        pos.y += 0.5f;
        Quaternion rot = Quaternion.Euler(Vector3.zero);
        GameObject go = pool.InstantiateAPS(index, pos, rot, Vector3.one);
        return go;
    }

    public void SpawnItem(int index, Transform parent ,int count = 1)
    {
        GameObject go = SpawnObject(index, parent);
        if (go != null)
        {
            if(go.TryGetComponent(out ItemCtrl item))
            {
                item.Spawn(count);
            }
            else
            {
                Destroy(go);
            }
        }        
    }


    public void SpawnItemWithRate(int[] itemIndexs, int rewardCount, Transform parent)
    {
        int[] rates = new int[itemIndexs.Length];
        int maxWeight = 0;        

        if(rates.Length > 0)
        {
            for (int i = 0; i < rates.Length; i++)
            {
                int rate = 0;
                if (InventoryManager._inst.Dict_Material.ContainsKey(itemIndexs[i]))
                {
                    rate = InventoryManager._inst.Dict_Material[itemIndexs[i]].Rate;
                }
                else
                {
                    //오류
                    rate = 0;
                    Debug.Log("저장 되어 있지 않은 인덱스가 들어갔습니다.");
                }
                rates[i] = rate;
                maxWeight += rate;
            }

            for(int i = 0; i < rewardCount; i++)
            {
                int index = RandomPickMaterial(rates, maxWeight);
                SpawnItem(itemIndexs[index], parent);
            }            
        }

    }

    int RandomPickMaterial(int[] rates, int maxWeight)
    {
        int pivot = UnityEngine.Random.Range(0, maxWeight);
        int cumulativeWeight = 0;
        int index = 0;

        for (int i = 0; i < rates.Length; i++)
        {
            cumulativeWeight += rates[i];
            if (pivot <= cumulativeWeight)
            {
                index = i;
                break;
            }
        }

        return index;
    }
}
