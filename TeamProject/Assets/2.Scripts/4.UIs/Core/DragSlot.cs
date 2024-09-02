using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : UI_Base
{    
    public static DragSlot _inst { get { return TSingleton<DragSlot>._inst; } }

    public bool isFromInven = true;
    public UI_Slot SlotInven;
    public UI_EquipSlot SlotEquip;

    public RectTransform _rect;
    CanvasGroup _groupCanvas;
    Image _imageItem;
   
    void Start()
    {
        Init();
    }

    public override void Init()
    {        
        _rect = GetComponent<RectTransform>();
        _imageItem = GetComponent<Image>();
        _groupCanvas = GetComponent<CanvasGroup>();
    }

    public void DragSetImage(Image icon)
    {
        _imageItem.sprite = icon.sprite;
        SetAlpha(1);
    }

    public void SetAlpha(float alpha)
    {
        Color color = _imageItem.color;
        color.a = alpha;
        _imageItem.color = color;
    }

    public void SetCanvas(bool isRaycast)
    {
        if (isRaycast == false)
        {
            _groupCanvas.alpha = 0.6f;
        }
        else
        {
            _groupCanvas.alpha = 1;
        }
        _groupCanvas.blocksRaycasts = isRaycast;
    }
}
