using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_PetInvenSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IDropHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] TextMeshProUGUI m_txtLevel;
    [SerializeField] TextMeshProUGUI m_txtLV;
    [SerializeField] TextMeshProUGUI m_txtName;
    [SerializeField] TextMeshProUGUI m_txtHp;
    [SerializeField] Image m_icon;
    [SerializeField] Slider m_hpBar;

    PetController m_petCtrl;
    UI_Inventory m_managerInven;
    UI_PetBoxController m_managerPetbox;

    
    GameObject m_draggingObject;
    RectTransform m_canvasTF;

    [HideInInspector] public PetController Pet { get { return m_petCtrl; } }
    [HideInInspector] public Image Icon { get { return m_icon; } }
    [HideInInspector] public UI_Inventory ManagerInven { get { return m_managerInven; } }
    [HideInInspector] public UI_PetBoxController ManagerPetBox { get { return m_managerPetbox; } }
    Vector2 m_dragOffset = Vector2.zero;
    int m_slotNum;
    public int SlotNum { get { return m_slotNum; } }
    void UpdateDraggingObjectPos(PointerEventData eventData)
    {
        if (m_draggingObject == null && m_managerInven != null)
            return;

        // �巡�� ���� Ŀ���� ȭ�� ��ǥ ���
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
            m_petCtrl = pet;
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
        m_managerPetbox = manager;
        if (pet != null)
        {            
            m_txtLevel.enabled = true;
            m_txtLV.enabled = true;
            m_txtName.enabled = true;
            m_txtHp.enabled = true;
            m_icon.enabled = true;
            m_hpBar.gameObject.SetActive(true);
            m_petCtrl = pet;
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
        if (m_icon.enabled == false)
            return;
        if (m_managerInven != null)
        {
            if (m_managerInven.m_currentPortrait != null)
                m_managerInven.m_currentPortrait.SetActive(false);
            m_managerInven.m_currentPortrait = PetEntryManager._inst.m_dicPetPortraitObject[m_petCtrl.PetInfo.Index];
            m_managerInven.m_currentPortrait.SetActive(true);
            m_managerInven.ClickPetSlot(m_petCtrl);
        }
        else if (m_managerPetbox != null)
        {
            m_managerPetbox.ShowSelectedInfo(m_petCtrl);
            m_managerPetbox.m_currentPetPortrait = PetEntryManager._inst.m_dicPetPortraitObject[m_petCtrl.PetInfo.Index];
            m_managerPetbox.m_currentPetPortrait.SetActive(true);
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
        if (eventData.pointerDrag.transform.TryGetComponent(out UI_PetBoxSlot slot))
        {
            m_petCtrl = slot.Pet;
            slot.SwapSlot(this);
            m_icon.enabled = true;
            m_icon.sprite = slot.Icon.sprite;
            PetEntryManager._inst.m_listPetEntryCtrl.Add(m_petCtrl);
            m_managerPetbox.InitPetInven();
        }
        else
            return;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_draggingObject != null)
            Destroy(m_draggingObject);
        if (!m_icon.enabled || m_managerPetbox == null)
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
        if (m_managerPetbox == null)
            return;
        UpdateDraggingObjectPos(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(m_draggingObject);
    }
    public void ResetSlot()
    {

    }
}
