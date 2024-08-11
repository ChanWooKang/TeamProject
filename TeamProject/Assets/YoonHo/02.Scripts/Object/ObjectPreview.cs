using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPreview : MonoBehaviour
{
    [SerializeField] List<Collider> colliderList = new List<Collider>(); // �浹�� ������Ʈ�� ������ ����Ʈ

    [SerializeField]
    int layerGround; // ���� ���̾� (�����ϰ� �� ��)
    const int IGNORE_RAYCAST_LAYER = 2;  // ignore_raycast (�����ϰ� �� ��)

    [SerializeField] bool m_isFixed;

    [SerializeField]
    Material green;
    [SerializeField]
    Material red;
    [SerializeField]
    Material blue;
    private void Awake()
    {
        m_isFixed = false;
    }
    void Update()
    {        
            ChangeColor();
    }

    private void ChangeColor()
    {
        if (m_isFixed)
            return;
        if (colliderList.Count > 0)
            SetColor(red);
        else
            SetColor(green);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Add(other);

        if(other.gameObject.tag == "Player")
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER)
            colliderList.Remove(other);
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
    public void FixedObject()
    {
        m_isFixed = true;
        SetColor(blue);
    }
}
