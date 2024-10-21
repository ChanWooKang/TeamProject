using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : TSingleton<UIManager>
{
    [SerializeField] UI_PetEnryInfoBoxController m_uiPetEntryInfoBox;
    public UI_PetEnryInfoBoxController UIPetEntry { get { return m_uiPetEntryInfoBox; } }
    public void UIOff()
    {
        gameObject.SetActive(false);
    }
    public void UIOn()
    {
        gameObject.SetActive(true);
    }

}
