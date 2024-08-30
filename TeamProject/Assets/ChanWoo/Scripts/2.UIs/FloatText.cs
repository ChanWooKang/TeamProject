using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class FloatText : MonoBehaviour
{
    public const float _lifeTime = 1.5f;
    const string _objectName = "FloatText";

    public string TextValue = string.Empty;
    public float FloatSpeed = 8.0f;
    Text _text;


    public void Init()
    {
        _text = GetComponentInChildren<Text>();
    }

    public static GameObject Create(
        string objName,       
        Vector3 pos,
        object value,                
        Transform parent = null)
    {
        GameObject go = 
            PoolingManager._inst.InstantiateAPS(_objectName, pos,
                                                Quaternion.identity, Vector3.one * 0.6f, parent);
        go.name = objName;
        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;        
        go.GetComponent<FloatText>().Configure(value, _lifeTime);
        if (parent != null)
            go.transform.SetParent(parent);
        return go;
    }

    public void Configure(object value, float lifeTime = 0)
    {
        if (_text == null)
            Init();

        TextValue = $"{value}";

        StartCoroutine(Floatting());
        if(lifeTime > 0)
        {
            Invoke("Despawn", lifeTime);
        }
    }

    IEnumerator Floatting()
    {
        while (true)
        {
            _text.text = TextValue;
            transform.rotation = Camera.main.transform.rotation;
            transform.Translate(new Vector3(0, FloatSpeed * Time.deltaTime, 0));
            yield return null;
        }
    }

    void Despawn()
    {
        StopCoroutine(Floatting());
        gameObject.DestroyAPS();
    }
}
