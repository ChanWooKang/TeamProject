using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTableController : MonoBehaviour
{
    [SerializeField] GameObject m_uiInteractionObj;
    UI_Interaction m_interaction;
    [SerializeField] ObjectPreview m_objPreview;
   
    private void OnTriggerEnter(Collider other)
    {
        if (m_objPreview.IsDone && other.CompareTag("Player"))
        {
            if (m_interaction == null)
            {
                GameObject ui = Instantiate(m_uiInteractionObj);
                Canvas canvas = ui.GetComponent<Canvas>();
                canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                m_interaction = ui.GetComponent<UI_Interaction>();
                m_interaction.OpenInteraction();
            }
            else
            {
                m_interaction.OpenInteraction();
            }
        }        
    }
    private void OnTriggerStay(Collider other)
    {
        if (m_objPreview.IsDone && other.CompareTag("Player"))
        {
            if (m_interaction == null)
            {
                GameObject ui = Instantiate(m_uiInteractionObj);
                Canvas canvas = ui.GetComponent<Canvas>();
                canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                m_interaction = ui.GetComponent<UI_Interaction>();
                m_interaction.OpenInteraction();
            }
        }    
        
        if(m_objPreview.IsDone && other.CompareTag("Pet"))
        {
            // 제작중인 오브젝트가 있을 때 팻을 던지면 작업에 착수
            // m_interaction.PetWork();
        }
    }
    private void OnTriggerExit(Collider other)
    {       
        if (m_objPreview.IsDone && other.CompareTag("Player") && m_interaction != null)
        {
            m_interaction.CloseInteraction();
        }

        
    }
}
