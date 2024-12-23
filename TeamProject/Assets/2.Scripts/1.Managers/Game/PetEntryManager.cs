using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEntryManager : TSingleton<PetEntryManager>
{
    const int m_maxEntryCount = 6;
    public int MaxEntryCount { get { return m_maxEntryCount; } }
    public List<GameObject> m_listPetEntryPrefab;
    public List<GameObject> m_listPetPortraitPrefab;
    public List<int> m_listEntryPetUniqueindex;
    [HideInInspector] public Dictionary<int, PetController> m_dictPetEntryCtrl;// <Dic<uniqueIndex, PetController>>
    public Dictionary<int, GameObject> m_dicPetPortraitObject;
    public Dictionary<int, GameObject> m_dicPetObject;
    public Dictionary<int, int> m_dictPetballIndex; // <Dic<uniqueIndex, petballindex>>
    Dictionary<int, GameObject> m_dicPetPortraitPrefab;

    [SerializeField] UI_PetEnryInfoBoxController m_uiPetEntryInfoBox;
    [SerializeField] Transform m_petPortraitRoot;


    int m_tempOffset;
    [HideInInspector] public UI_PetBoxController m_petBoxCtrl;
    private void Awake()
    {
        m_dicPetObject = new Dictionary<int, GameObject>();
        m_dicPetPortraitObject = new Dictionary<int, GameObject>();
        m_dicPetPortraitPrefab = new Dictionary<int, GameObject>();
        m_listEntryPetUniqueindex = new List<int>();
        for (int i = 0; i < m_listPetEntryPrefab.Count; i++)
        {
            int index = 1000 + i;
            m_dicPetObject.Add(index, m_listPetEntryPrefab[i]);
            m_dicPetPortraitPrefab.Add(index, m_listPetPortraitPrefab[i]);
        }
    }
    private void Start()
    {
        m_dictPetEntryCtrl = new Dictionary<int, PetController>(m_maxEntryCount);
        m_dictPetballIndex = new Dictionary<int, int>();
    }
    int i = 0;
    private void Update()
    {
        // 임시
        if (Input.GetKeyDown(KeyCode.F12))
        {
            m_tempOffset = Random.Range(0, 10000);
            if (i == 0)
                i = 1;
            else
                i = 0;
            PetController petCtrl = m_listPetEntryPrefab[i].GetComponent<PetController>();
            int index = 0;
            if (i == 0)
                index = 1000;
            else
                index = 1001;

            petCtrl.InitPet(index);
            AddEntry(index, index + (++m_tempOffset), 500);            
        }
        
    }
    public void InitEntry()
    {
        m_listPetEntryPrefab = new List<GameObject>(m_maxEntryCount);
        m_dictPetEntryCtrl = new Dictionary<int, PetController>();
        m_dictPetballIndex = new Dictionary<int, int>();
    }

    public void AddEntry(int index, int UniqueId, int ballIndex)
    {
        if (!m_dictPetballIndex.ContainsKey(UniqueId)) // 유니크 아이디 검사
        {
            m_listEntryPetUniqueindex.Add(UniqueId); // 내가 가진 펫에 유니크 아이디 추가
            m_dictPetballIndex.Add(UniqueId, ballIndex); // 잡은 펫을 잡은 볼의 인덱스 검사를 위한 추가ㅏ
            
            if (m_dictPetEntryCtrl.Count < MaxEntryCount) // 내가 가진 펫이 6마리 미만일 때
            {
                InitPetEntry(index, UniqueId); // 엔트리에 펫 추가
                m_uiPetEntryInfoBox.InitAllEntryIcon(); // UI Init
                m_uiPetEntryInfoBox.IsAllDead = false;
            }
            else
            {
                //엔트리에 펫이 꽉 찼을 때
                if (m_petBoxCtrl != null) // 펫박스가 있음
                {
                    m_petBoxCtrl.GetPetIntheBox(InitPetBox(index, UniqueId));
                }
                else //  펫박스가 없음
                {}
            }
        }
    }

    public void RemoveEntry(PetController pet)
    {
        m_listPetEntryPrefab.Remove(pet.gameObject);
        //m_listPetEntryCtrl.Remove(pet);
    }
    public void InitPetBox(UI_PetBoxController petbox)
    {
        m_petBoxCtrl = petbox;
    }
    public void InitPetEntry(int index,int uniqueID)
    {
        PetController pet = PoolingManager._inst.AddPetPool(m_dicPetObject[index], index, uniqueID); // 잡은 펫은 풀링에 추가하여 관리
        m_dictPetEntryCtrl.Add(pet.Stat.UniqueID, pet); // 펫 컨트롤러 저장
        m_uiPetEntryInfoBox.SetHudInfoBox(pet); // 펫 HUD 초기화
        m_uiPetEntryInfoBox.InitCurrentPetIndex(pet.Stat.UniqueID, m_dictPetEntryCtrl.Count); // 현재 잡은 펫을 1선으로
        if (!m_dicPetPortraitObject.ContainsKey(pet.PetInfo.Index)) // 펫 초상화 저장
        {
            GameObject portrait = Instantiate(m_dicPetPortraitPrefab[pet.PetInfo.Index], m_petPortraitRoot, false);
            portrait.transform.position = m_petPortraitRoot.position;
            m_dicPetPortraitObject.Add(pet.PetInfo.Index, portrait);
            portrait.SetActive(false);
        }
    }
    PetController InitPetBox(int index, int uniqueID)
    {
        PetController pet = PoolingManager._inst.AddPetPool(m_dicPetObject[index], index, uniqueID);
        return pet;
    }

}
