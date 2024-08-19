using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DefineDatas;

public class UI_MenuSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] Image m_icon;
    [SerializeField] RectTransform[] m_infoBoxPoses;
    [SerializeField] GameObject m_infoBoxPrefab;
    UI_InfoBox m_uiInfoBox;
    UI_Interaction m_uiInteraction;
    WeaponInfo m_wpInfo;
    int m_x;
    int m_y;
    public void InitSlot(int num, int x, int y, UI_Interaction interation)
    {
        m_uiInteraction = interation;
        LowBase weaponTable = Managers._table.Get(LowDataType.WeaponTable);
        int index = 200 + num;
        string nameEn = weaponTable.ToStr(index, "NameEn");
        string nameKr = weaponTable.ToStr(index, "NameKr");
        string desc = weaponTable.ToStr(index, "Desc");
        string spriteName = weaponTable.ToStr(index, "SpriteName");
        string materialsIndexStr = weaponTable.ToStr(index, "Materials");
        string[] materialsIndexStrArray = materialsIndexStr.Split('/');
        int[] materialsIndexArray = new int[materialsIndexStrArray.Length];
        for (int i = 0; i < materialsIndexStrArray.Length; i++)
        {
            if (int.TryParse(materialsIndexStrArray[i], out int number))
            {
                materialsIndexArray[i] = number;
            }
        }
        string materialsCost = weaponTable.ToStr(index, "MaterialsCost");
        string[] materialsCostStrArray = materialsCost.Split('/');
        int[] materialsCostArray = new int[materialsCostStrArray.Length];
        for (int i = 0; i < materialsCostArray.Length; i++)
        {
            if (int.TryParse(materialsCostStrArray[i], out int number))
            {
                materialsCostArray[i] = number;
            }
        }
        float damage = weaponTable.ToFloat(index, "Damage");
        float weight = weaponTable.ToFloat(index, "Weight");
        m_wpInfo = new WeaponInfo(index, nameEn, nameKr, desc, spriteName, materialsIndexArray, materialsCostArray, damage, weight);
        m_x = x;
        m_y = y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_uiInfoBox == null)
        {
            GameObject ui = Instantiate(m_infoBoxPrefab, m_infoBoxPoses[0]);
            m_uiInfoBox = ui.GetComponent<UI_InfoBox>();
        }


        if (m_y < 3)
        {
            if (m_x < 5)
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[0]);

            }
            else
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[1]);
            }
        }
        else
        {
            if (m_x < 5)
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[2]);
            }
            else
            {
                m_uiInfoBox.gameObject.transform.SetParent(m_infoBoxPoses[3]);
            }
        }
        m_uiInfoBox.OpenBox(m_wpInfo.NameKr, m_wpInfo.Desc);
        m_uiInfoBox.transform.SetParent(transform.parent);
        m_uiInfoBox.transform.SetAsLastSibling();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_uiInfoBox != null)
            m_uiInfoBox.CloseBox();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        m_uiInteraction.ReadyToCraftSometing(m_wpInfo);
        m_uiInteraction.OpenInteraction();
        m_uiInfoBox.CloseBox();
        
    }
}
