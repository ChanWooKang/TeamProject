using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public abstract class UI_Base : MonoBehaviour
{
    public abstract void Init();
    
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
   
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);   

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Utilitys.FindChild(gameObject, names[i], true);
            else
                objects[i] = Utilitys.FindChild<T>(gameObject, names[i], true);

            //if (objects[i] == null)
            //Debug.Log($"Failed To Bind : ({names[i]})");
        }
    }

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
        {
            //Debug.Log("Failed to Get");
            return null;
        }

        return objects[index] as T;
    }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, MouseEvent type = MouseEvent.Click)
    {
        UI_EventHandler evt = Utilitys.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case MouseEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
        }        
    }

    protected GameObject GetObject(int index) { return Get<GameObject>(index); }
    protected Text GetText(int index) { return Get<Text>(index); }
    protected Image GetImage(int index) { return Get<Image>(index); }
}
