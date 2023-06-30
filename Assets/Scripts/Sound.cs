using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public enum AudioTypes { soundEffect, music}
    public AudioTypes audioType;

    public string name;
    public AudioClip clip;

    [Range(0, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;    

    [HideInInspector]
    public AudioSource source;
    public bool loop;
    public bool playOnAwake;
}
