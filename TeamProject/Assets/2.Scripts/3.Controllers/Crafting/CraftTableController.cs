using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTableController : MonoBehaviour
{
    [SerializeField] GameObject m_uiInteractionObj;
    UI_Interaction m_interaction;
    [SerializeField] ObjectPreview m_objPreview;
    PetController m_petCtrl;
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
                m_interaction.OpenInteraction(this);
            }
            else
            {
                m_interaction.OpenInteraction(this);
            }
            if (m_objPreview.IsDone && other.CompareTag("Pet"))
            {
                if (m_petCtrl == null && m_interaction != null)
                {
                    m_petCtrl = other.gameObject.GetComponent<PetController>();
                    m_petCtrl.MoveToObject(gameObject.transform.position);
                    m_interaction.SetPetEntry();
                }
                else
                    return;               
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
                m_interaction.OpenInteraction(this);
            }
        }    
        
        if(m_objPreview.IsDone && other.CompareTag("Pet"))
        {
            if (m_petCtrl == null && m_interaction != null)
            {
                m_petCtrl = other.gameObject.GetComponent<PetController>();
                m_petCtrl.MoveToObject(gameObject.transform.position);
                m_interaction.SetPetEntry();
            }
            else
                return;
        }
    }
    private void OnTriggerExit(Collider other)
    {       
        if (m_objPreview.IsDone && other.CompareTag("Player") && m_interaction != null)
        {
            m_interaction.CloseInteraction();
        }
        if (m_objPreview.IsDone && other.CompareTag("Pet"))
        {            
            m_interaction.SetNoEntry();
            m_petCtrl = null;   
        }
    }
}
