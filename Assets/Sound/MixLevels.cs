using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
public class MixLevels : MonoBehaviour {
    public AudioMixer masterMixer;

    public void SetBGMVol(float volumebgm)
    {
        masterMixer.SetFloat("volBGM", volumebgm);
    }
    public void SetSFXVol(float volumesfx)
    {
        masterMixer.SetFloat("volSFX", volumesfx);
    }

}
