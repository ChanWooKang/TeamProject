using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairLayController : MonoBehaviour
{
    [SerializeField]
    GameObject m_prefabInfoBox;
    [SerializeField]
    RectTransform m_infoBoxRoot;

    int monLayer;
   
    bool targetMonRay;
    
    public LayerMask m_monsterTarget;

    RaycastHit m_hit;

    private void Start()
    {
        monLayer = (1 << LayerMask.NameToLayer("Monster")); //+ (1 << LayerMask.NameToLayer("Pet")));
       
    }

    private void Update()
    {
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Vector3 rayDir = Camera.main.transform.forward;
        RaycastHit hit;
        Debug.DrawRay(rayOrigin, rayDir * 100f, Color.red);
        targetMonRay = Physics.Raycast(rayOrigin, rayDir, out hit, Mathf.Infinity, monLayer);
        
        if (targetMonRay)
        {
            if (hit.collider.gameObject.TryGetComponent(out MonsterController mon))
            {
                if (mon != null)
                    mon.ShowHud();
            }            
            else if(hit.collider.gameObject.TryGetComponent(out BossCtrl boss))
            {
                if (boss != null)
                    boss.ShowHud();
            }
        }
        
    }


}
