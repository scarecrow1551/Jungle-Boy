using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    private static AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefsInit.effects;

    }

    public static void PlaySound(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position,audioSource.volume);
        
    }
    public static void PlaySoundGlobal(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public static void UI_Sound()
    {
        AudioClip clip = Resources.Load("Sound/Click") as AudioClip;

        audioSource.PlayOneShot(clip);
    }
}
