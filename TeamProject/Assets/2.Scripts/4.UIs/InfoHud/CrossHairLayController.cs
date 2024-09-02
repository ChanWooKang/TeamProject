using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairLayController : MonoBehaviour
{
    [SerializeField]
    GameObject m_prefabInfoBox;
    [SerializeField]
    RectTransform m_infoBoxRoot;
     
    
    public LayerMask m_monsterTarget;
    RaycastHit m_hit;

    private void Update()
    {
        Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Vector3 rayDir = Camera.main.transform.forward;

        //if(Physics.Raycast(rayOrigin, rayDir, out m_hit, Mathf.Infinity, m_monsterTarget))
        //{
        //    GameObject infoBox = Instantiate(m_prefabInfoBox, m_infoBoxRoot);
        //    Vector3 targetPos = Camera.main.WorldToScreenPoint(m_hit.transform.position);
        //    infoBox.transform.position = targetPos;
        //}
    }

    
}
