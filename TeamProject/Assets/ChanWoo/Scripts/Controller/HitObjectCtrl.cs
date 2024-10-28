using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public abstract class HitObjectCtrl : MonoBehaviour, IHitAble
{
    public int Index;
    protected HitObjectInfo _info;
    public List<WeaponType> AttackTypes;
    [SerializeField] Transform baseHitPoint;    
    [SerializeField] protected float _hp;

    protected bool isDead;
    protected bool isInit = false;

    public virtual void Init()
    {
        _info = Managers._data.Dict_HitObject[Index];
        _hp = _info.HP;

        isDead = false;
        isInit = true;
    }

    void GetItemByRandom()
    {
        int randValue = Random.Range(0, 100);
        Debug.Log(_info.RewardIndex);
        if (randValue < InventoryManager._inst.Dict_Material[_info.RewardIndex].Rate)
        {
            GetItem(_info.RewardIndex);
        }        
    }

    void GetItem(int dataIndex, int cnt = 1)
    {
        BaseItem item = InventoryManager._inst.GetItemData(dataIndex);

        if (InventoryManager._inst.CheckSlot(item, cnt))
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

    public bool CheckAttackType(WeaponType type)
    {
        return AttackTypes.Contains(type);
    }

    public abstract void OnDamage();
   

    public void OnDamage(float damage, Transform attacker)
    {
        Vector3 hitPoint = transform.position + baseHitPoint.position;
        OnDamage(damage, attacker, hitPoint);
    }

    public virtual void OnDamage(float damage, Transform attacker, Vector3 hitPoint)
    {
        if (isDead)
            return;

        isDead = GetHitDamage(damage);
        DamageTextManager._inst.ShowDamageText(hitPoint, damage);
        OnDamage();
        GetItemByRandom();
        
    }

    //IEnumerator OnDamageEvent()
    //{
    //    if (isDead)
    //    {
    //        yield return new WaitForSeconds(0.2f);
    //        OnDeadEvent();
    //        yield break;
    //    }

    //    yield return new WaitForSeconds(0.3f);
    //}

    public virtual void OnDeadEvent()
    {
        int spawnCount = _info.RewardCount;
        SpawnManager._inst.SpawnItem(_info.RewardIndex, transform,spawnCount);        
    }
}
