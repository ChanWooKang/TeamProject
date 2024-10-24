using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class PlayerColliderCtrl : MonoBehaviour
{
    PlayerCtrl _manager;

    [SerializeField, Range(0.2f, 0.5f)] float hitRateTime;
    float hitCntTime;   
    float hitDamage;
    

    public void Init(PlayerCtrl manager)
    {
        _manager = manager;
        hitCntTime = 0;
        hitDamage = 0;
    }

    public void OnDamage(float damage)
    {
        _manager.OnDamage(damage);
    } 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interact"))
        {
            if(other.TryGetComponent(out ObjectData data))
            {
                if(data.type == eInteractType.Item)
                {
                    _manager.SetRecognizeObject(other.gameObject);
                    if(other.TryGetComponent(out ItemCtrl item))
                    {
                        if (item.isRootAble)
                        {
                            if (item.Root())
                                _manager.SetRecognizeObject();
                        }                        
                    }                    
                }
            }
        }

        if (other.CompareTag("MonsterSkill"))
        {
            if (other.TryGetComponent(out BaseSkill skill))
            {
                OnDamage(skill.Damage);
                other.gameObject.DestroyAPS();
            }
        }

        if (other.CompareTag("Fire"))
        {
            Debug.Log("������");
            if(other.TryGetComponent(out FlameCtrl flame))
            {
                hitCntTime = 1;
                hitDamage = flame.Damage;
            }            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            hitCntTime += Time.deltaTime;
            if (hitCntTime > hitRateTime)
            {
                OnDamage(hitDamage);
                hitCntTime = 0;
            }
                
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            hitCntTime = 0;
            hitDamage = 0;
        }

        if(_manager.RecognizeObject != null)
        {
            if(other.gameObject == _manager.RecognizeObject)
            {
                _manager.SetRecognizeObject();
            }
        }
    }
}
