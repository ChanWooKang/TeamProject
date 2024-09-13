using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_PetInvenSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI m_txtLevel;
    [SerializeField] TextMeshProUGUI m_txtLV;
    [SerializeField] TextMeshProUGUI m_txtName;
    [SerializeField] TextMeshProUGUI m_txtHp;
    [SerializeField] Image m_icon;
    [SerializeField] Slider m_hpBar;

    PetController petCtrl;
    public void InitSlot(PetController pet = null)
    {
        if(pet != null)
        {
            m_txtLevel.enabled = true;
            m_txtLV.enabled = true;
            m_txtName.enabled = true;
            m_txtHp.enabled = true;
            m_icon.enabled = true;
            m_hpBar.gameObject.SetActive(true);
            petCtrl = pet;
            m_txtLevel.text = pet.PetLevel.ToString();
            m_txtName.text = pet.PetInfo.NameKr;
            m_txtHp.text = pet.Stat.HP + "/" + pet.Stat.MaxHP;
            m_hpBar.value = pet.Stat.HP / pet.Stat.MaxHP;
        }
        else
        {
            m_txtLevel.enabled = false;
            m_txtLV.enabled = false;
            m_txtName.enabled = false;
            m_txtHp.enabled = false;
            m_icon.enabled = false;
            m_hpBar.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }

}
