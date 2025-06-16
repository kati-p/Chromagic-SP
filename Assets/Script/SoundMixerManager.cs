using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private AudioClip enterSFX;

    public Slider masterSlider;
    public Slider SFXSlider;
    public Slider musicSlider;

    private void Start()
    {
        float masterVol, sfxVol, musicVol;

        if (audioMixer.GetFloat("MasterVolume", out masterVol))
        {
            masterSlider.value = Mathf.Pow(10, masterVol / 20f);
        }

        if (audioMixer.GetFloat("SFXVolume", out sfxVol))
        {
            SFXSlider.value = Mathf.Pow(10, sfxVol / 20f);
        }

        if (audioMixer.GetFloat("MusicVolume", out musicVol))
        {
            musicSlider.value = Mathf.Pow(10, musicVol / 20f);
        }
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }

    public void PlayEnterSFX()
    {
        SoundManager.Instance.PlaySoundFXClip(enterSFX, 1f, transform);
    }
}
