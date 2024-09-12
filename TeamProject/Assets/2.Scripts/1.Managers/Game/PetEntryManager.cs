using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEntryManager : TSingleton<PetEntryManager>
{
    const int m_maxEntryCount = 6;
    public int MaxEntryCount { get { return m_maxEntryCount; } }
    public List<GameObject> m_listPetEntry;
    private void Start()
    {
        //юс╫ц
        for (int i = 0; i < m_listPetEntry.Count; i++)
        {
            PetController petCtrl = m_listPetEntry[i].GetComponent<PetController>();
            petCtrl.InitPet(1000 + i);
            PoolingManager._inst.AddPetPool(petCtrl);
        }
    }
    public void InitEntry()
    {
        m_listPetEntry = new List<GameObject>(m_maxEntryCount);                
    }

    public void AddEntry(PetController pet)
    {
        m_listPetEntry.Add(pet.gameObject);
    }

    public void RemoveEntry(PetController pet)
    {
        m_listPetEntry.Remove(pet.gameObject);
    }


}
