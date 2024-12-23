using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using DefineDatas;

public class UI_Workload : MonoBehaviour
{
    #region [����]
    ObjectPreview m_ObjManager;

    [SerializeField]
    GameObject m_workloadBox;
    [SerializeField]
    GameObject m_EntryPetBox;
    [SerializeField]
    GameObject m_noEntryBox;
    [SerializeField]
    TextMeshProUGUI m_leftTimetxt;
    [SerializeField]
    Slider m_fSlider;
    [SerializeField]
    Slider m_cSlider;
    [SerializeField]
    TextMeshProUGUI m_textName;
    [SerializeField]
    TextMeshProUGUI m_textWorkAbility;
    [SerializeField]
    Image m_icon;
    AudioSource m_audioSource;
    #endregion [����]

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
    public void OpenUI(ObjectPreview manager, AudioSource source, float progress)
    {
        m_ObjManager = manager;
        m_fSlider.value = progress;
        m_audioSource = source;
        m_workloadBox.SetActive(true);
    }
    public void CloseUI()
    {
        m_workloadBox.SetActive(false);
        m_cSlider.value = 0;
    }
    public bool PressFkey()
    {
        if (m_workloadBox.activeSelf && Input.GetKey(KeyCode.F))
        {
            m_playerAbilityWeight = m_playerAbility;
            m_ObjManager.MovePlayerFarfromObject();
        }


        if (m_fSlider.value >= m_fSlider.maxValue)
        {
            StopCoroutine(SetProgress());
            TechnologyManager._inst.TechPointUp();
            SoundManager._inst.PlaySfx("CraftDone");
            m_audioSource.Stop();
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
    public void SetPetWorkEntry(float ability, string nameKr, string nameEn)
    {
        StartCoroutine(SetProgress());
        m_workloadBox.SetActive(true);
        m_workloadBox.SetActive(false);
        m_noEntryBox.SetActive(false);
        m_textName.text = nameKr;
        m_textWorkAbility.text = ability.ToString();
        m_EntryPetBox.SetActive(true);
        m_petAbilityWeight += ability;
        m_icon.sprite = PoolingManager._inst._poolingIconByName[nameEn].prefab;
    }
    public void SetNoWorkEntry()
    {
        m_petAbilityWeight = 0;
        m_noEntryBox.SetActive(true);
        m_EntryPetBox.SetActive(false);
    }

    IEnumerator SetProgress()
    {
        float weight = m_playerAbilityWeight + m_petAbilityWeight;
        m_fSlider.value += weight * Time.deltaTime;
        if (weight > 0)
        {
            if (!m_audioSource.isPlaying)
                SoundManager._inst.PlaySfxAtObject(m_audioSource, "Hammering");
        }        
        if (m_petAbilityWeight > 0)
            m_leftTimetxt.text = Mathf.CeilToInt((m_fSlider.maxValue - m_fSlider.value) / (m_playerAbilityWeight + m_petAbilityWeight)).ToString();
        else
            m_leftTimetxt.text = Mathf.CeilToInt((m_fSlider.maxValue - m_fSlider.value) / (m_playerAbility)).ToString();
        m_ObjManager.Progress = m_fSlider.value;
        yield return null;
    }

}
