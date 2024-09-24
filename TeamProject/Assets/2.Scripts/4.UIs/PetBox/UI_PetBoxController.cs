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
    List<UI_PetBoxSlot> m_listPetBoxSlots1;
    List<UI_PetBoxSlot> m_listPetBoxSlots2;
    List<UI_PetBoxSlot> m_listPetBoxSlots3;

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
    [SerializeField] GameObject m_noDataObj;


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
        if (m_dicPetboxSlotLists == null)
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

        m_listPetBoxSlots1 = new List<UI_PetBoxSlot>();
        m_listPetBoxSlots2 = new List<UI_PetBoxSlot>();
        m_listPetBoxSlots3 = new List<UI_PetBoxSlot>();
        for (int i = 0; i < petBoxSlots.Length; i++)
        {
            if (i < 25)
            {
                m_listPetBoxSlots1.Add(petBoxSlots[i]);
                m_listPetBoxSlots1[i].InitSlot(i);
            }
            else if (i >= 25 && i < 50)
            {
                m_listPetBoxSlots2.Add(petBoxSlots[i]);
                m_listPetBoxSlots2[i - 25].InitSlot(i);
            }
            else if (i >= 50 && i < 75)
            {
                m_listPetBoxSlots3.Add(petBoxSlots[i]);
                m_listPetBoxSlots3[i - 50].InitSlot(i);

            }
        }
        m_dicPetboxSlotLists.Add(1, m_listPetBoxSlots1);
        m_dicPetboxSlotLists.Add(2, m_listPetBoxSlots2);
        m_dicPetboxSlotLists.Add(3, m_listPetBoxSlots3);


        m_selectedPetInfoBox.SetActive(false);
        m_noDataObj.SetActive(true);
        m_boxNum = 1;
        PetEntryManager._inst.InitPetBox(this);
        m_petBoxUI.SetActive(false);
    }
    void OpenPetBox(int boxNum)
    {
        m_petBoxUI.SetActive(true);
        for (int i = 1; i < 4; i++)
        {
            for (int j = 0; j < m_dicPetboxSlotLists[i].Count; j++)
            {
                m_dicPetboxSlotLists[i][j].ActiveSlot(false);
            }
        }
        for (int i = 0; i < m_dicPetboxSlotLists[boxNum].Count; i++)
        {
            m_dicPetboxSlotLists[boxNum][i].ActiveSlot(true);
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
        for (int i = 0; i < m_dicPetboxSlotLists.Count; i++)
        {
            for (int j = 0; j < m_dicPetboxSlotLists[i + 1].Count; j++)
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
    public void ShowSelectedInfo(PetController pet)
    {
        m_noDataObj.SetActive(false);
        m_selectedPetInfoSlot.InitSlot(0, this, pet);
        m_textSelectedPetAttack.text = pet.Stat.AttackDamage.ToString();
        m_textSelectedPetWorkAbility.text = pet.PetInfo.WorkAbility.ToString();
    }
}
