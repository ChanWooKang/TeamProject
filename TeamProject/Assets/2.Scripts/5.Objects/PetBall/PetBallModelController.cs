using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBallModelController : MonoBehaviour
{
    [SerializeField]
    Material[] m_materials;
    public void SetMaterial(string name)
    {
        Material mat = null; 
        switch (name)
        {
            case "PetBall":
                mat = m_materials[0];
                break;
            case "GreatBall":
                mat = m_materials[1];
                break;
            case "SuperBall":
                mat = m_materials[2];
                break;
        }
        GetComponent<MeshRenderer>().material = mat;
    }
}
