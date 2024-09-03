using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DefineDatas;

public class AutoRespawnManager : MonoBehaviour
{
    public RectTransform uiHudRoot;
    SpawnManager spawn;
    Queue<int> _monsterQ;

    int _currAmount = 0;
    int _totalAmount = 0;
    int _reserveAmount = 0;

    [SerializeField] float _spawnRadius = 5.0f;
    [SerializeField] float _spawnTime = 5.0f;

    private void Start()
    {
        Init();
        spawn.OnSpawnEvent -= AddMonsterCount;
        spawn.OnSpawnEvent += AddMonsterCount;
    }

    private void Update()
    {
        while (_reserveAmount + _currAmount < _totalAmount)
        {
            StartCoroutine(ReserveSpawn());
        }
    }

    void Init()
    {        
        spawn = SpawnManager._inst;
        _monsterQ = new Queue<int>();        
        if(spawn.spawnPoints.Count > 0)
        {
            for(int i = 0; i < spawn.spawnPoints.Count; i++)
            {
                int index = spawn.spawnPoints[i].Index;
                for(int j = 0; j < spawn.spawnPoints[i].maxSpawnCount; j++)
                {
                    _monsterQ.Enqueue(index);
                }
                SetKeepMonsterCount(spawn.spawnPoints[i].maxSpawnCount);
            }
        }
    }

    void AddMonsterCount(int index, int value)
    {
        _currAmount += value;
        if (value < 0)
            _monsterQ.Enqueue(index);
    }

    void SetKeepMonsterCount(int value)
    {
        _totalAmount += value;
    }

    IEnumerator ReserveSpawn()
    {
        _reserveAmount++;
        yield return new WaitForSeconds(Random.Range(4, _spawnTime));        
        int index = _monsterQ.Dequeue();
        //Debug.Log(index);
        GameObject go = spawn.SpawnMonster(index);

        if(go.TryGetComponent(out MonsterController monster))
        {
            Vector3 randPos = new Vector3();
            if (go.TryGetComponent(out NavMeshAgent agent) == false)
                agent = go.AddComponent<NavMeshAgent>();

            while (true)
            {
                Vector3 randDir = Random.insideUnitSphere * Random.Range(0, _spawnRadius);
                randDir.y = 0;
                randPos = go.transform.position + randDir;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(randPos, path))
                    break;
            }

            //죽은애 부활 
            if (monster.isDead)
                monster.OnResurrectEvent();
            else
            {
                //최초 생성 시                  
                GameObject hud = PoolingManager._inst.InstantiateAPS(1000000, uiHudRoot);
                HudController hudctrl = hud.GetComponent<HudController>();
                monster.SetHud(hudctrl);    
                //최초 생성 시 
                // 미니맵 마크 , HUD 생성 
            }
           
            go.transform.position = randPos;
        }
        _reserveAmount--;
    }
}
