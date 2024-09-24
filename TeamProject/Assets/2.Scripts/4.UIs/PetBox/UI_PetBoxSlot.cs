using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UI_PetBoxSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Image m_petIcon;
    PetController m_petCtrl;

    GameObject m_draggingObject;
    RectTransform m_canvasTF;

    Vector2 m_dragOffset = Vector2.zero;
    int m_slotNum;

    public int SlotNum { get { return m_slotNum; } }
    public PetController Pet { get { return m_petCtrl; } }

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
    public void ActiveSlot(bool isActive)
    {
        if (isActive)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
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
    public void OnDrop(PointerEventData eventData)
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_draggingObject != null)
            Destroy(m_draggingObject);
        if (!m_petIcon.enabled)
            return;

        m_draggingObject = new GameObject("Dragging Object");
        m_draggingObject.transform.SetParent(m_petIcon.canvas.transform);
        m_draggingObject.transform.SetAsLastSibling();
        m_draggingObject.transform.localScale = Vector3.one;

        CanvasGroup canvasGroup = m_draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        Image dragIcon = m_draggingObject.AddComponent<Image>();
        dragIcon.sprite = m_petIcon.sprite;
        dragIcon.color = m_petIcon.color;
        dragIcon.material = m_petIcon.material;
        dragIcon.rectTransform.sizeDelta = m_petIcon.rectTransform.sizeDelta;

        m_canvasTF = dragIcon.canvas.transform as RectTransform;
        UpdateDraggingObjectPos(eventData);

    }
    public void OnDrag(PointerEventData eventData)
    {
        UpdateDraggingObjectPos(eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(m_draggingObject);
    }
    void UpdateDraggingObjectPos(PointerEventData eventData)
    {
        if (m_draggingObject == null)
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
}
