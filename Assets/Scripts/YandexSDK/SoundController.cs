using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [Header("Volume control")]
    [SerializeField] public AudioMixer mixer;
    [SerializeField] AudioMixerGroup musicMixerGroup, effectsMixerGroup, masterMixerGroup;
    float sfxVolume, musicVolume;
    [Header("All sounds")]  
    [SerializeField] Sound[] sounds;
    [Header("UI")]
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    [SerializeField] 
    private Canvas settingsCanvas;

    private void Awake()
    {
        sfxSlider.minValue = 0.001f;
        sfxSlider.maxValue = 1;
        musicSlider.minValue = 0.001f;
        musicSlider.maxValue = 1;

        foreach (Sound s in sounds)
        {                 
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
            s.audioSource.playOnAwake = s.isPlayOnAwake;
            switch (s.typeOfSound)
            {
                case TypeOfSound.Music:
                    s.audioSource.outputAudioMixerGroup = musicMixerGroup;
                    break;
                case TypeOfSound.SFX:
                    s.audioSource.outputAudioMixerGroup = effectsMixerGroup;
                    break;
            }
        }
    }

    private void OnEnable()
    {
        sfxSlider.onValueChanged.AddListener(OnSfxSlideValueChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSlideValueChanged);
    }
    private void OnDisable()
    {
        sfxSlider.onValueChanged.RemoveListener(OnSfxSlideValueChanged);
        musicSlider.onValueChanged.RemoveListener(OnMusicSlideValueChanged);
    }
    void Start()
    {
        sfxVolume = Bank.Instance.playerInfo.effectsVolume;
        mixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        sfxSlider.value = sfxVolume;

        musicVolume = Bank.Instance.playerInfo.musicVolume;
        mixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        musicSlider.value = musicVolume;

        Play("Background");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Play();
    }
    public void StopPlay(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Stop();
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s;
    }
    //По кнопке
    public void MakeClickSound()
    {
        Play("ButtonClick");
    }
   
    //По кнопке Закрыть настройки
    public static void SaveVolumeSetting()
    {
        YandexSDK.Save();
    }

    private void OnApplicationFocus(bool focus)
    {       
        Silence(!focus);
#if !UNITY_EDITOR
        if(AdvManager.isAdvOpen)
            return;
        if (!focus)
            Time.timeScale = 0;
        else Time.timeScale = 1;
#endif
    }
    private void OnApplicationPause(bool pause)
    {
        Silence(pause);
#if !UNITY_EDITOR
        if(AdvManager.isAdvOpen)
            return;
        if (pause)
            Time.timeScale = 0;
        else Time.timeScale = 1;
#endif
    }
    void Silence(bool silence)
    {
        AudioListener.pause = silence;
    }

    public void MuteGame()
    {
        AdvManager.isAdvOpen = true;
        AudioListener.volume = 0;
        Time.timeScale = 0;
    }
    public void UnmuteGame()
    {
        AdvManager.isAdvOpen = false;
        AudioListener.volume = 1;
        Time.timeScale = 1;
    }

    void OnSfxSlideValueChanged(float newValue)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(newValue) * 20);
        sfxVolume = newValue;
        Bank.Instance.playerInfo.effectsVolume = sfxVolume;
    }
    void OnMusicSlideValueChanged(float newValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(newValue) * 20);
        musicVolume = newValue;
        Bank.Instance.playerInfo.musicVolume = musicVolume;
    }
}
