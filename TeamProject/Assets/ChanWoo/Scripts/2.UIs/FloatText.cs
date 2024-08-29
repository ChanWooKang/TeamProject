using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class FloatText : MonoBehaviour
{
    public const float _lifeTime = 1.5f;
    const string _objectName = "FloatText";

    public string Text = string.Empty;
    public float FloatSpeed = 8.0f;
    Text _text;

    public static GameObject Create
        (string objName,
         bool byPlayer, 
         Vector3 pos, object value, float lifeTime = 0, Transform parent = null)
    {
        return null;
    }
}
