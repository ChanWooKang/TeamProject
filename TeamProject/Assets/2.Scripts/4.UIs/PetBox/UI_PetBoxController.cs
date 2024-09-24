using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] Image[] m_boxNumImage;

    [SerializeField] GameObject m_petBoxUI;
    [SerializeField] GameObject m_selectedPetInfoBox;
    #endregion [Component]

    private void Awake()
    {
        m_petBoxUI.SetActive(false);
    }
    private void Update()
    {
        //юс╫ц 
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_petBoxUI.SetActive(true);
            InitUI();
        }
        if(m_petBoxUI.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                if (--m_boxNum < 1)
                    m_boxNum = 3;
              
            }   
            else if(Input.GetKeyDown(KeyCode.E))
            {
                if (++m_boxNum > 3)
                    m_boxNum = 1;
            }
            for (int i = 0; i < m_boxNumImage.Length; i++)
            {
                if (m_boxNum - 1 == i)
                    m_boxNumImage[i].enabled = true;
                else
                    m_boxNumImage[i].enabled = false;
            }
        }
    }
    public void InitUI()
    {
        InitPetBox();
        InitPetInven();
    }
    void InitPetInven()
    {
        m_listEntrySlots = new List<UI_PetInvenSlot>();
        UI_PetInvenSlot[] invenSlots = m_entrySlotsRoot.GetComponentsInChildren<UI_PetInvenSlot>();
        for(int i = 0;i < invenSlots.Length; i++)
        {
            m_listEntrySlots.Add(invenSlots[i]);
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
        m_boxNum = 1;

        UI_PetBoxSlot[] petBoxSlots = m_petBoxSlotsRoot.GetComponentsInChildren<UI_PetBoxSlot>();
        for(int i = 0; i < petBoxSlots.Length; i++)
        {
            m_listCurrentPetBoxSlots.Add(petBoxSlots[i]);
            m_listCurrentPetBoxSlots[i].InitSlot(i);
            m_selectedPetInfoBox.SetActive(false);
        }
        PetEntryManager._inst.InitPetBox(this);
    }
}
