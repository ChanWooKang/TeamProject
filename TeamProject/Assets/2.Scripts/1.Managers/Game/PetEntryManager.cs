using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEntryManager : TSingleton<PetEntryManager>
{
    const int m_maxEntryCount = 6;
    public int MaxEntryCount { get { return m_maxEntryCount; } }
    public List<GameObject> m_listPetEntryPrefab;
    public List<GameObject> m_listPetPortraitPrefab;
    [HideInInspector] public List<PetController> m_listPetEntryCtrl;
    Dictionary<int, GameObject> m_dicPetPortraitPrefab;
    public Dictionary<int, GameObject> m_dicPetPortraitObject;
    public Dictionary<int, GameObject> m_dicPetObject;
    List<int> m_listPetIndex;

    [SerializeField] UI_PetEnryInfoBoxController m_uiPetEntryInfoBox;
    [SerializeField] Transform m_petPortraitRoot;


    int m_tempOffset;
    [HideInInspector] public UI_PetBoxController m_petBoxCtrl;
    private void Awake()
    {
        m_dicPetObject = new Dictionary<int, GameObject>();
        m_dicPetPortraitObject = new Dictionary<int, GameObject>();
        m_dicPetPortraitPrefab = new Dictionary<int, GameObject>();
        for (int i = 0; i < m_listPetEntryPrefab.Count; i++)
        {
            int index = 1000 + i;
            m_dicPetObject.Add(index, m_listPetEntryPrefab[i]);
            m_dicPetPortraitPrefab.Add(index, m_listPetPortraitPrefab[i]);
        }
    }
    private void Start()
    {
        m_listPetEntryCtrl = new List<PetController>(m_maxEntryCount);
        m_listPetIndex = new List<int>();
    }
    int i = 0;
    private void Update()
    {
        // ¿”Ω√
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
            AddEntry(index, index + (++m_tempOffset));            
        }
    }
    public void InitEntry()
    {
        m_listPetEntryPrefab = new List<GameObject>(m_maxEntryCount);
        m_listPetEntryCtrl = new List<PetController>(m_maxEntryCount);
        m_listPetIndex = new List<int>();
    }

    public void AddEntry(int index, int UniqueId)
    {
        if (!m_listPetIndex.Contains(UniqueId))
        {
            m_listPetIndex.Add(UniqueId);
            PetController pet = m_dicPetObject[index].GetComponent<PetController>();
            pet.InitPet(index);

            if (m_listPetEntryCtrl.Count < MaxEntryCount)
            {
                InitPetEntry(pet, UniqueId);
                m_uiPetEntryInfoBox.InitAllEntryIcon();
            }
            else
            {
                //ø£∆Æ∏Æø° ∆Í¿Ã ≤À √°¿ª ∂ß
                if (m_petBoxCtrl != null)
                {
                    m_petBoxCtrl.GetPetIntheBox(pet);
                }
                else
                {

                }
            }
        }
    }

    public void RemoveEntry(PetController pet)
    {
        m_listPetEntryPrefab.Remove(pet.gameObject);
        m_listPetEntryCtrl.Remove(pet);
    }
    public void InitPetBox(UI_PetBoxController petbox)
    {
        m_petBoxCtrl = petbox;
    }
    public void InitPetEntry(PetController pet, int UniqueID)
    {
        m_listPetEntryCtrl.Add(pet);
        PoolingManager._inst.AddPetPool(pet);
        m_uiPetEntryInfoBox.SetHudInfoBox(pet);
        m_uiPetEntryInfoBox.InitCurrentPetIndex(pet.PetInfo.Index, m_listPetEntryCtrl.Count);
        if (!m_dicPetPortraitObject.ContainsKey(pet.PetInfo.Index))
        {
            GameObject portrait = Instantiate(m_dicPetPortraitPrefab[pet.PetInfo.Index], m_petPortraitRoot, false);
            m_dicPetPortraitObject.Add(pet.PetInfo.Index, portrait);
            portrait.SetActive(false);
        }
    }

}
