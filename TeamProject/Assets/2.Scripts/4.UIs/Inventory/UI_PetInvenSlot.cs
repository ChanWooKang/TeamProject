using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_PetInvenSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] TextMeshProUGUI m_txtLevel;
    [SerializeField] TextMeshProUGUI m_txtLV;
    [SerializeField] TextMeshProUGUI m_txtName;
    [SerializeField] TextMeshProUGUI m_txtHp;
    [SerializeField] Image m_icon;
    [SerializeField] Slider m_hpBar;

    PetController petCtrl;
    UI_Inventory m_managerInven;
    UI_PetBoxController m_managerPetbox;

    GameObject m_draggingObject;
    RectTransform m_canvasTF;

    Vector2 m_dragOffset = Vector2.zero;
    int m_slotNum;
    void UpdateDraggingObjectPos(PointerEventData eventData)
    {
        if (m_draggingObject == null && m_managerInven != null)
            return;

        // 드래그 중인 커서의 화면 좌표 계산
        Vector3 screenPos = eventData.position + m_dragOffset;

        Vector3 newPos = Vector3.zero;

        Camera cam = eventData.pressEventCamera;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_canvasTF, screenPos, cam, out newPos))
        {
            m_draggingObject.transform.position = newPos;
            m_draggingObject.transform.rotation = m_canvasTF.rotation;
        }
    }

    public void InitSlot(UI_Inventory manager = null, PetController pet = null)
    {
        if (manager != null)
        {
            m_managerInven = manager;
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
            m_icon.sprite = null;
            m_icon.enabled = false;
            m_hpBar.gameObject.SetActive(false);
        }
    }
    public void InitSlot(int Num, UI_PetBoxController manager = null, PetController pet = null)
    {
        m_slotNum = Num; 
        if (manager != null)
        {
            m_managerPetbox = manager;
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
            m_icon.sprite = null;
            m_icon.enabled = false;
            m_hpBar.gameObject.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_icon.sprite == null)
            return;
        if (m_managerInven != null)
        {
            if (m_managerInven.m_currentPortrait != null)
                m_managerInven.m_currentPortrait.SetActive(false);
            m_managerInven.m_currentPortrait = PetEntryManager._inst.m_dicPetPortraitObject[petCtrl.PetInfo.Index];
            m_managerInven.m_currentPortrait.SetActive(true);
            m_managerInven.ClickPetSlot(petCtrl);
        }
        else
        {
            m_managerPetbox.ShowSelectedInfo(petCtrl);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
       
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_draggingObject != null)
            Destroy(m_draggingObject);
        if (!m_icon.enabled || m_icon.sprite ==  null)
            return;

        m_draggingObject = new GameObject("Dragging Object");
        m_draggingObject.transform.SetParent(m_icon.canvas.transform);
        m_draggingObject.transform.SetAsLastSibling();
        m_draggingObject.transform.localScale = Vector3.one;

        CanvasGroup canvasGroup = m_draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        Image dragIcon = m_draggingObject.AddComponent<Image>();
        dragIcon.sprite = m_icon.sprite;
        dragIcon.color = m_icon.color;
        dragIcon.material = m_icon.material;
        dragIcon.rectTransform.sizeDelta = m_icon.rectTransform.sizeDelta;

        m_canvasTF = dragIcon.canvas.transform as RectTransform;
        UpdateDraggingObjectPos(eventData);

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!m_icon.enabled || m_icon.sprite == null)
            return;
        UpdateDraggingObjectPos(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(m_draggingObject);        
    }
}
