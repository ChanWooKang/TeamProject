using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;
public class CraftTableController : MonoBehaviour
{
    [SerializeField] GameObject m_uiInteractionObj;
    UI_CraftDeskInteraction m_interaction;
    [SerializeField] ObjectPreview m_objPreview;
    PetController m_petCtrl;

    bool isClosed;

    private void Awake()
    {
        isClosed = false;
    }
    private void Update()
    {
        if (GameManagerEx._inst.UIStateValue >= 1 && m_interaction != null)
        {
            m_interaction.CloseInteraction();
        }
        if (GameManagerEx._inst.UIStateValue == 0 && isClosed && m_interaction != null)
        {
            if (!m_interaction.UIMenu.activeSelf)
                m_interaction.OpenInteractionCraftTable(this);
        }

    }
    private void LateUpdate()
    {
        if(m_petCtrl != null && !m_petCtrl.gameObject.activeSelf)
        {
            m_interaction.SetNoEntry();
            m_petCtrl = null;
        }

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
                UI_InterectionManager interactionM = ui.GetComponent<UI_InterectionManager>();
                interactionM.InitComponent(InteractionType.Craft);
                m_interaction = ui.GetComponent<UI_CraftDeskInteraction>();

                m_interaction.OpenInteractionCraftTable(this);

            }
            else
            {
                m_interaction.OpenInteractionCraftTable(this);
            }
            if (m_petCtrl == null)
                m_interaction.SetNoEntry();
            isClosed = true;
        }
        if (m_objPreview.IsDone && other.CompareTag("Pet"))
        {
            if (m_petCtrl == null && m_interaction != null)
            {
                m_petCtrl = other.gameObject.GetComponent<PetController>();
                // m_petCtrl.MoveToObject(gameObject.transform.position);
                m_interaction.SetPetEntry(m_petCtrl, this);
            }
            else
                return;
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
                UI_InterectionManager interactionM = ui.GetComponent<UI_InterectionManager>();
                interactionM.InitComponent(InteractionType.Craft);
                m_interaction = ui.GetComponent<UI_CraftDeskInteraction>();
                m_interaction.OpenInteractionCraftTable(this);
            }
            if (m_petCtrl == null)
                m_interaction.SetNoEntry();
            isClosed = true;
        }

        if (m_objPreview.IsDone && other.CompareTag("Pet"))
        {
            if (m_petCtrl == null && m_interaction != null)
            {
                m_petCtrl = other.gameObject.GetComponent<PetController>();
                // m_petCtrl.MoveToObject(gameObject.transform.position);
                m_interaction.SetPetEntry(m_petCtrl, this);
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
            isClosed = false;
        }
        if (m_objPreview.IsDone && other.CompareTag("Pet"))
        {
            m_interaction.SetNoEntry();
            m_petCtrl = null;
        }
    }

}
