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
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (m_UICrafting == null)
            {
                GameObject ui = Instantiate(m_UICraftingPrefab);
                Canvas canvas = ui.GetComponent<Canvas>();
                canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                m_UICrafting = ui.GetComponent<UI_Craft>();
                m_UICrafting.OpenUI();
            }
            else
                m_UICrafting.OpenUI();
        }      
    }
}
