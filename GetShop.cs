using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GetShop : MonoBehaviour, IPointerClickHandler
{
    public AudioClip Add, Alert;
    public string type;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(PlayerPrefsInit.coin >= 200)
        {
            PlayerPrefsInit.coin = PlayerPrefsInit.coin - 200;

            PlayerPrefs.SetInt("Coin", PlayerPrefsInit.coin);

            if (type == "Attack")
                PlayerPrefsInit.attack = PlayerPrefsInit.attack + 1;
            else if (type == "AttackSpeed")
                PlayerPrefsInit.attack_speed = PlayerPrefsInit.attack_speed + 0.5f;
            else if(type == "Health")
                PlayerPrefsInit.health = PlayerPrefsInit.health + 1;

            PlayerPrefs.SetFloat("attack", PlayerPrefsInit.attack);
            PlayerPrefs.SetFloat("attack_speed", PlayerPrefsInit.attack_speed);
            PlayerPrefs.SetFloat("health", PlayerPrefsInit.health);

            PlayerPrefs.Save();

            Sound.PlaySoundGlobal(Add);

            GetComponent<Image>().color = Color.yellow;

            FindObjectOfType<ShopManager>().UpdateTextUI();

            StartCoroutine(colorWaiting());
        }
        else
        {
            Sound.PlaySoundGlobal(Alert);
            GetComponent<Image>().color = Color.red;
            StartCoroutine(colorWaiting());
        }
    }

    private IEnumerator colorWaiting()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<Image>().color = Color.white;
    }

}
