using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_WeaponInfo : MonoBehaviour
{
    [SerializeField] Image m_weaponIcon;
    [SerializeField] TextMeshProUGUI m_txtWeaponName;
    [SerializeField] TextMeshProUGUI m_txtWeaponAmmo;
    [SerializeField] GameObject m_noneObj;

    int m_currentAmmo;
    int m_FullAmmo;


    public void InitSlot(WeaponItemInfo wInfo = null)
    {
        if (wInfo != null)
        {
            m_weaponIcon.enabled = true;
            m_txtWeaponName.enabled = true;
            m_txtWeaponAmmo.enabled = true;
            m_noneObj.SetActive(false);
            m_weaponIcon.sprite = PoolingManager._inst._poolingIconByIndex[wInfo.Index].prefab;
            m_txtWeaponName.text = wInfo.NameKr;
            if (wInfo.Ammo > 0)
            {
                m_txtWeaponAmmo.enabled = true;
                m_FullAmmo = wInfo.Ammo;
                m_currentAmmo = wInfo.CurrentAmmo;

                SetAmmo(m_currentAmmo, m_FullAmmo);
            }
            else
                m_txtWeaponAmmo.enabled = false;
        }
        else
        {
            m_weaponIcon.enabled = false;
            m_txtWeaponName.text = "¸Ç ¼Õ";
            m_txtWeaponAmmo.enabled = false;
            m_noneObj.SetActive(true);
        }
    }

    void SetAmmo(int currentAmmo, int FullAmmo)
    {
        m_txtWeaponAmmo.text = currentAmmo.ToString() + "/" + FullAmmo.ToString();
    }

    public void ShootWeapon()
    {
        m_currentAmmo--;

        m_txtWeaponAmmo.text = m_currentAmmo.ToString() + "/" + m_FullAmmo.ToString();
    }

    public void ReloadWeapon(int currentAmmo)
    {
        m_txtWeaponAmmo.text = currentAmmo.ToString() + "/" + m_FullAmmo.ToString();
    }
}
