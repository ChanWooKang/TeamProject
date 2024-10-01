using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineDatas;

public class UI_Inventory : MonoBehaviour
{    

    #region [Main Component & Param]
    [Header("Main")]
    public GameObject main;
    public GameObject Slot_Parent;
    public Text InvenWeightText;
    public Image InvenWeightFill;
    public UI_StatInfo _statInfo;
    [SerializeField] GameObject[] m_tags;
    UI_Slot[] slots;
    #endregion [Main Component & Param]


    #region [PetInven Component & Param]
    [Header("PetInven")]
    public GameObject petInven;
    public GameObject petSlot_Parent;
    [HideInInspector] public GameObject m_currentPortrait;
    UI_PetInvenSlot[] m_petSlots;
    [SerializeField] GameObject m_DescBox;
    [SerializeField] GameObject m_StatusBox;
    [SerializeField] GameObject m_SkillBox;

    [SerializeField] UI_PetEnryInfoBoxController m_petEntryBox;
    [SerializeField] TextMeshProUGUI m_textPetDesc;
    [SerializeField] TextMeshProUGUI m_textPetAttack;
    [SerializeField] TextMeshProUGUI m_textPetWorkAbility;
    [SerializeField] TextMeshProUGUI m_textSkill1Name;
    [SerializeField] TextMeshProUGUI m_textSkill1Value;
    [SerializeField] Image m_iconSkill1;
    [SerializeField] TextMeshProUGUI m_textSkill2Name;
    [SerializeField] TextMeshProUGUI m_textSkill2Value;
    [SerializeField] Image m_iconSkill2;
    [SerializeField] TextMeshProUGUI m_textSkill3Name;
    [SerializeField] TextMeshProUGUI m_textSkill3Value;
    [SerializeField] Image m_iconSkill3;

    [SerializeField] Sprite m_imageRangeSkill;
    [SerializeField] Sprite m_imageBuff;
    [SerializeField] Sprite m_imageAttackBuff;
    [SerializeField] Sprite m_imageHealBuff;
    #endregion [PetInven Component & Param]

    #region [Technology Component & Param]
    [Header("Technology")]
    [SerializeField] public GameObject TechnologyBox;
    #endregion [Technology Component & Param]

    public bool isOnUI;
    public UI_Slot[] GetInvenSlots { get { return slots; } }

    public void Init()
    {
        slots = Slot_Parent.GetComponentsInChildren<UI_Slot>();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Init();
        }
        UI_ItemInfo._info.Init();
        _statInfo.Init();
        CloseUI();

    }

    public void TryOpenInventory()
    {
        if (isOnUI)
            CloseUI();
        else
            OpenUI();

        GameManagerEx._inst.ControlUI(isOnUI, true);
    }

    void OpenUI()
    {
        isOnUI = true;        
        main.SetActive(isOnUI);
        petInven.SetActive(false);
        for (int i = 0; i < m_tags.Length; i++)
        {
            m_tags[i].SetActive(true);
        }
        _statInfo.SetUI();
        m_petEntryBox.CloseUI();
    }

    void CloseUI()
    {
        isOnUI = false;
        m_petEntryBox.OpenUI();
        main.SetActive(isOnUI);
        petInven.SetActive(false);
        m_DescBox.SetActive(false);
        m_StatusBox.SetActive(false);
        m_SkillBox.SetActive(false);
        if (m_currentPortrait != null)
            m_currentPortrait.SetActive(false);
        UI_ItemInfo._info.OffInformation();
        for (int i = 0; i < m_tags.Length; i++)
        {
            m_tags[i].SetActive(false);
        }
    }

    #region [Main]
    public void SettingInvenWeight()
    {
        float weight = InventoryManager._inst.InvenWeight;
        float maxWeight = GameManagerEx._inst.playerManager._stat.CarryWeight;

        InvenWeightFill.fillAmount = weight / maxWeight;
        InvenWeightText.text = string.Format("{0:D2} / {1:D2}", (int)weight, (int)maxWeight);
    }

    public float GetItemWeights()
    {
        float weights = 0;
        foreach (UI_Slot slot in slots)
        {
            weights += slot.itemWeight;
        }
        return weights;
    }

    public void UseItemAtSlot(int slotNumber, int count)
    {
        if (slotNumber >= slots.Length)
            return;

        slots[slotNumber].SetSlotCount(-count);
    }

    public void AcquireItem(BaseItem newItem, int count = 1)
    {
        if (newItem.Type == eItemType.Gold)
            return;

        if (newItem.Type != eItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemData != null)
                {
                    if (slots[i].itemData.Index == newItem.Index)
                    {
                        if (slots[i].CheckRestSlot(newItem, count))
                        {
                            slots[i].SetSlotCount(count);
                            return;
                        }
                        else
                        {
                            int value = newItem.MaxStack - slots[i].itemCount;
                            slots[i].SetSlotCount(value);
                            count -= value;
                            continue;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData == null)
            {
                slots[i].AddItem(newItem, count);
                return;
            }
        }
    }

    public bool CheckSlotFull(BaseItem newItem, int count = 1)
    {
        if (eItemType.Equipment != newItem.Type)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemData != null)
                {
                    if (slots[i].itemData.Index == newItem.Index)
                    {
                        if (slots[i].itemCount + count <= slots[i].itemData.MaxStack)
                        {
                            return false;
                        }
                        else
                        {
                            continue;
                        }

                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemData == null)
            {
                return false;
            }
        }

        return true;
    }
    #endregion [Main]

    #region [PetInven]
    void InitPetInven()
    {
        m_petSlots = petSlot_Parent.GetComponentsInChildren<UI_PetInvenSlot>();
        for (int i = 0; i < m_petSlots.Length; i++)
        {
            if (i < PetEntryManager._inst.m_listPetEntryCtrl.Count)
                m_petSlots[i].InitSlot(this, PetEntryManager._inst.m_listPetEntryCtrl[i]);
            else
                m_petSlots[i].InitSlot();
        }

    }
    #endregion [PetInven]

    #region [Button]
    public void MainInvenTagBnt()
    {
        main.SetActive(true);
        petInven.SetActive(false);
        TechnologyBox.SetActive(false);
    }
    public void PetInvenTagBtn()
    {
        main.SetActive(false);
        petInven.SetActive(true);
        TechnologyBox.SetActive(false);
        InitPetInven();
    }
    public void TechnologyTabBtn()
    {
        main.SetActive(false);
        petInven.SetActive(true);
        TechnologyBox.SetActive(true);
    }
    public void ClickPetSlot(PetController pet)
    {
        m_textPetDesc.text = pet.PetInfo.Desc;
        m_textPetAttack.text = pet.PetInfo.Damage.ToString();
        m_textPetWorkAbility.text = pet.PetInfo.WorkAbility.ToString();
        ShowPetSkills(pet.PetInfo.Index);
        m_DescBox.SetActive(true);
        m_StatusBox.SetActive(true);
        m_SkillBox.SetActive(true);

    }
    void ShowPetSkills(int index) // 스킬 인포창 
    {
        List<SkillInfo> skills = Managers._data.Dict_MonsterSkill[index];
        int skillsCount = skills.Count;
        switch (skillsCount)
        {
            case 1:
                m_iconSkill1.sprite = SetSkillType(skills, 0);
                m_textSkill1Name.text = skills[0].NameKr;
                m_textSkill1Value.text = skills[0].DamageTimes.ToString();
                m_iconSkill2.enabled = false;
                m_textSkill2Name.text = NoneSKill();
                m_textSkill2Value.enabled = false;
                m_iconSkill3.enabled = false;
                m_textSkill3Name.text = NoneSKill();
                m_textSkill3Value.enabled = false;
                break;
            case 2:
                m_iconSkill1.sprite = SetSkillType(skills, 0);
                m_textSkill1Name.text = skills[0].NameKr;
                m_textSkill1Value.text = skills[0].DamageTimes.ToString();
                m_iconSkill2.sprite = SetSkillType(skills, 1);
                m_textSkill2Name.text = skills[1].NameKr;
                m_textSkill2Value.text = skills[1].DamageTimes.ToString();
                m_iconSkill3.enabled = false;
                m_textSkill3Name.text = NoneSKill();
                m_textSkill3Value.enabled = false;
                break;
            case 3:
                m_iconSkill1.sprite = SetSkillType(skills, 0);
                m_textSkill1Name.text = skills[0].NameKr;
                m_textSkill1Value.text = skills[0].DamageTimes.ToString();
                m_iconSkill2.sprite = SetSkillType(skills, 1);
                m_textSkill2Name.text = skills[1].NameKr;
                m_textSkill2Value.text = skills[1].DamageTimes.ToString();
                m_iconSkill3.sprite = SetSkillType(skills, 2);
                m_textSkill3Name.text = skills[2].NameKr;
                m_textSkill3Value.text = skills[2].DamageTimes.ToString();
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
    #endregion [Button]
}
