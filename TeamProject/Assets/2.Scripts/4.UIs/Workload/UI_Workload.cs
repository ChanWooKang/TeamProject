using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Workload : MonoBehaviour
{
    Canvas m_canvas;
    Camera m_mainCam;
    [SerializeField]
    TextMeshProUGUI m_leftTimetxt;
    [SerializeField]
    Slider m_fSlider;
    [SerializeField]
    Slider m_cSlider;
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
        m_fSlider.value = 0;
        m_cSlider.value = 0;
    }
    public bool PressFkey()
    {
        m_fSlider.value += Time.deltaTime;
        if(m_fSlider.value >= 1)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    public void UpFKey()
    {
        m_fSlider.value = 0;
    }
    public bool PressCKey()
    {
        m_cSlider.value += Time.deltaTime;
        if (m_cSlider.value >= 1)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
    public void UpCKey()
    {
        m_cSlider.value = 0;
    }
    void SetUILookAtCamera()
    {
        transform.LookAt(m_mainCam.transform);
    }
}
