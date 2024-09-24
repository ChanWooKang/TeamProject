using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI_PetBoxController : MonoBehaviour
{
    #region [Component]
    int m_boxNum;

    [SerializeField] Transform m_entrySlotsRoot;
    [SerializeField] Transform m_petBoxSlotsRoot;

    List<UI_PetInvenSlot> m_listEntrySlots;
    Dictionary<int, List<UI_PetBoxSlot>> m_dicPetboxSlotLists; // Dic<boxNum, List<petSlot>>
    List<UI_PetBoxSlot> m_listCurrentPetBoxSlots;

    [SerializeField] UI_PetInvenSlot m_selectedPetInfoSlot;

    [SerializeField] TextMeshProUGUI m_textBoxNum;

    [SerializeField] TextMeshProUGUI m_textSelectedPetAttack;
    [SerializeField] TextMeshProUGUI m_textSelectedPetWorkAbility;
    [SerializeField] TextMeshProUGUI m_textSelectedPetSkill1Name;
    [SerializeField] TextMeshProUGUI m_textSelectedPetSkill1Value;
    [SerializeField] TextMeshProUGUI m_textSelectedPetSkill2Name;
    [SerializeField] TextMeshProUGUI m_textSelectedPetSkill2Value;
    [SerializeField] TextMeshProUGUI m_textSelectedPetSkill3Name;
    [SerializeField] TextMeshProUGUI m_textSelectedPetSkill3Value;
    #endregion [Component]

    public void InitUI()
    {
        InitPetBox();
    }
    void InitPetInven()
    {
        m_listEntrySlots = new List<UI_PetInvenSlot>();
        UI_PetInvenSlot[] invenSlots = m_entrySlotsRoot.GetComponentsInChildren<UI_PetInvenSlot>();
        for(int i = 0;i < invenSlots.Length; i++)
        {
            m_listEntrySlots.Add(invenSlots[i]);
        }
        for (int i = 0; i < m_listEntrySlots.Count; i++)
        {
            if (i < PetEntryManager._inst.m_listPetEntryCtrl.Count)
                m_listEntrySlots[i].InitSlot(i, this, PetEntryManager._inst.m_listPetEntryCtrl[i]);
            else
                m_listEntrySlots[i].InitSlot(i);
        }
    }

    void InitPetBox()
    {
        m_dicPetboxSlotLists = new Dictionary<int, List<UI_PetBoxSlot>>();
        m_listCurrentPetBoxSlots = new List<UI_PetBoxSlot>();

        
    }
}
