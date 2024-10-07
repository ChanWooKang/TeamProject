using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDatas;

public class UI_InterectionManager : MonoBehaviour
{
    [SerializeField] GameObject m_uiMenuSlotPrefab;
    [SerializeField] GameObject m_uiWorkloadPrefab;
    public void InitComponent(InteractionType type)
    {
        switch (type)
        {
            case InteractionType.Craft:
                gameObject.AddComponent<UI_CraftDeskInteraction>();
                UI_CraftDeskInteraction Cinteraction = GetComponent<UI_CraftDeskInteraction>();
                Cinteraction.Init(m_uiMenuSlotPrefab, m_uiWorkloadPrefab);
                break;
            case InteractionType.EnforceAnvil:
                gameObject.AddComponent<UI_EnforceAnvilInteraction>();
                UI_EnforceAnvilInteraction Einteraction = GetComponent<UI_EnforceAnvilInteraction>();
                Einteraction.Init(m_uiMenuSlotPrefab, m_uiWorkloadPrefab);
                break;
            case InteractionType.PetBall:
                gameObject.AddComponent<UI_PetBallCraftInteraction>();
                UI_PetBallCraftInteraction Pinteraction = GetComponent<UI_PetBallCraftInteraction>();
                Pinteraction.Init(m_uiMenuSlotPrefab, m_uiWorkloadPrefab);

                break;
        }

    }

}
