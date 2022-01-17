using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bike_BikeMinigame1 : MonoBehaviour
{
    public float speed;
    public bool isTop;
    public int indexSpeed;
    public bool isMove;
    public bool isBranchState;
    public int stage;
    public bool isBeginSpawn;
    public bool isTut;


    private void Awake()
    {
        speed = 5;
        stage = 0;
        isTop = true;
        indexSpeed = 2;
        isMove = true;
        isBranchState = false;
        isBeginSpawn = false;
        isTut = true;
        transform.position = new Vector3(-62.29f, -1.39f + 1.11f, 0);
    }

    public void GetTop()
    {
        transform.DOMoveY(transform.position.y + 1.11f, 0.1f);
        isTop = true;
    }
    public void GetBot()
    {
        transform.DOMoveY(transform.position.y - 1.11f, 0.1f);
        isTop = false;
    }
    public void GetRight()
    {
        GameController_BikeMinigame1.instance.isCameraFollow = false;
        transform.DOMoveX(transform.position.x + 2, 0.1f).OnComplete(() =>
        {
            GameController_BikeMinigame1.instance.isCameraFollow = true;
        });
        indexSpeed++;
    }
    public void GetLeft()
    {
        GameController_BikeMinigame1.instance.isCameraFollow = false;
        transform.DOMoveX(transform.position.x - 2, 0.1f).OnComplete(() =>
        {
            GameController_BikeMinigame1.instance.isCameraFollow = true;
        });
        indexSpeed--;
    }
    void DelayEndNhapNho()
    {
        transform.DOMoveY(transform.position.y - 0.3f, 0.5f);
    }

    public void DelayShowTutorial2()
    {
        GameController_BikeMinigame1.instance.canvas.transform.GetChild(0).gameObject.SetActive(true);
        Time.timeScale = 0;
        GameController_BikeMinigame1.instance.isTutorial2 = true;
    }

    private void Update()
    {
        if (isMove)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Path"))
        {
            isBranchState = true;
            if (isTut)
            {
                var tmpTut = GameController_BikeMinigame1.instance.tutorial1;
                tmpTut.transform.localPosition = new Vector3(tmpTut.transform.localPosition.x, 2.22f, tmpTut.transform.localPosition.z);
                tmpTut.SetActive(true);
                tmpTut.transform.DOLocalMoveY(tmpTut.transform.localPosition.y - 5, 1).SetEase(Ease.Linear).SetLoops(-1);
                GameController_BikeMinigame1.instance.bikeObj.isMove = false;
                GameController_BikeMinigame1.instance.isCameraFollow = false;
            }
            
        }
        if (collision.gameObject.CompareTag("Obtacle"))
        {
            isBeginSpawn = true;
            Invoke(nameof(DelayShowTutorial2), 1);
            if (!isTop)
            {
                transform.position = new Vector3(transform.position.x, -1.39f + collision.gameObject.transform.position.y + 0.54f, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, -1.39f + 1.11f + collision.gameObject.transform.position.y + 0.54f, 0);
            }
            transform.eulerAngles = Vector3.zero;
            GameController_BikeMinigame1.instance.indexCameraUp = 0;
            GameController_BikeMinigame1.instance.isDangRe = false;
        }
        else if (collision.gameObject.CompareTag("ImagePicture"))
        {
            if (!isTop)
            {
                transform.position = new Vector3(transform.position.x, -1.39f + collision.gameObject.transform.position.y + 0.54f, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, -1.39f + 1.11f + collision.gameObject.transform.position.y + 0.54f, 0);
            }
            transform.eulerAngles = Vector3.zero;
            GameController_BikeMinigame1.instance.indexCameraUp = 0;
            GameController_BikeMinigame1.instance.isDangRe = false;
        }
        if (collision.gameObject.CompareTag("Balloon"))
        {
            stage++;
            if(stage == 3 && indexSpeed != 1)
            {
                GameController_BikeMinigame1.instance.Lose();
            }
            if(stage == 4 && !isTop)
            {
                GameController_BikeMinigame1.instance.Lose();
            }
            if(stage == 5)
            {
                GameController_BikeMinigame1.instance.StageRock();
            }
        }
        if (collision.gameObject.CompareTag("Trash"))
        {
            GameController_BikeMinigame1.instance.Lose();
            isMove = false;
            transform.DOMoveX(transform.position.x - 4, 1);
        }
        if (collision.gameObject.CompareTag("Tree"))
        {
            if(indexSpeed != 1)
            {
                GameController_BikeMinigame1.instance.Lose();
                isMove = false;
                transform.DOMoveX(transform.position.x + 4, 1);
                transform.DOShakeRotation(1);
            }
            else
            {
                transform.DOMoveY(transform.position.y + 0.3f, 0.5f);
                Invoke(nameof(DelayEndNhapNho), 1.8f);
            }           
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            GameController_BikeMinigame1.instance.Lose();
            isMove = false;
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            GameController_BikeMinigame1.instance.Win();
            GetComponent<BoxCollider2D>().enabled = false;
            transform.DOMoveX(transform.position.x + 20, 2).OnComplete(() => 
            {
                Destroy(gameObject);
            });
        }
        if (collision.gameObject.CompareTag("Thief"))
        {
            GameController_BikeMinigame1.instance.Lose();
            speed = 0;
            collision.GetComponent<Enemy_BikeMinigame1>().speed = 0;
            GameController_BikeMinigame1.instance.bikeObj.transform.DOMoveX(transform.position.x - 4, 1);         
        }
        if (collision.gameObject.CompareTag("HandCuff"))
        {
            GameController_BikeMinigame1.instance.Lose();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Path"))
        {
            isBranchState = false;
        }
    }
}
