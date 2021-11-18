using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnTutorialOnMouse_BikeMinigame1 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController_BikeMinigame1.instance.isMouseOverButton = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameController_BikeMinigame1.instance.isMouseOverButton = false;
    }
}

