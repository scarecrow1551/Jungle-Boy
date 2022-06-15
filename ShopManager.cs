using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI Coin;
    public TextMeshProUGUI Attack;
    public TextMeshProUGUI Attack_Speed;
    public TextMeshProUGUI Health;

    private void Start()
    {
        UpdateTextUI();
    }

    public void UpdateTextUI()
    {
        Coin.text         = PlayerPrefsInit.coin.ToString() + "$";
        Attack.text       = "Attack : " + PlayerPrefsInit.attack.ToString();
        Attack_Speed.text = "Attack Speed : " + PlayerPrefsInit.attack_speed.ToString();
        Health.text       = "Health : " + PlayerPrefsInit.health.ToString();
    }
}
