using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    bool m_isAllDead;
    GameObject m_recalledPet;
    PetController m_petCtrl;
    HudController m_recalledPetsHud;
    public GameObject RecalledPet { get { return m_recalledPet; } }
    public HudController RecalledPetHud { get { return m_recalledPetsHud; } }
    public bool IsAllDead { get { return m_isAllDead; } set { m_isAllDead = value; } }
    #endregion [펫 관련]

    #region [UI 관련]
    [SerializeField] HudController m_hudInfo;
    [SerializeField] TextMeshProUGUI m_textLevelUp;
    Animator m_animCtrl;
    bool isCoroutineRunning = false;
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
        m_isAllDead = false;
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
    void InitEntryIcon()
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
        else if (PetEntryManager._inst.m_dictPetEntryCtrl.Count == 0)
        {
            for (int i = 0; i < m_listPetIcon.Count; i++)
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
            SoundManager._inst.PlaySfx("Recall");
            m_isPetOut = true;
        }
        else
        {
            PutIn();
            SoundManager._inst.PlaySfx("PutIn");
            m_isPetOut = false;
        }
        return m_isPetOut;

    }
    public void SetRecalledPetHud()
    {
        if (m_recalledPetsHud != null)
            m_hudInfo.DisPlayEntryHud(m_petCtrl.Stat.HP / m_petCtrl.Stat.MaxHP);
    }
    public void RightSwap()
    {
        if (PetEntryManager._inst.m_dictPetEntryCtrl.Count < 2)
            return;
        m_animCtrl.SetTrigger("SwapRight");
        int tempNum = m_currentPetNum;

        if (++m_currentPetNum >= PetEntryManager._inst.m_listEntryPetUniqueindex.Count)
            m_currentPetNum = 0;

        int nextUIndex = PetEntryManager._inst.m_listEntryPetUniqueindex[m_currentPetNum];
        m_currentPetUIndex = nextUIndex;
        CheckAllDead(nextUIndex, tempNum);

        SetHudInfoBox(PetEntryManager._inst.m_dictPetEntryCtrl[m_currentPetUIndex]);
    }
    void LeftSwap()
    {
        if (PetEntryManager._inst.m_dictPetEntryCtrl.Count < 2)
            return;
        if (PetEntryManager._inst.m_dictPetEntryCtrl.Count == 2)
            m_animCtrl.SetTrigger("SwapRight");
        else
            m_animCtrl.SetTrigger("SwapLeft");
        int temp = m_currentPetNum;

        if (--m_currentPetNum < 0)
            m_currentPetNum = PetEntryManager._inst.m_dictPetEntryCtrl.Count - 1;

        int prevUIndex = PetEntryManager._inst.m_listEntryPetUniqueindex[m_currentPetNum];
        m_currentPetUIndex = prevUIndex;
        CheckAllDead(prevUIndex, temp, false);

        SetHudInfoBox(PetEntryManager._inst.m_dictPetEntryCtrl[m_currentPetUIndex]);
    }
    void CheckAllDead(int swapUIndex, int temp, bool isright = true)
    {
        if (PetEntryManager._inst.m_dictPetEntryCtrl[swapUIndex].Stat.HP <= 0)
        {
            bool isAllDead = true;
            foreach (PetController pet in PetEntryManager._inst.m_dictPetEntryCtrl.Values)
            {
                if (pet.Stat.HP > 0)
                {
                    isAllDead = false;
                    m_isAllDead = isAllDead;
                    break;
                }
            }
            if (isAllDead)
            {
                m_isAllDead = isAllDead;
                m_currentPetNum = temp;
                return;
            }
            else
            {
                if (isright)
                    RightSwap();
                else
                    LeftSwap();
            }
        }

    }
    void ReCall(Vector3 pos)
    {
        if (PetEntryManager._inst.m_dictPetEntryCtrl.Count == 0)
            return;
        m_recalledPet = PoolingManager._inst.InstantiateAPS(m_currentPetUIndex, pos, PetEntryManager._inst.m_listPetEntryPrefab[1].transform.rotation, Vector3.one);
        PoolingManager._inst.InstantiateAPS("RecallAura", pos, PoolingManager._inst._poolingEffectByName["RecallAura"].prefab.transform.rotation, Vector3.one);
        PoolingManager._inst.InstantiateAPS("RecallMisc", pos + Vector3.up, PoolingManager._inst._poolingEffectByName["RecallAura"].prefab.transform.rotation, Vector3.one);
        m_petCtrl = m_recalledPet.GetComponent<PetController>();
        GameManagerEx._inst.playerManager.SetCorrentRecallPet(m_petCtrl);
        m_petCtrl.ReCall();
        PetController pet = m_recalledPet.GetComponent<PetController>();
        if (m_recalledPetsHud == null)
        {
            m_recalledPetsHud = pet._hudCtrl;
        }
        GameManagerEx._inst.recalledPetManager = m_petCtrl;

    }
    void PutIn()
    {
        if (m_recalledPet == null)
            return;
        if (m_petCtrl.Stat.HP <= 0)
            RightSwap();
        GameManagerEx._inst.playerManager.SetCorrentRecallPet(null);
        m_recalledPetsHud.HideHud();
        m_recalledPetsHud = null;
        PoolingManager._inst.InstantiateAPS("PutIn", m_recalledPet.transform.position + Vector3.up, m_recalledPet.transform.rotation, Vector3.one);
        PoolingManager.DestroyAPS(m_recalledPet);
        m_recalledPet = null;
        m_petCtrl = null;
        GameManagerEx._inst.recalledPetManager = null;
    }
    public void SetHudInfoBox(PetController pet)
    {
        m_hudInfo.gameObject.SetActive(true);
        m_hudInfo.InitHud(pet.PetInfo.NameKr, pet.Stat.Level, null, Color.white, true, null);
    }

    public IEnumerator StartLevelUPText()
    {
        if (isCoroutineRunning)
            yield return null;
        else
        {
            isCoroutineRunning = true;
            RectTransform rect = m_textLevelUp.GetComponent<RectTransform>();
            RectTransform parent = rect.parent.GetComponent<RectTransform>();
            Vector2 start = new Vector2((parent.sizeDelta.x / 2) + (rect.sizeDelta.x / 2), rect.anchoredPosition.y);
            while (true)
            {
                rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, Vector2.zero, Time.deltaTime * 2.5f);
                if (Vector2.Distance(rect.anchoredPosition, Vector2.zero) <= 0.1f)
                {
                    StartCoroutine(EndLevelUpText(start, rect, parent));
                    break;
                }
                yield return null;
            }
        }
    }
    IEnumerator EndLevelUpText(Vector2 start, RectTransform rect, RectTransform parent)
    {
        Vector2 target = new Vector2(-Mathf.Abs((parent.sizeDelta.x / 2) + (start.x / 2)), start.y);
        while (true)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, target, Time.deltaTime * 2.5f);
            if (Vector2.Distance(rect.anchoredPosition, target) <= 0.1f)
            {
                rect.anchoredPosition = new Vector2((parent.sizeDelta.x / 2) + (rect.sizeDelta.x / 2), rect.anchoredPosition.y);
                isCoroutineRunning = false;
                break;
            }
            yield return null;
        }
    }
}
