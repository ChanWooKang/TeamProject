using System.Text;
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
        //임시 
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!m_petBoxUI.activeSelf)
            {
                InitUI();
                OpenPetBox(m_boxNum);
            }
            else
                ClosePetBox();
        }

        if (m_petBoxUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (--m_boxNum < 1)
                        m_boxNum = 3;
                    OpenPetBox(m_boxNum);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    if (++m_boxNum > 3)
                        m_boxNum = 1;
                    OpenPetBox(m_boxNum);
                }
                for (int i = 0; i < m_boxNumImage.Length; i++)
                {
                    if (m_boxNum - 1 == i)
                        m_boxNumImage[i].enabled = true;
                    else
                        m_boxNumImage[i].enabled = false;

                    m_textBoxNum.text = "상자 " + m_boxNum.ToString();
                }
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
        for (int i = 0; i < invenSlots.Length; i++)
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
       
        m_boxNum = 1;

        UI_PetBoxSlot[] petBoxSlots = m_petBoxSlotsRoot.GetComponentsInChildren<UI_PetBoxSlot>();
        for (int j = 0; j < 3; j++)
        {
            m_listCurrentPetBoxSlots = new List<UI_PetBoxSlot>();
            for (int i = 0; i < petBoxSlots.Length; i++)
            {
                m_listCurrentPetBoxSlots.Add(petBoxSlots[i]);
                m_listCurrentPetBoxSlots[i].InitSlot(i);
            }
            m_dicPetboxSlotLists.Add(m_boxNum, m_listCurrentPetBoxSlots);
            ++m_boxNum;
        }
        m_selectedPetInfoBox.SetActive(false);
        m_boxNum = 1;
        PetEntryManager._inst.InitPetBox(this);
        m_petBoxUI.SetActive(false);
    }
    void OpenPetBox(int boxNum)
    {
        m_petBoxUI.SetActive(true);
        m_listCurrentPetBoxSlots = m_dicPetboxSlotLists[1];
        UI_PetBoxSlot[] petBoxSlots = m_petBoxSlotsRoot.GetComponentsInChildren<UI_PetBoxSlot>();
        for (int i = 0; i < petBoxSlots.Length; i++)
        {
            petBoxSlots[i] = m_listCurrentPetBoxSlots[i];
        }
    }
    void ClosePetBox()
    {
        m_petBoxUI.SetActive(false);
        m_boxNumImage[0].enabled = true;
        m_boxNumImage[1].enabled = false;
        m_boxNumImage[2].enabled = false;
        m_boxNum = 1;
    }

    public bool GetPetIntheBox(PetController pet)
    {
        for(int i = 0;i < m_dicPetboxSlotLists.Count; i++)
        {
            for(int j = 0; j < m_dicPetboxSlotLists[i+1].Count; j++)
            {
                if (m_dicPetboxSlotLists[i + 1][j].Pet == null)
                {
                    m_dicPetboxSlotLists[i + 1][j].InitSlot(m_dicPetboxSlotLists[i + 1][j].SlotNum, pet);
                    return true;
                }
            }
        }
        return false;
    }
}
