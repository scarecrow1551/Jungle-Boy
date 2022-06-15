using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MenuPause : MonoBehaviour, IPointerClickHandler
{
    public  bool isMainMenu,IsRestart,IsExit;
    public  GameObject setting;
    private bool toggle = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isMainMenu)
        {
            SceneManager.LoadScene(0);
        }
        else if (IsRestart)
        {
            SceneManager.LoadScene(Application.loadedLevel);
        }
        else if (IsExit)
        {
            Application.Quit();
        }
        else
        {
            PlayerScript.allPause = !PlayerScript.allPause;
            toggle = !toggle;
            setting.SetActive(toggle);
        }
    }
}
