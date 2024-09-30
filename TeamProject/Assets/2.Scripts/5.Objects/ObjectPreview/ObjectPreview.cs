using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPreview : MonoBehaviour
{
    #region [참조]
    [SerializeField]
    GameObject m_detectiveAreaObj;
    [SerializeField]
    List<Collider> colliderList = new List<Collider>(); // 충돌한 오브젝트들 저장할 리스트
    UI_Workload m_uiWorkload;
    Architecture m_architectureInfo;
    BoxCollider m_collider;
    NavMeshObstacle m_navObstacle;
    #endregion [참조]

    [SerializeField]
    int layerGround; // 지형 레이어 (무시하게 할 것)
    const int IGNORE_RAYCAST_LAYER = 2;  // ignore_raycast (무시하게 할 것)
    float m_progress;

    bool m_isFixed;
    bool m_isDone;
    public bool IsDone { get { return m_isDone; } }
    public float Progress { get { return m_progress; } set { m_progress = value; } }

    #region[Prefab & Material]
    [SerializeField]
    GameObject m_uiWorkloadPrefab;
    [SerializeField]
    Material m_originalMaterial;
    [SerializeField]
    Material green;
    [SerializeField]
    Material red;
    [SerializeField]
    Material blue;
    #endregion[Prefab & Material]

    #region [임시 펫]
    PetController m_PetCtrl;
    #endregion [임시 펫]
    private void Awake()
    {
        m_isFixed = false;
        m_isDone = false;
        m_collider = GetComponent<BoxCollider>();
        m_navObstacle = GetComponent<NavMeshObstacle>();
        m_navObstacle.enabled = false;  
    }

    void Update()
    {
        if (!m_isFixed)
            ChangeColor();
        if (m_uiWorkload != null && m_uiWorkload.isActiveAndEnabled)
        {
            if (m_uiWorkload.PressFkey())
            {
                SetColor(m_originalMaterial);
                BoxCollider collider = GetComponent<BoxCollider>();
                collider.isTrigger = false;
                m_isDone = true;
                m_collider.size = new Vector3(1.5f, 1f, 1.5f);
                gameObject.transform.parent.gameObject.isStatic = true;
                if (m_PetCtrl != null)
                    m_PetCtrl.JobDone();                
            }
            if (Input.GetKeyUp(KeyCode.F))            
                m_uiWorkload.UpFKey();                            
            if (Input.GetKey(KeyCode.C))
                if (m_uiWorkload.PressCKey())
                    Destroy(gameObject.transform.parent.gameObject);
            if (Input.GetKeyUp(KeyCode.C))
                m_uiWorkload.UpCKey();
        }
    }
    private void LateUpdate()
    {
        if(!m_isFixed && m_PetCtrl != null && !m_PetCtrl.isActiveAndEnabled)
        {
            m_uiWorkload.SetNoWorkEntry();
            m_PetCtrl = null;
        }
    }
    private void ChangeColor()
    {
        if (m_isFixed)
            return;
        if (colliderList.Count > 0)
            SetColor(red);
        else
            SetColor(green);
        transform.parent.LookAt(GameManagerEx._inst.playerManager.transform);
    }

    public void SetColor(Material mat)
    {
        Material[] newMaterials = new Material[GetComponent<Renderer>().materials.Length];

        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = mat;
        }
        GetComponent<Renderer>().materials = newMaterials;
    }
    public void SetArchitectureInfo(Architecture arc)
    {
        m_architectureInfo = arc;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Add(other);

        if (m_isFixed && !m_isDone && (other.CompareTag("Player") || other.CompareTag("Pet")))
        {
            if (m_uiWorkload == null)
            {
                GameObject ui = Instantiate(m_uiWorkloadPrefab);
                Canvas canvas = ui.GetComponent<Canvas>();
                canvas.worldCamera = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();
                ui.transform.position = gameObject.transform.position + gameObject.transform.up * 1.5f + gameObject.transform.right * 1.5f;
                m_uiWorkload = ui.GetComponentInChildren<UI_Workload>();
                m_uiWorkload.OpenUI(this, m_progress);
                m_uiWorkload.SetProgressValue(m_architectureInfo.Progress);
                m_progress = m_architectureInfo.Progress;
            }
            else
                m_uiWorkload.OpenUI(this, m_progress);
            switch (other.tag)
            {
                case "Player":
                    break;
                case "Pet":
                    if (m_PetCtrl == null)
                    {
                        m_PetCtrl = other.gameObject.GetComponent<PetController>();
                        m_PetCtrl.MoveToObject(gameObject.transform.position);
                        m_uiWorkload.SetPetWorkEntry(m_PetCtrl.PetInfo.WorkAbility, m_PetCtrl.PetInfo.NameKr);
                    }
                    break;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (m_isFixed && !m_isDone && other.CompareTag("Player"))
        {
            if (m_uiWorkload != null && !m_uiWorkload.IsOpen())
            {
                m_uiWorkload.OpenUI(this, m_progress);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Remove(other);

        if (m_isFixed && other.CompareTag("Player"))
        {
            if (m_uiWorkload != null)
            {
                m_uiWorkload.CloseUI();
            }
        }
        if (m_isFixed && other.CompareTag("Pet"))
        {
            if (m_uiWorkload != null)
            {
                m_uiWorkload.SetNoWorkEntry();
                m_PetCtrl = null;
            }
        }

    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
    public void FixedObject()
    {
        m_isFixed = true;
        m_detectiveAreaObj.layer = LayerMask.NameToLayer("Default");
        
        m_PetCtrl = null;
        m_navObstacle.enabled = true;
        SetColor(blue);
    }
    public void MovePlayerFarfromObject()
    {
        
        Transform playerT = GameManagerEx._inst.playerManager.transform;
        playerT.LookAt(transform);
        Vector3 forward = playerT.forward;

        forward.y = 0;

        Vector3 moveDir = -forward.normalized;

        transform.Translate(moveDir * 1, Space.World);
    }
}
