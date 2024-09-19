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
    public Dictionary<int, GameObject> m_dicPetPortraitObject;
    public Dictionary<int, GameObject> m_dicPetObject;
    List<int> m_listPetIndex;

    [SerializeField] UI_PetEnryInfoBoxController m_uiPetEntryInfoBox;
    [SerializeField] Transform m_petPortraitRoot;
    private void Awake()
    {
        m_dicPetObject = new Dictionary<int, GameObject>();
        m_dicPetPortraitObject = new Dictionary<int, GameObject>();
        for(int i = 0; i < m_listPetEntryPrefab.Count; i++)
        {
            int index = 1000 + i;
            m_dicPetObject.Add(index, m_listPetEntryPrefab[i]);
            m_dicPetPortraitObject.Add(index, m_listPetPortraitPrefab[i]);
        }
    }
    private void Start()
    {
        //임시
        m_listPetEntryCtrl = new List<PetController>(m_maxEntryCount);
        m_listPetIndex = new List<int>();
        for (int i = 0; i < m_listPetEntryPrefab.Count; i++)
        {
            PetController petCtrl = m_listPetEntryPrefab[i].GetComponent<PetController>();
            int index = 1000 + i;
            petCtrl.InitPet(index);
            m_listPetIndex.Add(index);
            m_listPetEntryCtrl.Add(petCtrl);
            PoolingManager._inst.AddPetPool(petCtrl); // 펫을 잡을 때 이 함수를 실행해야함            
            m_dicPetPortraitObject[index] = Instantiate(m_dicPetPortraitObject[index], m_petPortraitRoot, false);
            m_dicPetPortraitObject[index].SetActive(false);
        }
        m_uiPetEntryInfoBox.SetHudInfoBox(m_listPetEntryCtrl[0]);
    }
    public void InitEntry()
    {
        m_listPetEntryPrefab = new List<GameObject>(m_maxEntryCount);
        m_listPetEntryCtrl = new List<PetController>(m_maxEntryCount);
        m_listPetIndex = new List<int>();
    }

    public void AddEntry(int Petindex)
    {
       
        if (!m_listPetIndex.Contains(Petindex))
        {
            m_listPetIndex.Add(Petindex);
            if (m_listPetEntryPrefab.Count <= MaxEntryCount)
            {
                PetController pet = m_dicPetObject[Petindex].GetComponent<PetController>();
                pet.InitPet(Petindex);
                m_listPetEntryCtrl.Add(pet);
                PoolingManager._inst.AddPetPool(pet);
                m_uiPetEntryInfoBox.SetHudInfoBox(pet);
                m_uiPetEntryInfoBox.InitCurrentPetIndex(Petindex);
                GameObject portrait = Instantiate(m_listPetPortraitPrefab[Petindex], m_petPortraitRoot, false);
                portrait.SetActive(false);
            }
            else
            {
                //엔트리에 펫이 꽉 찼을 때
            }
        }
        else
        {
            // 이미 보유중인 펫을 잡았을 때
        }
    }
    
    public void RemoveEntry(PetController pet)
    {
        m_listPetEntryPrefab.Remove(pet.gameObject);
        m_listPetEntryCtrl.Remove(pet);
    }


}
