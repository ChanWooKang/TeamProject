using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PetEnryInfoBoxController : MonoBehaviour
{
    const int offset = 100;
    public int m_currentPetIndex;
    int m_maxEntryCount;
    int m_currentPetNum;

    List<Image> m_listPetIcon;


    #region [펫 관련]
    bool m_isPetOut;
    GameObject m_recalledPet;
    PetController m_petCtrl;
    HudController m_recalledPetsHud;
    #endregion [펫 관련]

    #region [UI 관련]
    [SerializeField] HudController m_hudInfo;
    #endregion [UI 관련]
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    //우측 스왑            
        //    RightSwap();
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    //좌측 스왑
        //    LeftSwap();
        //}
        if (Input.GetKeyDown(KeyCode.E))
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
        //임시
        m_currentPetNum = 0;
        m_hudInfo.gameObject.SetActive(false);
        // m_listPetIcon = new List<Image>(m_maxEntryCount);
    }
    public void InitEntryIcon()
    {
        // 풀에서 이미지를 받아옴
    }
    public void OpenUI()
    {
        gameObject.SetActive(true);
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
    public void InitCurrentPetIndex(int index)
    {
        m_currentPetIndex = index + offset;
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
        if (PetEntryManager._inst.m_listPetEntryCtrl.Count == 0)
            return;
        m_recalledPet = PoolingManager._inst.InstantiateAPS(m_currentPetIndex, GameManagerEx._inst.playerManager.transform.position + (Vector3.forward * 0.5f) + (Vector3.right * 0.8f), PetEntryManager._inst.m_listPetEntryPrefab[1].transform.rotation, Vector3.one);
        m_petCtrl = m_recalledPet.GetComponent<PetController>();
        m_petCtrl.ReCall();
        PetController pet = m_recalledPet.GetComponent<PetController>();
        if (m_recalledPetsHud == null)
        {
            m_recalledPetsHud = pet._hudCtrl;
        }

    }
    void PutIn()
    {
        if (m_recalledPet == null)
            return;
        m_recalledPetsHud.HideHud();
        PoolingManager.DestroyAPS(m_recalledPet);
    }
    public void SetHudInfoBox(PetController pet)
    {
        m_hudInfo.gameObject.SetActive(true);
        m_hudInfo.InitHud(pet.PetInfo.NameKr, pet.PetLevel, null, Color.white, true, null);
    }
    public void ShowPetPortrait()
    {

    }

}
