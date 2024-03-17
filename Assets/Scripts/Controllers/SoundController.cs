//using System;
//using UnityEngine;
//using UnityEngine.Audio;
//using UnityEngine.UI;

//public class SoundController : MonoBehaviour
//{
//    [Header("Volume control")]
//    [SerializeField] Slider effectsSlider;
//    [SerializeField] Slider musicSlider;
//    [SerializeField] public AudioMixer mixer;
//    [SerializeField] AudioMixerGroup musicMixerGroup, effectsMixerGroup;
//    float effectsVolume = 0.75f, musicVolume = 0.75f;
//    [Header("All sounds")]  
//    [SerializeField] Sound[] sounds;

//    public static SoundController instance;
//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            transform.SetParent(null);
//            DontDestroyOnLoad(transform);
//        }
//        else Destroy(gameObject);

//        foreach (Sound s in sounds)
//        {                 
//            s.audioSource = gameObject.AddComponent<AudioSource>();
//            s.audioSource.clip = s.clip;
//            s.audioSource.volume = s.volume;
//            s.audioSource.pitch = s.pitch;
//            s.audioSource.loop = s.loop;
//            s.audioSource.playOnAwake = s.isPlayOnAwake;
//            switch (s.typeOfSound)
//            {
//                case TypeOfSound.Music:
//                    s.audioSource.outputAudioMixerGroup = musicMixerGroup;
//                    break;
//                case TypeOfSound.SFX:
//                    s.audioSource.outputAudioMixerGroup = effectsMixerGroup;
//                    break;
//            }
//        }
        
//    }

//    void Start()
//    {
//        effectsVolume = Progress.Instance.playerInfo.effectsVolume;
//        musicVolume = Progress.Instance.playerInfo.musicVolume;
//        effectsSlider.value = effectsVolume;
//        musicSlider.value = musicVolume;
//        Play("Background");
//    }

//    public void Play(string skinName)
//    {
//        Sound s = Array.Find(sounds, sound => sound.skinName == skinName);
//        s.audioSource.Play();
//    }

//    public Sound GetSound(string skinName)
//    {
//        Sound s = Array.Find(sounds, sound => sound.skinName == skinName);
//        return s;
//    }
//    //По кнопке
//    public void MakeClickSound()
//    {
//        Play("ButtonClick");
//    }

//    public void SetEffectsLevel()
//    {      
//        mixer.SetFloat("EffectsVolume", Mathf.Log10(effectsSlider.value) * 20);
//        effectsVolume = effectsSlider.value;
        
//    }
//    public void SetMusicLevel()
//    {
//        mixer.SetFloat("MusicVolume", Mathf.Log10(musicSlider.value) * 20);
//        musicVolume = musicSlider.value;
        
//    }
    
//    public void SaveVolumeSetting()
//    {
//        Progress.Instance.playerInfo.effectsVolume = effectsVolume;
//        Progress.Instance.playerInfo.musicVolume = musicVolume;
//        YandexSDK.Save();
//    }

//    private void OnApplicationFocus(bool focus)
//    {       
//        Silence(!focus);       
//    }
//    private void OnApplicationPause(bool pause)
//    {
//        Silence(pause);
//    }
//    void Silence(bool silence)
//    {
//        AudioListener.pause = silence;
//    }

//    public void MuteGame()
//    {
//        AudioListener.volume = 0;
//    }
//    public void UnmuteGame()
//    {
//        AudioListener.volume = 1;
//    }

//}
