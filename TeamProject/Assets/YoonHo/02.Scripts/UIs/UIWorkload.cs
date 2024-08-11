using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorkload : MonoBehaviour
{
    Canvas m_canvas;
    Camera m_mainCam;
    void Start()
    {
        m_mainCam = Camera.main;
        m_canvas = GetComponent<Canvas>();
        m_canvas.worldCamera = m_mainCam;
    }
    void Update()
    {
        SetUILookAtCamera();
    }
    public void OpenUI()
    {
        gameObject.SetActive(true);
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
    void SetUILookAtCamera()
    {
        transform.LookAt(m_mainCam.transform);
    }
}
