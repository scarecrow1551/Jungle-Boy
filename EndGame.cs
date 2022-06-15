using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndGame : MonoBehaviour, IPointerClickHandler
{
    public bool Is_MainMenu, Is_Restart, Is_Exit;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Is_MainMenu)
        {
            Application.LoadLevel(0);
        }
        else if (Is_Restart)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else if (Is_Exit)
        {
            Application.Quit();
        }
        Sound.UI_Sound();
    }
}
