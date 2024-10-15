using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;
using DefineDatas;

public class UI_InterectionManager : MonoBehaviour
{
    [SerializeField] GameObject m_uiMenuSlotPrefab;
    [SerializeField] GameObject m_uiWorkloadPrefab;
    [SerializeField] TextMeshProUGUI m_textUiName;
    StringBuilder m_sb;
    public void InitComponent(InteractionType type)
    {
        m_sb = new StringBuilder();
        switch (type)
        {
            case InteractionType.Craft:
                gameObject.AddComponent<UI_CraftDeskInteraction>();
                UI_CraftDeskInteraction Cinteraction = GetComponent<UI_CraftDeskInteraction>();
                Cinteraction.InitCraft(m_uiMenuSlotPrefab);
                m_sb.Append("제작대");
                break;
            case InteractionType.EnforceAnvil:
                gameObject.AddComponent<UI_EnforceAnvilInteraction>();
                UI_EnforceAnvilInteraction Einteraction = GetComponent<UI_EnforceAnvilInteraction>();
                Einteraction.Init();
                m_sb.Append("강화모루");
                break;
            case InteractionType.PetBall:
                gameObject.AddComponent<UI_PetBallCraftInteraction>();
                UI_PetBallCraftInteraction Pinteraction = GetComponent<UI_PetBallCraftInteraction>();
                Pinteraction.InitCraft(m_uiMenuSlotPrefab);
                m_sb.Append("펫볼제작대");
                break;
            case InteractionType.Brazier:
                gameObject.AddComponent<UI_BrazierInteraction>();
                UI_BrazierInteraction BInteraction = GetComponent<UI_BrazierInteraction>();
                BInteraction.Init();
                m_sb.Append("화로");
                break;
        }
        m_textUiName.text = m_sb.ToString();
    }

    

}
