using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTextManager : TSingleton<DamageTextManager>
{
    public Transform damageUI;
    public float destoryTime = 1.0f;

    public void ShowDamageText(Vector3 pos, float damage)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);

        GameObject go = PoolingManager._inst.InstantiateAPS("DamageText", damageUI);
        go.transform.position = screenPos;

        go.GetComponent<DamageText>().Configure(damage, destoryTime);
        

        Canvas canvas = go.GetComponentInParent<Canvas>();
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 10;        
    }
}
