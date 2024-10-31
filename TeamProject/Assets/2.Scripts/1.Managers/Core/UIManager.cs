using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : TSingleton<UIManager>
{
    [SerializeField] GameObject m_uiSoundSettingPrefab;
    [SerializeField] UI_PetEnryInfoBoxController m_uiPetEntryInfoBox;
    UI_SoundSetting m_uiSoundSetting;
      
    public UI_PetEnryInfoBoxController UIPetEntry { get { return m_uiPetEntryInfoBox; } }
    public void UIOff()
    {
        gameObject.SetActive(false);
    }
    public void UIOn()
    {
        gameObject.SetActive(true);
    }


    public void ClickSoundBtn()
    {
        if (m_uiSoundSetting == null)
        {
            GameObject ui = Instantiate(m_uiSoundSettingPrefab);
            m_uiSoundSetting = ui.GetComponent<UI_SoundSetting>();

            m_uiSoundSetting.OpenUI();
        }
        else
            m_uiSoundSetting.OpenUI();
    }
}
