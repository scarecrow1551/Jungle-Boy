using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndGameScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI Record_TMP;
    public TextMeshProUGUI Current_TMP;
    public TextMeshProUGUI CollectedCoins_TMP;
    public TextMeshProUGUI MonstersKilled_TMP;

    private void Start()
    {
        Record_TMP.text  = "Record: " + PlayerPrefs.GetInt("Record").ToString() + "M";

        Current_TMP.text = "Current : " +FindObjectOfType<GameManager>()._distance().ToString() + "M";

        CollectedCoins_TMP.text = "collected coins: " + FindObjectOfType<GameManager>()._coin().ToString();

        MonstersKilled_TMP.text = "monsters killed: " + FindObjectOfType<GameManager>()._monsters().ToString();

    }

}
