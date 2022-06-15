using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour, IPointerClickHandler
{
    public bool Is_Start, Is_Exit;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Is_Start)
        {
            Application.LoadLevel(1);
        }
        else if (Is_Exit)
        {
            Application.Quit();
        }
        Sound.UI_Sound();
    }
}
