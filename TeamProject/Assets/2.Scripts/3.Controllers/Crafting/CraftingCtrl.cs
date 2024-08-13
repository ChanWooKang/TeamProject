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
                m_UICrafting = ui.GetComponent<UI_Craft>();
            }
            else
                m_UICrafting.OpenUI();
        }      
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
