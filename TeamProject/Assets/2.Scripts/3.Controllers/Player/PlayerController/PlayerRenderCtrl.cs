using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderCtrl : MonoBehaviour
{
    PlayerCtrl _manager;

    [Header("Render Parent")]
    [SerializeField] Transform _parent;
    [SerializeField] Color _baseColor;

    Renderer[] _renders;

    public void Init(PlayerCtrl manager)
    {
        _manager = manager;        
        _renders = _parent.GetComponentsInChildren<Renderer>();
    }


    public void ChangeColor(Color color)
    {
        foreach(Renderer render in _renders)
        {
            if(render.materials.Length > 1)
            {
                for (int i = 0; i < render.materials.Length; i++)
                    render.materials[i].color = color;
            }
            else
            {
                render.material.color = color;
            }
        }
    }

    public void ReturnColor()
    {
        ChangeColor(_baseColor);
    }
}
