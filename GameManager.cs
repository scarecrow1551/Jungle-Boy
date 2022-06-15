using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [Header("Звуки")]
    public  AudioClip attack_sound;

    public  AudioClip jump_sound;

    public  AudioClip spike_sound;

    public  AudioClip coin_sound;

    public  AudioClip lose_Sound;

    [Header("Тексты")]
    public TextMeshProUGUI coin_TMP;
    public TextMeshProUGUI distance_TMP;

    [Header("Префабы")]

    public GameObject End_Game_Prefab;

    private PlayerScript player_controller;

    private CameraShake  camera_script;

    private int collected_coins;

    private int monsters_killed;

    private float cur_distance;
    void Start()
    {
        player_controller = FindObjectOfType<PlayerScript>();

        camera_script = FindObjectOfType<CameraShake>();


        coin_TMP.text = PlayerPrefs.GetInt("Coin").ToString() + "$"; // обновляем UI текст в панельке.

        // 
        PlayerPrefs.GetInt("Record", 0);
        PlayerPrefs.GetInt("CollectedCoins", 0);
        PlayerPrefs.GetInt("MonstersKilled", 0);

    }


    public void AddCoin()
    {
        collected_coins = collected_coins + 1;
        PlayerPrefsInit.coin = PlayerPrefsInit.coin + 1;
        PlayerPrefs.SetInt("Coin", PlayerPrefsInit.coin); // сохраняем монету, и даем + 1, при подбирании.
        PlayerPrefs.Save(); // сохраняем монеты.

        Sound.PlaySoundGlobal(coin_sound); // воспроизводим звук монет.
        coin_TMP.text = PlayerPrefs.GetInt("Coin").ToString() + "$"; // обновляем UI текст в панельке.
    }

    public void Monster_Kill()
    {
        monsters_killed = monsters_killed + 1;
    }

    public void PlaySpike_Sound()
    {
        Sound.PlaySoundGlobal(spike_sound);
    }

    public void PlayAttackSound()
    {
        Sound.PlaySoundGlobal(attack_sound);
    }

    public void PlayJumpSound()
    {
        Sound.PlaySoundGlobal(jump_sound);
    }

    public void CameraShake(float duration,float amount, float factor)
    {
        camera_script.Shake(duration, amount, factor);
    }

    public IEnumerator end_game()
    {
        PlayerScript.isDeath = true;

        Sound.PlaySoundGlobal(lose_Sound);

        yield return new WaitForSeconds(1f);

        End_Game_Prefab.SetActive(true);
    }

    public void End_Game()
    {
        StartCoroutine(end_game());
    }

    public int _distance() { return (int)cur_distance; }
    public int _coin() { return collected_coins; }
    public int _monsters() { return monsters_killed; }

    public void Add_Score( float distance )
    {
        if (distance > cur_distance)
        {
            cur_distance = distance;

            if (cur_distance > PlayerPrefs.GetInt("Record", 0))
            {
                PlayerPrefs.SetInt("Record", (int)cur_distance);
            }
        }

        distance_TMP.text = ((int)cur_distance).ToString() + "M";
    }


}
