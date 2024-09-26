using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineDatas;
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
    [SerializeField] Image m_imageSelectedPetSkill1;
    [SerializeField] Image m_imageSelectedPetSkill2;
    [SerializeField] Image m_imageSelectedPetSkill3;
    [SerializeField] Sprite m_imageRangeSkill;
    [SerializeField] Sprite m_imageBuff;
    [SerializeField] Sprite m_imageAttackBuff;
    [SerializeField] Sprite m_imageHealBuff;

    [HideInInspector] public GameObject m_currentPetPortrait;
    [SerializeField] GameObject m_petBoxUI;
    [SerializeField] GameObject m_selectedPetInfoBox;
    [SerializeField] GameObject m_noDataObj;

    bool m_isUIOpen;
    #endregion [Component]
  
    private void Update()
    {               
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
    public void PetBoxInteraction()
    {
        if (!m_petBoxUI.activeSelf)
        {
            InitUI();
            OpenPetBox(m_boxNum);
        }
        else
            ClosePetBox();
    }
    public void InitUI()
    {
        if (m_dicPetboxSlotLists == null)
        {
            InitPetBox();
            Canvas canvas = gameObject.GetComponent<Canvas>();
            canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
        }
        InitPetInven();
    }
    public void InitPetInven()
    {
        m_listEntrySlots = new List<UI_PetInvenSlot>();
        UI_PetInvenSlot[] invenSlots = m_entrySlotsRoot.GetComponentsInChildren<UI_PetInvenSlot>();
        for (int i = 0; i < invenSlots.Length; i++)
        {
            m_listEntrySlots.Add(invenSlots[i]);
            if (i < PetEntryManager._inst.m_listPetEntryCtrl.Count)
                m_listEntrySlots[i].InitSlot(i, this, PetEntryManager._inst.m_listPetEntryCtrl[i]);
            else
                m_listEntrySlots[i].InitSlot(i, this);
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
                m_listPetBoxSlots1[i].InitSlot(i, this);
            }
            else if (i >= 25 && i < 50)
            {
                m_listPetBoxSlots2.Add(petBoxSlots[i]);
                m_listPetBoxSlots2[i - 25].InitSlot(i, this);
            }
            else if (i >= 50 && i < 75)
            {
                m_listPetBoxSlots3.Add(petBoxSlots[i]);
                m_listPetBoxSlots3[i - 50].InitSlot(i, this);

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
        m_isUIOpen = true;
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
        if(m_currentPetPortrait != null)
        m_currentPetPortrait.SetActive(false);
        GameManagerEx._inst.ControlUI(m_isUIOpen, true);
    }
    public void ClosePetBox()
    {
        m_isUIOpen = false;
        m_petBoxUI.SetActive(false);
        m_noDataObj.SetActive(true);
        m_selectedPetInfoBox.SetActive(false);
        m_boxNumImage[0].enabled = true;
        m_boxNumImage[1].enabled = false;
        m_boxNumImage[2].enabled = false;
        m_boxNum = 1;
        if(m_currentPetPortrait != null)
        m_currentPetPortrait.SetActive(false);
        GameManagerEx._inst.ControlUI(m_isUIOpen, true);    
    }


    public bool GetPetIntheBox(PetController pet)
    {
        for (int i = 0; i < m_dicPetboxSlotLists.Count; i++)
        {
            for (int j = 0; j < m_dicPetboxSlotLists[i + 1].Count; j++)
            {
                if (m_dicPetboxSlotLists[i + 1][j].Pet == null)
                {
                    m_dicPetboxSlotLists[i + 1][j].InitSlot(m_dicPetboxSlotLists[i + 1][j].SlotNum,this, pet);
                    return true;
                }
            }
        }
        return false;
    }
    public void ShowSelectedInfo(PetController pet)
    {
        m_noDataObj.SetActive(false);
        m_selectedPetInfoBox.SetActive(true);
        m_selectedPetInfoSlot.InitSlot(0, this, pet);
        m_textSelectedPetAttack.text = pet.PetInfo.Damage.ToString();
        m_textSelectedPetWorkAbility.text = pet.PetInfo.WorkAbility.ToString();
        ShowPetSkills(pet.PetInfo.Index);
    }
    void ShowPetSkills(int index) // 스킬 인포창 
    {
        List<SkillInfo> skills = Managers._data.Dict_MonsterSkill[index];
        int skillsCount = skills.Count;
        switch (skillsCount)
        {
            case 1:
                m_imageSelectedPetSkill1.sprite = SetSkillType(skills, 0);
                m_textSelectedPetSkill1Name.text = skills[0].NameKr;
                m_textSelectedPetSkill1Value.text = skills[0].DamageTimes.ToString();
                m_imageSelectedPetSkill2.enabled = false;
                m_textSelectedPetSkill2Name.text = NoneSKill();
                m_textSelectedPetSkill2Value.enabled = false;
                m_imageSelectedPetSkill3.enabled = false;
                m_textSelectedPetSkill3Name.text = NoneSKill();
                m_textSelectedPetSkill3Value.enabled = false;
                break;
            case 2:
                m_imageSelectedPetSkill1.sprite = SetSkillType(skills, 0);
                m_textSelectedPetSkill1Name.text = skills[0].NameKr;
                m_textSelectedPetSkill1Value.text = skills[0].DamageTimes.ToString();
                m_imageSelectedPetSkill2.enabled = true;
                m_imageSelectedPetSkill2.sprite = SetSkillType(skills, 1);
                m_textSelectedPetSkill2Name.text = skills[1].NameKr;
                m_textSelectedPetSkill2Value.text = skills[1].DamageTimes.ToString();
                m_imageSelectedPetSkill3.enabled = false;
                m_textSelectedPetSkill3Name.text = NoneSKill();
                m_textSelectedPetSkill3Value.enabled = false;
                break;
            case 3:
                m_imageSelectedPetSkill1.sprite = SetSkillType(skills, 0);
                m_textSelectedPetSkill1Name.text = skills[0].NameKr;
                m_textSelectedPetSkill1Value.text = skills[0].DamageTimes.ToString();
                m_imageSelectedPetSkill2.enabled = true;
                m_imageSelectedPetSkill2.sprite = SetSkillType(skills, 1);
                m_textSelectedPetSkill2Name.text = skills[1].NameKr;
                m_textSelectedPetSkill2Value.text = skills[1].DamageTimes.ToString();
                m_imageSelectedPetSkill3.enabled = true;
                m_imageSelectedPetSkill3.sprite = SetSkillType(skills, 2);
                m_textSelectedPetSkill3Name.text = skills[2].NameKr;
                m_textSelectedPetSkill3Value.text = skills[2].DamageTimes.ToString();
                break;
        }
    }
    Sprite SetSkillType(List<SkillInfo> skills, int index) // 스킬 타입 셋
    {
        eAttackType type = skills[index].Type;
        switch (type)
        {
            case eAttackType.RangeAttack:
                return m_imageRangeSkill;

            case eAttackType.Buff:
                return m_imageBuff;

            case eAttackType.AttackBuff:
                return m_imageAttackBuff;

            case eAttackType.HealBuff:
                return m_imageHealBuff;

        }
        return null;
    }
    string NoneSKill()
    {
        return "스킬 없음";
    }
}
