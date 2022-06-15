using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameObjectEnabler : MonoBehaviour, IPointerClickHandler
{
    public GameObject enable;
    public GameObject disable;

    public void OnPointerClick(PointerEventData eventData)
    {
        Sound.UI_Sound();
        enable.gameObject.SetActive(true);
        disable.gameObject.SetActive(false);
    }
}
