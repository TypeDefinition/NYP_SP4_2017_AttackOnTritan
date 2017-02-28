using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour {

    public AudioClip sound;

    public Button button { get { return GetComponent<Button>(); } }
    public AudioSource source { get { return GetComponent<AudioSource>(); } }
	// Use this for initialization
	void Start () {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;

        button.onClick.AddListener(() => PlaySound());
	}
	void PlaySound()
    {
        source.PlayOneShot(sound);
    }

}
