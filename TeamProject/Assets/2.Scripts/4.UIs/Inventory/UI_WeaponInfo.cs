using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_WeaponInfo : MonoBehaviour
{
    [SerializeField] Image m_weaponIcon;
    [SerializeField] Image m_petballIcon;
    [SerializeField] TextMeshProUGUI m_txtWeaponName;
    [SerializeField] TextMeshProUGUI m_txtWeaponAmmo;
    [SerializeField] TextMeshProUGUI m_txtBallCount;
    [SerializeField] TextMeshProUGUI m_txtBallName;
    [SerializeField] GameObject m_noneObj;



    int m_currentAmmo;
    int m_FullAmmo;
    int m_ballIndex;


    public int BallIndex { get { return m_ballIndex; } }

    private void Awake()
    {
        InitWeaponSlot();
        InitPetBallSlot();
    }
    private void Update()
    {
        if (m_petballIcon.enabled)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                SwapBall(m_ballIndex);
            }
        }
    }
    public void InitWeaponSlot(WeaponItemInfo wInfo = null)
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
    public void InitPetBallSlot(PetBallInfo bInfo = null)
    {
        if (bInfo == null)
        {
            m_txtBallCount.enabled = false;
            m_petballIcon.enabled = false;
            m_txtBallName.enabled = false;
        }
        else
        {
            m_txtBallCount.enabled = true;
            m_petballIcon.enabled = true;
            m_txtBallName.enabled = true;
            m_ballIndex = bInfo.Index;
            m_txtBallCount.text = InventoryManager._inst.GetItemCount(m_ballIndex).ToString();
            m_txtBallName.text = bInfo.NameKr;
            m_petballIcon.sprite = PoolingManager._inst._poolingIconByIndex[m_ballIndex].prefab;
        }
    }
    void SetAmmo(int currentAmmo, int FullAmmo)
    {
        m_txtWeaponAmmo.text = currentAmmo.ToString() + "/" + FullAmmo.ToString();
    }

    public void ShootWeapon(int ammo)
    {
        m_currentAmmo = ammo;


        m_txtWeaponAmmo.text = ammo.ToString() + "/" + m_FullAmmo.ToString();
    }

    public void ReloadWeapon(int currentAmmo)
    {
        m_txtWeaponAmmo.text = currentAmmo.ToString() + "/" + m_FullAmmo.ToString();
    }
    public void SetBallCount()
    {
        m_txtBallCount.text = InventoryManager._inst.GetItemCount(m_ballIndex).ToString();
    }

    void SwapBall(int ballIndex)
    {
        ballIndex++;
        if (!InventoryManager._inst.Dict_PetballCount.ContainsKey(ballIndex))
            ballIndex = InventoryManager._inst.List_PetBallIndex[0];
        InitPetBallSlot(InventoryManager._inst.Dict_Petball[ballIndex]);
        m_ballIndex = ballIndex;

    }
}
