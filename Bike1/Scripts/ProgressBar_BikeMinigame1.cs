using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar_BikeMinigame1 : MonoBehaviour
{
    public Image fill;
    public bool isOnProgress;
    public Bike_BikeMinigame1 bikeObj;
    private const float ROAD_LENGTH = 301.27f;
    private const float START_POS_BIKE = -62.29f;
    public GameObject bikeIcon;

    private void Start()
    {
        isOnProgress = true;
    }

    private void FixedUpdate()
    {
        if (isOnProgress && !GameController_BikeMinigame1.instance.isLose && !GameController_BikeMinigame1.instance.isWin)
        {
            float ratio = (bikeObj.transform.position.x - START_POS_BIKE) / (ROAD_LENGTH - START_POS_BIKE);
            fill.fillAmount = ratio;
            bikeIcon.GetComponent<RectTransform>().DOAnchorPosX(ratio * fill.GetComponent<RectTransform>().rect.width, 0.1f);
        }
    }
}
