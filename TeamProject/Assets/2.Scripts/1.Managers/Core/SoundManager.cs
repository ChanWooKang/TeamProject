using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DefineDatas;

public class SoundManager : TSingleton<SoundManager>
{
    public AudioMixer m_masterMixer;
    //임시    
    [SerializeField] AudioMixerGroup m_audioBGMGroup;
    [SerializeField] AudioMixerGroup m_audioSFXGroup;
    //==

    [SerializeField]PoolSoundClip[] m_BGMClips;
    [SerializeField]PoolSoundClip[] m_SFXClips;

    
    Dictionary<string, AudioClip> m_dicBGM;
    Dictionary<string, AudioClip> m_dicSFX;

    StartSoundSettingInfo m_soundSettingInfo;
    AudioSource m_bgmPlayer;
    AudioSource m_sfxPlayer;
    private void Awake()
    {
        //임시
        InitSoundData(new StartSoundSettingInfo(true, false, 1, false, false, 1), m_audioBGMGroup, m_audioSFXGroup);
    }
    private void Start()
    {
        PlayBGM("InGameScene");
    }
    void SettingBGMPlayer(StartSoundSettingInfo info)
    {
        m_bgmPlayer.loop = info.m_isLoopBGM;
        m_bgmPlayer.mute = info.m_isMuteBGM;
        m_bgmPlayer.volume = info.m_volumeBGM;
    }

    void SettingSFXPlayer(StartSoundSettingInfo info)
    {
        m_sfxPlayer.loop = info.m_isLoopSFX;
        m_sfxPlayer.mute = info.m_isMuteSFX;
        m_sfxPlayer.volume = info.m_volumeSFX;
    }

    public void InitSoundData(StartSoundSettingInfo settingInfo, AudioMixerGroup bgmMixer, AudioMixerGroup sfxMixer)
    {
        
        m_dicBGM = new Dictionary<string, AudioClip>();
        m_dicSFX = new Dictionary<string, AudioClip>();
      
        for (int i = 0; i < m_BGMClips.Length; i++)
        {
            m_dicBGM.Add(m_BGMClips[i].name, m_BGMClips[i].m_clip);            
        }
       
        for (int i = 0; i < m_SFXClips.Length; i++)
        {           
            m_dicSFX.Add(m_SFXClips[i].name, m_SFXClips[i].m_clip);
        }
       

       m_soundSettingInfo = settingInfo;

        GameObject go = new GameObject("SoundPlayer", typeof(AudioSource));
        go.transform.SetParent(transform);
        m_sfxPlayer = go.GetComponent<AudioSource>();
        m_bgmPlayer = gameObject.AddComponent<AudioSource>();

        m_bgmPlayer.playOnAwake = false;
        m_bgmPlayer.outputAudioMixerGroup = bgmMixer;
        m_sfxPlayer.playOnAwake = false;
        m_sfxPlayer.outputAudioMixerGroup = sfxMixer;
        SettingBGMPlayer(m_soundSettingInfo);
        SettingSFXPlayer(m_soundSettingInfo);
    }
    public void PlayBGM(string name)
    {        
        AudioClip clip = m_dicBGM[name];
        m_bgmPlayer.clip = clip;

        m_bgmPlayer.Play();
    }
    public void SetBGMVolume(float vol) 
    {
        m_bgmPlayer.volume = vol;
    }
    public void PlaySfx(string name, float vol = 1f) // 사운드매니저오브젝트에서 중첩되는 sfx 재생 (일반적으로 ui에 사용)
    {       
        AudioClip clip = m_dicSFX[name];

        m_sfxPlayer.PlayOneShot(clip);
    }
    public void PlaySfxAtPoint(string name, Vector3 pos) // 특정 위치에서 오디오 클립 재생 (오디오 소스 객체가 동적으로 생성)
    {        
        AudioClip clip = m_dicSFX[name];        
      
        GameObject audioObject = new GameObject("TemporaryAudio");
        audioObject.transform.position = pos;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        audioSource.outputAudioMixerGroup = m_audioSFXGroup;
        audioSource.Play();

        Destroy(audioObject, clip.length); // 클립 재생 후 오브젝트 제거
    }
    public void PlaySfxAtObject(AudioSource source, string name) // 오브젝트에서 중첩되는(혹은 될 수 있는) sfx 재생 (오브젝트가 오디오 소스를 가지고 있어야함)
    {        
        AudioClip clip = m_dicSFX[name];
        source.spatialBlend = 1f;
        source.rolloffMode = AudioRolloffMode.Logarithmic; // 거리에 따른 사운드 증감
        source.volume = m_sfxPlayer.volume;
        source.PlayOneShot(clip);
    }
}
