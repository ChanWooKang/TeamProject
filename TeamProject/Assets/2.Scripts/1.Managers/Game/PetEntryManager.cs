using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEntryManager : TSingleton<PetEntryManager>
{
    const int m_maxEntryCount = 6;
    public int MaxEntryCount { get { return m_maxEntryCount; } }
    public List<GameObject> m_listPetEntry;
    
    public List<PetController> m_listPetEntryCtrl;

    [SerializeField] UI_PetEnryInfoBoxController m_uiPetEntryInfoBox;
    private void Start()
    {
        //임시
        m_listPetEntryCtrl = new List<PetController>(m_maxEntryCount);        

        for (int i = 0; i < m_listPetEntry.Count; i++)
        {
            PetController petCtrl = m_listPetEntry[i].GetComponent<PetController>();
            petCtrl.InitPet(1000 + i);
            m_listPetEntryCtrl.Add(petCtrl);
            PoolingManager._inst.AddPetPool(petCtrl); // 포켓몬을 잡을 때 이 함수를 실행해야함
            
        }
        m_uiPetEntryInfoBox.SetHudInfoBox(m_listPetEntryCtrl[0]);
    }
    public void InitEntry()
    {
        m_listPetEntry = new List<GameObject>(m_maxEntryCount);                
    }

    public void AddEntry(PetController pet)
    {
        m_listPetEntry.Add(pet.gameObject);
        m_listPetEntryCtrl.Add(pet);
    }

    public void RemoveEntry(PetController pet)
    {
        m_listPetEntry.Remove(pet.gameObject);
        m_listPetEntryCtrl.Remove(pet);
    }


}
