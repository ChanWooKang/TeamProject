using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : TSingleton<UIManager>
{
    
    public void UIOff()
    {
        gameObject.SetActive(false);
    }
    public void UIOn()
    {
        gameObject.SetActive(true);
    }
}
