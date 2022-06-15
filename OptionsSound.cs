using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsSound : MonoBehaviour, IPointerClickHandler
{
    public GameObject enable;
    public GameObject disable;

    public Slider effects, music;

    private Sound _sound;
    private Music _music;

    private void Start()
    {
        effects.value = PlayerPrefsInit.effects;
        music.value   = PlayerPrefsInit.music;

        _sound = FindObjectOfType<Sound>();
        _music = FindObjectOfType<Music>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        PlayerPrefs.SetFloat("effects", PlayerPrefsInit.effects);
        PlayerPrefs.SetFloat("music",   PlayerPrefsInit.music);
        PlayerPrefs.Save();

        Sound.UI_Sound();

        enable.gameObject.SetActive(true);
        disable.gameObject.SetActive(false);

    }

    private void FixedUpdate()
    {
        PlayerPrefsInit.effects = effects.value;
        PlayerPrefsInit.music   = music.value;

        _sound.GetComponent<AudioSource>().volume = PlayerPrefsInit.effects;
        _music.GetComponent<AudioSource>().volume = PlayerPrefsInit.music;
    }

}
