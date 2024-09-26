using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBoxAnimEventController : MonoBehaviour
{
    UI_PetBoxController m_uiPetBoxCtrl;
    public void InitAnimEventController(UI_PetBoxController ctrl)
    {
        m_uiPetBoxCtrl = ctrl;
    }
     void Anim_BoxInteraction()
    {
        m_uiPetBoxCtrl.PetBoxInteraction();
    }
}
