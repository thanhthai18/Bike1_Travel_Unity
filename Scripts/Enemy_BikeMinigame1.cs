using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BikeMinigame1 : MonoBehaviour
{
    public float speed = 0;



    void DelayRotate()
    {
        transform.eulerAngles = new Vector3(0, 0, 32.349f);
    }
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Path"))
        {
            Invoke(nameof(DelayRotate), 0.6f);
            Destroy(gameObject, 3);
        }
    }
}
