using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class HitObjectCtrl : MonoBehaviour, IHitAble
{
    public int Index;
    HitObjectInfo _info;
    [SerializeField] Transform baseHitPoint;    
    [SerializeField] float _hp;

    bool isDead;

    Coroutine DamageCoroutine = null;


    void Start()
    {
        Init();
    }

    public void Init()
    {
        _info = Managers._data.Dict_HitObject[Index];
        _hp = _info.HP;

        isDead = false;
        DamageCoroutine = null;
    }

    void GetItemByRandom()
    {
        int randValue = Random.Range(0, 100);
        if (randValue < InventoryManager._inst.Dict_Material[_info.RewardIndex].Rate)
        {
            GetItem(_info.RewardIndex);
        }        
    }

    void GetItem(int dataIndex, int cnt = 1)
    {
        BaseItem item = InventoryManager._inst.GetItemData(dataIndex);

        if (InventoryManager._inst.CheckSlot(item, cnt) == false)
        {
            InventoryManager._inst.AddInvenItem(item, cnt);
        }
    }

    bool GetHitDamage(float damage)
    {
        if(_hp > damage)
        {
            _hp -= damage;
            return false;
        }
        else
        {
            _hp = 0;
            return true;
        }
    }

    public void OnDamage(float damage, Transform attacker)
    {
        Vector3 hitPoint = transform.position + baseHitPoint.position;
        OnDamage(damage, attacker, hitPoint);
    }

    public void OnDamage(float damage, Transform attacker, Vector3 hitPoint)
    {
        if (isDead)
            return;

        isDead = GetHitDamage(damage);
        DamageTextManager._inst.ShowDamageText(hitPoint, damage);
        GetItemByRandom();
        if (DamageCoroutine != null)
            StopCoroutine(DamageCoroutine);
        DamageCoroutine = StartCoroutine(OnDamageEvent());
    }

    IEnumerator OnDamageEvent()
    {
        if (isDead)
        {
            yield return new WaitForSeconds(0.2f);
            OnDeadEvent();
            yield break;
        }

        yield return new WaitForSeconds(0.3f);
    }

    void OnDeadEvent()
    {
        int spawnCount = _info.RewardCount;
        SpawnManager._inst.SpawnItem(_info.RewardIndex, transform,spawnCount);
        //for (int i = 0; i < spawnCount; i++) 
        //{
        //    SpawnManager._inst.SpawnItem(_info.RewardIndex, transform);
        //}
        gameObject.SetActive(false);
    }
}
