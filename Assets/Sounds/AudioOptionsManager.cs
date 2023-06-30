using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AudioOptionsManager : MonoBehaviour
{
    public static float MusicVolume { get; private set; }
    public static float SoundEffectsVolume { get; private set; }

    public void OnMusicSliderValueChange(float value)
    {
        MusicVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
    }

    public void OnSoundEffectsSilderValueChange(float value)
    {
        SoundEffectsVolume = value;
        AudioManager.Instance.UpdateMixerVolume();
    }
}
