using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_MaterialSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_txtMaterialName;
    [SerializeField] TextMeshProUGUI m_txtMaterialCost;
    [SerializeField] Image m_icon;

    public void OpenSlot(Sprite icon, string name, int cost, int own)
    {
        m_icon.sprite = icon;
        m_txtMaterialName.text = name;
        StringBuilder sb = new StringBuilder(own);
        sb.Append('/', cost);
        m_txtMaterialCost.text = sb.ToString();
    }
}
