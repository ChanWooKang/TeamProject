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

    List<RequiredItem> m_listRequiredItem;
    float m_playerAbility;
    float m_playerAbilityWeight;
    float m_petAbilityWeight;
   

    public void OpenUI()
    {
        
        m_workloadBox.SetActive(true);
        if (m_playerAbilityWeight > 0)
            StartCoroutine(SetProgress());
        StartCoroutine(Test());
    }
    public void CloseUI()
    {
        m_workloadBox.SetActive(false);
        m_cSlider.value = 0;
    }
    public bool PressFkey()
    {
        if (Input.GetKey(KeyCode.F))
            m_playerAbilityWeight = 100f;

        //m_fSlider.value += Time.deltaTime * 100;
        //m_leftTimetxt.text = Mathf.CeilToInt((m_fSlider.maxValue - m_fSlider.value) / 100f).ToString();
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
        m_petAbilityWeight += ability;
    }


    IEnumerator SetProgress()
    {
        while (m_fSlider.value < m_fSlider.maxValue)
        {
            m_fSlider.value += (m_playerAbilityWeight + m_petAbilityWeight) * Time.deltaTime;
            if (m_petAbilityWeight > 0)
                m_leftTimetxt.text = Mathf.CeilToInt((m_fSlider.maxValue - m_fSlider.value) / (m_playerAbilityWeight + m_petAbilityWeight)).ToString();
            else
                m_leftTimetxt.text = Mathf.CeilToInt((m_fSlider.maxValue - m_fSlider.value) / (m_playerAbility)).ToString();
            yield return null;
        }
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(2f);

        SetPetWorkAbility(50f);
    }
}
