using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PetEnryInfoBoxController : MonoBehaviour
{
    const int offset = 100;
    int m_maxEntryCount;
    int m_currentPetIndex;
    int m_currentPetNum;

    List<Image> m_listPetIcon;


    #region [�� ����]
    bool m_isPetOut;
    GameObject m_recalledPet;
    HudController m_recalledPetsHud;
    #endregion [�� ����]

    #region [UI ����]
    [SerializeField] HudController m_hudInfo;
    #endregion [UI ����]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //���� ����            
            RightSwap();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //���� ����
            LeftSwap();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!m_isPetOut)
            {
                ReCall();
                m_isPetOut = true;
            }
            else
            {
                PutIn();
                m_isPetOut = false;
            }
        }
    }
    private void Awake()
    {
        m_maxEntryCount = PetEntryManager._inst.MaxEntryCount;
        m_currentPetIndex = 1100;
        //�ӽ�
        m_currentPetNum = 0;
        
        // m_listPetIcon = new List<Image>(m_maxEntryCount);
    }
    public void InitEntryIcon()
    {
        // Ǯ���� �̹����� �޾ƿ�
    }
    void RightSwap()
    {
        //for (int i = 0; i < m_listPetIcon.Count; i++)
        //{

        //}
        if (++m_currentPetNum >= PetEntryManager._inst.m_listPetEntryCtrl.Count)
            m_currentPetNum = 0;

        m_currentPetIndex = offset + PetEntryManager._inst.m_listPetEntryCtrl[m_currentPetNum].PetInfo.Index;
        SetHudInfoBox(PetEntryManager._inst.m_listPetEntryCtrl[m_currentPetNum]);
    }
    void LeftSwap()
    {
        //for (int i = 0; i < m_listPetIcon.Count; i++)
        //{

        //}
        if (--m_currentPetNum < 0)
            m_currentPetNum = PetEntryManager._inst.m_listPetEntryCtrl.Count - 1;

        m_currentPetIndex = offset + PetEntryManager._inst.m_listPetEntryCtrl[m_currentPetNum].PetInfo.Index;
        SetHudInfoBox(PetEntryManager._inst.m_listPetEntryCtrl[m_currentPetNum]);
    }

    void ReCall()
    {
        m_recalledPet = PoolingManager._inst.InstantiateAPS(m_currentPetIndex, GameManagerEx._inst.playerManager.transform.position + (Vector3.forward * 0.5f) + (Vector3.right * 0.8f), PetEntryManager._inst.m_listPetEntry[1].transform.rotation, Vector3.one);
        if (m_recalledPetsHud == null)
        {
            GameObject hud = PoolingManager._inst.InstantiateAPS(1000000);
            hud.SetActive(true);
            m_recalledPetsHud = hud.GetComponent<HudController>();
        }
        PetController pet = m_recalledPet.GetComponent<PetController>();
        pet.SetHud(m_recalledPetsHud, PoolingManager._inst._hudRootTransform);
    }
    void PutIn()
    {
        PoolingManager.DestroyAPS(m_recalledPet);
        m_recalledPetsHud.HideHud();
    }
    public void SetHudInfoBox(PetController pet)
    {
        m_hudInfo.InitHud(pet.PetInfo.NameKr, pet.PetLevel, null, Color.white, true, null);        
    }


}
