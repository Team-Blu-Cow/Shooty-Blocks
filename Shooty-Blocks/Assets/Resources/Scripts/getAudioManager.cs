using UnityEngine;

public class getAudioManager : MonoBehaviour
{
    // use of this script if purely to access the audio manager singleton via UI elements

    public void Play(string in_string)
    {
        AudioManager.instance.Play(in_string);
    }

    public void Stop(string in_string)
    {
        AudioManager.instance.Stop(in_string);
    }

    public void StopAll()
    {
        AudioManager.instance.StopAll();
    }

    public void FadeOut(string in_string)
    {
        AudioManager.instance.FadeOut(in_string);
    }

    public void FadeIn(string in_string)
    {
        AudioManager.instance.FadeIn(in_string);
    }

    public void setMusicVolume(float in_vol)
    {
        AudioManager.instance.setVolume("Music Volume", in_vol);
    }

    public void setMasterVolume(float in_vol)
    {
        AudioManager.instance.setVolume("Master Volume", in_vol);
    }

    public void setSFXVolume(float in_vol)
    {
        AudioManager.instance.setVolume("SFX Volume", in_vol);
    }
}