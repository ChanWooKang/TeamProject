using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class BrazierController : MonoBehaviour
{
    [SerializeField] GameObject m_uiInteractionObj;
    [SerializeField] ObjectPreview m_objPreview;
    UI_BrazierInteraction m_uiInteraction;

    bool isClosed;

    private void Awake()
    {
        isClosed = false;
    }
    private void Update()
    {
        if (GameManagerEx._inst.UIStateValue >= 1 && m_uiInteraction != null)
        {
            m_uiInteraction.CloseEnforce();
        }
        if (GameManagerEx._inst.UIStateValue == 0 && isClosed && m_uiInteraction != null)
        {
            //if (!m_uiInteraction.UIEnforce.activeSelf)
            //    m_uiInteraction.OpenEnforce(this);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (m_objPreview.IsDone && other.CompareTag("Player"))
        {
            if (m_uiInteraction == null)
            {
                GameObject ui = Instantiate(m_uiInteractionObj);
                Canvas canvas = ui.GetComponent<Canvas>();
                canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                UI_InterectionManager interactionM = ui.GetComponent<UI_InterectionManager>();
                interactionM.InitComponent(InteractionType.Brazier);
                m_uiInteraction = ui.GetComponent<UI_BrazierInteraction>();

                m_uiInteraction.OpenEnforce(this);

            }
            else
            {
                m_uiInteraction.OpenEnforce(this);
            }

            isClosed = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (m_objPreview.IsDone && other.CompareTag("Player"))
        {
            if (m_uiInteraction == null)
            {
                GameObject ui = Instantiate(m_uiInteractionObj);
                Canvas canvas = ui.GetComponent<Canvas>();
                canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                UI_InterectionManager interactionM = ui.GetComponent<UI_InterectionManager>();
                interactionM.InitComponent(InteractionType.Brazier);
                m_uiInteraction = ui.GetComponent<UI_BrazierInteraction>();

                m_uiInteraction.OpenEnforce(this);

            }
           
            isClosed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_objPreview.IsDone && other.CompareTag("Player") && m_uiInteraction != null)
        {
            m_uiInteraction.CloseEnforce();
            isClosed = false;
        }
    }

}
