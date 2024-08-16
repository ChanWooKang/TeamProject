using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTableController : MonoBehaviour
{
    [SerializeField] GameObject m_uiInteractionObj;
    UI_Interaction m_interaction;
    ObjectPreview m_objPreview;
    private void Awake()
    {
        //юс╫ц
        m_objPreview = GetComponent<ObjectPreview>();
    }
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
    }
    private void OnTriggerExit(Collider other)
    {
        if (m_objPreview.IsDone && other.CompareTag("Player") && m_interaction != null)
        {
            m_interaction.CloseInteraction();
        }
    }
}
