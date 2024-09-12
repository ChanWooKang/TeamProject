using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PetEnryInfoBoxController : MonoBehaviour
{
    int m_maxEntryCount;

    List<Image> m_listPetIcon;
    bool m_isPetOut;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //우측 스왑            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //좌측 스왑
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

            }
        }
    }
    private void Awake()
    {
        m_maxEntryCount = PetEntryManager._inst.MaxEntryCount;

        // m_listPetIcon = new List<Image>(m_maxEntryCount);
    }
    public void InitEntryIcon()
    {
        // 풀에서 이미지를 받아옴
    }
    void RightSwap()
    {
        for (int i = 0; i < m_listPetIcon.Count; i++)
        {

        }
    }
    void LeftSwap()
    {
        for (int i = 0; i < m_listPetIcon.Count; i++)
        {

        }
    }
   
    void ReCall()
    {
        //Instantiate(PetEntryManager._inst.m_listPetEntry[0], GameManagerEx._inst.playerManager.transform.position + (Vector3.forward *0.5f) + (Vector3.right * 0.8f), PetEntryManager._inst.m_listPetEntry[0].transform.rotation);
        PoolingManager._inst.InstantiateAPS(1100, GameManagerEx._inst.playerManager.transform.position + (Vector3.forward * 0.5f) + (Vector3.right * 0.8f), PetEntryManager._inst.m_listPetEntry[0].transform.rotation, Vector3.one);
    }
    void PutIn()
    {
        
    }



}
