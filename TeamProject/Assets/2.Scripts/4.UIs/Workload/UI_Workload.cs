using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineDatas;

public class UI_Workload : MonoBehaviour
{
    [SerializeField]
    GameObject m_workloadBox;
    [SerializeField]
    TextMeshProUGUI m_leftTimetxt;
    [SerializeField]
    Slider m_fSlider;
    [SerializeField]
    Slider m_cSlider;
    
    float m_playerAbility;
    float m_playerAbilityWeight = 0;
    float m_petAbilityWeight;

    private void Update()
    {
        if (m_fSlider.value < m_fSlider.maxValue)
            StartCoroutine(SetProgress());
    }
    public bool IsOpen()
    {
        if (m_workloadBox.activeSelf)
            return true;
        return false;
    }
    public void OpenUI()
    {
        m_workloadBox.SetActive(true);      
    }
    public void CloseUI()
    {
        m_workloadBox.SetActive(false);
        m_cSlider.value = 0;
    }
    public bool PressFkey()
    {
        if (Input.GetKey(KeyCode.F))
            m_playerAbilityWeight = m_playerAbility;

        if (m_fSlider.value >= m_fSlider.maxValue)
        {
            StopCoroutine(SetProgress());
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public void UpFKey()
    {
        m_playerAbilityWeight = 0f;
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
    public void SetProgressValue(float progress)
    {
        m_playerAbility = 100f;
        m_fSlider.maxValue = progress;
        m_leftTimetxt.text = (progress / m_playerAbility).ToString();
        StartCoroutine(SetProgress());
    }
    public void SetPetWorkAbility(float ability)
    {
        StartCoroutine(SetProgress());
        m_workloadBox.SetActive(true);
        m_workloadBox.SetActive(false);
        m_petAbilityWeight += ability;
    }


    IEnumerator SetProgress()
    {
        float weight = m_playerAbilityWeight + m_petAbilityWeight;
        m_fSlider.value += weight * Time.deltaTime;

        if (m_petAbilityWeight > 0)
            m_leftTimetxt.text = Mathf.CeilToInt((m_fSlider.maxValue - m_fSlider.value) / (m_playerAbilityWeight + m_petAbilityWeight)).ToString();
        else
            m_leftTimetxt.text = Mathf.CeilToInt((m_fSlider.maxValue - m_fSlider.value) / (m_playerAbility)).ToString();
        yield return null;
    }
    
}
