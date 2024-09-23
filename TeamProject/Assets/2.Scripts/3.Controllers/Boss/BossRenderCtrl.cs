using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BossRenderCtrl : MonoBehaviour
{    
    Renderer[] _renders;

    //Components
    [SerializeField] Transform _model;
    [SerializeField] Transform _captureModel;
    

    public void Init()
    {       
        _renders = _model.GetComponentsInChildren<Renderer>();
    }

    public void ChangeColor(Color color)
    {
        if(_renders.Length > 0)
        {
            foreach (Renderer r in _renders)
            {
                r.material.color = color;
            }
        }        
    }

    public void ChangeLayer(eLayer layer)
    {
        gameObject.layer = (int)layer;
    }

    public void ChangeModelByCapture(bool isCapture)
    {
        _model.gameObject.SetActive(!isCapture);
        _captureModel.gameObject.SetActive(isCapture);
    }
}
