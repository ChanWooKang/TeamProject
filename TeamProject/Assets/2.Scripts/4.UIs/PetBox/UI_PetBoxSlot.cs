using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PetBoxSlot : MonoBehaviour
{
    [SerializeField] Image m_petIcon;
    PetController m_petCtrl;
    int m_slotNum;


    public void InitSlot(int num, PetController pet = null)
    {
        m_slotNum = num;
        if (pet != null)
        {
            m_petCtrl = pet;
            m_petIcon.enabled = true;            
        }
        else
        {
            m_petCtrl = null;
            m_petIcon.enabled = false;
        }
    }
}
