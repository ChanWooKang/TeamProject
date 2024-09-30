using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingCtrl : MonoBehaviour
{
    [SerializeField] GameObject m_UICraftingPrefab;
    UI_Craft m_UICrafting;
    // Update is called once per frame
    void Update()
    {
        OpenCraftingMenu();
    }

    void OpenCraftingMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_UICrafting != null)
                m_UICrafting.CloseUI();
        }
    }
}
