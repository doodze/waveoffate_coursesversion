using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _soundEffectsMixerGroup;

    public List<Sound> sounds;    

    private static AudioManager _instance;

    public static AudioManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);               
            }
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            switch (sound.audioType)
            {
                case Sound.AudioTypes.soundEffect:
                    sound.source.outputAudioMixerGroup = _soundEffectsMixerGroup;
                    break;
                case Sound.AudioTypes.music:
                    sound.source.outputAudioMixerGroup = _musicMixerGroup;
                    break;               
            }

            sound.source.playOnAwake = sound.playOnAwake;
        }
        
        Play("MainTheme");
    }

    public void Play(string name)
    {
        var sound = sounds.Find(s => s.name == name);

        if (sound == null)
            return;
        
        sound.source.Play();
    }
    public void Stop(string name)
    {
        var sound = sounds.Find(s => s.name == name);

        if (sound == null)
            return;

        sound.source.Stop();
    }

    public void UpdateMixerVolume()
    {
        _musicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(AudioOptionsManager.MusicVolume) * 20);
        _soundEffectsMixerGroup.audioMixer.SetFloat("Sound Effects Volume", Mathf.Log10(AudioOptionsManager.SoundEffectsVolume) * 20);
    }

    public void PlaySound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }
}
