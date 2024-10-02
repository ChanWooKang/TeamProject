using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBoxController : MonoBehaviour
{
    #region [UIPrefab]
    [SerializeField] GameObject m_uiPetBoxPrefab;
    #endregion [UIPrefab]
    #region [Component]
    [SerializeField] Animator m_animCtrl;
    [SerializeField]
    PetBoxAnimEventController m_animEventCtrl;
    UI_PetBoxController m_uiPetBoxCtrl;
    #endregion [Component]
    #region [Param]
    bool m_isOpen;
    bool m_isDetected;
    #endregion[Param]
    private void Awake()
    {
        m_isOpen = false;
    }
    private void Update()
    {
        if (m_isDetected)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SetBool();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isDetected = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isDetected = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_isDetected = false;
            m_isOpen = false;
            m_animCtrl.SetBool("IsOpen", m_isOpen);
        }
    }

    void SetBool()
    {
        if (m_uiPetBoxCtrl == null)
        {
            GameObject ui = Instantiate(m_uiPetBoxPrefab);
            m_uiPetBoxCtrl = ui.GetComponent<UI_PetBoxController>();
            m_uiPetBoxCtrl.ClosePetBox();
            m_animEventCtrl.InitAnimEventController(m_uiPetBoxCtrl);
        }
        if (m_isOpen)
            m_isOpen = false;
        else
            m_isOpen = true;
        m_animCtrl.SetBool("IsOpen", m_isOpen);
        GameManagerEx._inst.ControlUI(m_isOpen, true);
    }
}
