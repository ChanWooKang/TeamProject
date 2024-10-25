using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UI_PetEnryInfoBoxController : MonoBehaviour
{
    const int offset = 100;
    public int m_currentPetUIndex;
    
    public int m_currentPetNum;

    [SerializeField] List<Image> m_listPetIcon;
    List<Sprite> m_listAllPetIcon;
    #region [펫 관련]
    bool m_isPetOut;
    GameObject m_recalledPet;
    PetController m_petCtrl;
    HudController m_recalledPetsHud;
    public GameObject RecalledPet { get { return m_recalledPet; } }
    
    #endregion [펫 관련]

    #region [UI 관련]
    [SerializeField] HudController m_hudInfo;
    Animator m_animCtrl;
    #endregion [UI 관련]
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //우측 스왑
            InitEntryIcon();
            RightSwap();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            //좌측 스왑
            InitEntryIcon();
            LeftSwap();
        }

    }
    private void Awake()
    {        
        m_animCtrl = GetComponent<Animator>();
        m_listAllPetIcon = new List<Sprite>();
        for (int i = 0; i < PetEntryManager._inst.MaxEntryCount; i++)
            m_listAllPetIcon.Add(null);

        //임시
        m_currentPetNum = 0;

        m_hudInfo.gameObject.SetActive(false);
        InitEntryIcon();
    }
    public void Anim_SwapStop()
    {
        m_animCtrl.SetTrigger("SwapStop");
        InitEntryIcon();
    }
    public void InitEntryIcon()
    {
        if (PetEntryManager._inst.m_dictPetEntryCtrl == null)
            return;
        if (m_currentPetNum < 0)
            m_currentPetNum = 0;
        int Pidx = m_currentPetNum;
        if (m_listAllPetIcon[Pidx] != null)
        {
            m_listPetIcon[0].enabled = true;
            m_listPetIcon[0].sprite = m_listAllPetIcon[Pidx];
        }
        else
            m_listPetIcon[0].enabled = false;
        Pidx = m_currentPetNum - 1;
        if (Pidx < 0)
            Pidx = (PetEntryManager._inst.m_dictPetEntryCtrl.Count - 1);
        if (Pidx < 0)
            Pidx = 0;
        if (m_listAllPetIcon[Pidx] != null)
        {
            m_listPetIcon[1].enabled = true;
            m_listPetIcon[1].sprite = m_listAllPetIcon[Pidx];
        }
        else
            m_listPetIcon[1].enabled = false;
        Pidx = m_currentPetNum + 1;
        if (Pidx > PetEntryManager._inst.m_dictPetEntryCtrl.Count - 1)
            Pidx -= (PetEntryManager._inst.m_dictPetEntryCtrl.Count);
        if (m_listAllPetIcon[Pidx] != null)
        {
            m_listPetIcon[2].enabled = true;
            m_listPetIcon[2].sprite = m_listAllPetIcon[Pidx];
        }
        else
            m_listPetIcon[2].enabled = false;

        if (PetEntryManager._inst.m_dictPetEntryCtrl.Count == 1)
        {
            m_listPetIcon[1].enabled = false;
            m_listPetIcon[2].enabled = false;
        }
        else if (PetEntryManager._inst.m_dictPetEntryCtrl.Count == 2)
        {
            m_listPetIcon[1].enabled = false;
        }
        else if(PetEntryManager._inst.m_dictPetEntryCtrl.Count == 0)
        {
            for(int i = 0; i < m_listPetIcon.Count; i++)
            {
                m_listPetIcon[i].enabled = false;
            }
        }
        
        if (PetEntryManager._inst.m_listEntryPetUniqueindex.Count > 0)
        {
            int prevUIndex = PetEntryManager._inst.m_listEntryPetUniqueindex[m_currentPetNum];            
            m_currentPetUIndex = PetEntryManager._inst.m_dictPetEntryCtrl[prevUIndex].Stat.UniqueID;
        }
      
        //Debug.Log(m_currentPetNum);
    }
    public void InitAllEntryIcon()
    {

        int entryCount = PetEntryManager._inst.m_dictPetEntryCtrl.Count;
        for (int i = 0; i < entryCount; i++)
        {
            List<int> petentryUIndex = PetEntryManager._inst.m_listEntryPetUniqueindex;
            m_listAllPetIcon[i] = PoolingManager._inst._poolingIconByIndex[PetEntryManager._inst.m_dictPetEntryCtrl[petentryUIndex[i]].PetInfo.Index].prefab;
        }
        if (entryCount < 6)
        {
            for (int i = m_listAllPetIcon.Count; i < PetEntryManager._inst.MaxEntryCount; i++)
            {
                m_listPetIcon.Add(null);
            }
        }
        InitEntryIcon();
    }
    public void OpenUI()
    {
        gameObject.SetActive(true);
        InitEntryIcon();
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
    public void InitCurrentPetIndex(int index, int count)
    {
        m_currentPetUIndex = index;
        m_currentPetNum = count - 1;
        InitEntryIcon();
    }
    public bool RecallOrPutIn(Vector3 pos = new Vector3())
    {
        if (!m_isPetOut)
        {
            ReCall(pos);
            m_isPetOut = true;
        }
        else
        {
            PutIn();
            m_isPetOut = false;
        }
        return m_isPetOut;

    }
    public void RightSwap()
    {
        if (PetEntryManager._inst.m_dictPetEntryCtrl.Count < 2)
            return;
        m_animCtrl.SetTrigger("SwapRight");
        if (++m_currentPetNum >= PetEntryManager._inst.m_listEntryPetUniqueindex.Count)
            m_currentPetNum = 0;

        int prevUIndex = PetEntryManager._inst.m_listEntryPetUniqueindex[m_currentPetNum];


        m_currentPetUIndex = PetEntryManager._inst.m_dictPetEntryCtrl[prevUIndex].Stat.UniqueID;
        SetHudInfoBox(PetEntryManager._inst.m_dictPetEntryCtrl[prevUIndex]);
    }
    void LeftSwap()
    {
        if (PetEntryManager._inst.m_dictPetEntryCtrl.Count < 2)
            return;
        if (PetEntryManager._inst.m_dictPetEntryCtrl.Count == 2)
            m_animCtrl.SetTrigger("SwapRight");
        else
            m_animCtrl.SetTrigger("SwapLeft");
        if (--m_currentPetNum < 0)
            m_currentPetNum = PetEntryManager._inst.m_dictPetEntryCtrl.Count - 1;

        int prevUIndex = PetEntryManager._inst.m_listEntryPetUniqueindex[m_currentPetNum];

        m_currentPetUIndex = PetEntryManager._inst.m_dictPetEntryCtrl[prevUIndex].Stat.UniqueID;
        SetHudInfoBox(PetEntryManager._inst.m_dictPetEntryCtrl[prevUIndex]);
    }

    void ReCall(Vector3 pos)
    {
        if (PetEntryManager._inst.m_dictPetEntryCtrl.Count == 0)
            return;
        m_recalledPet = PoolingManager._inst.InstantiateAPS(m_currentPetUIndex, pos, PetEntryManager._inst.m_listPetEntryPrefab[1].transform.rotation, Vector3.one);
        m_petCtrl = m_recalledPet.GetComponent<PetController>();
        GameManagerEx._inst.playerManager.SetCorrentRecallPet(m_petCtrl);
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
        GameManagerEx._inst.playerManager.SetCorrentRecallPet(null);
        m_recalledPetsHud.HideHud();
        m_recalledPetsHud = null;
        PoolingManager.DestroyAPS(m_recalledPet);
        m_recalledPet = null;
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
