using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider_BikeMinigame1 : MonoBehaviour
{

    private BoxCollider2D col;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(2 * ((Screen.width * 1.0f) / Screen.height) * GetComponent<Camera>().orthographicSize, 2 * GetComponent<Camera>().orthographicSize);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            collision.GetComponent<SpriteRenderer>().DOFade(0, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
             {
                 collision.GetComponent<SpriteRenderer>().DOFade(1, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                 {
                     collision.GetComponent<SpriteRenderer>().DOFade(0, 0.3f).SetEase(Ease.Linear).OnComplete(() =>
                     {
                         collision.GetComponent<SpriteRenderer>().DOFade(1, 0.3f).SetEase(Ease.Linear);
                     });
                 });
             });
        }
    }
}
