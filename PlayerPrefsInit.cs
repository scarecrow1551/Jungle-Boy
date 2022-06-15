using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsInit : MonoBehaviour
{
    public static float effects, music;

    public static int coin;

    public static float attack, attack_speed, health;
    void Awake()
    {

        effects      = PlayerPrefs.GetFloat("effects", 0.90f);

        music        = PlayerPrefs.GetFloat("music", 0.80f);



        coin         = PlayerPrefs.GetInt("Coin",0); // базовая монетка где в конце 1 это дефолное значение

        attack       = PlayerPrefs.GetFloat("attack", 1);  // базовая атака где в конце 1 это дефолное значение
        attack_speed = PlayerPrefs.GetFloat("attack_speed", 1);  // базовая скорость атаки где в конце 1 это дефолное значение
        health       = PlayerPrefs.GetFloat("health", 3);  // хп базовая где в конце 1 это дефолное значение

    }

}
