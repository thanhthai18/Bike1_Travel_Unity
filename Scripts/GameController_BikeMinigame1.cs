using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameController_BikeMinigame1 : MonoBehaviour
{
    public static GameController_BikeMinigame1 instance;

    public Camera mainCamera;
    public Bike_BikeMinigame1 bikeObj;
    public Vector2 startMousePos;
    public Vector2 endMousePos;
    public float offset;
    public bool isCameraFollow;
    private float f2;
    public float posXClampPlayer;
    public float speedCamera;
    public List<Transform> posReLen;
    public List<Transform> posReXuong;
    public int indexCameraUp;
    public int indexBringBranch;
    public bool isLose, isWin, isTutorial2;
    public GameObject rockPrefab;
    public Transform rockPos;
    public List<Transform> listPosEnemy = new List<Transform>();
    public List<Enemy_BikeMinigame1> listEnemyPrefab = new List<Enemy_BikeMinigame1>();
    public int time;
    public Text txtTime;
    public int indexSpawnEnemy;
    public bool isDangRe;
    public GameObject tutorial1;
    public Canvas canvas;
    public Button btnTutorial;
    public bool isMouseOverButton;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);

        speedCamera = 5;
        indexSpawnEnemy = 0;
        isLose = false;
        isWin = false;
        isTutorial2 = false;
        isDangRe = false;
        time = 90;
        txtTime.text = time.ToString();
    }
    private void Start()
    {
        SetSizeCamera();
        StartCoroutine(CountingTime());
        tutorial1.SetActive(false);
        isCameraFollow = true;
        isMouseOverButton = false;
        indexCameraUp = 0;
        indexBringBranch = 0;
        btnTutorial.onClick.AddListener(OnClickButtonTutorial);
    }

    void SetSizeCamera()
    {
        float f1;
        f1 = 16.0f / 9;
        f2 = Screen.width * 1.0f / Screen.height;

        mainCamera.orthographicSize *= f1 / f2;
    }
    void DelayCameraFollow()
    {
        isCameraFollow = true;
    }

    void OnClickButtonTutorial()
    {
        if (!canvas.transform.GetChild(0).gameObject.activeSelf && !canvas.transform.GetChild(1).gameObject.activeSelf)
        {
            bikeObj.DelayShowTutorial2();
            isTutorial2 = true;
        }
        else if (canvas.transform.GetChild(0).gameObject.activeSelf || canvas.transform.GetChild(1).gameObject.activeSelf)
        {
            canvas.transform.GetChild(0).gameObject.SetActive(false);
            canvas.transform.GetChild(1).gameObject.SetActive(false);
            Time.timeScale = 1;
            isTutorial2 = false;
        }
    }

    public void Lose()
    {
        isLose = true;
        isCameraFollow = false;
        Debug.Log("Thua");
        Destroy(bikeObj.gameObject, 5);
        StopAllCoroutines();
    }

    public void Win()
    {
        isWin = true;
        isCameraFollow = false;
        Debug.Log("Win");
        StopAllCoroutines();
    }

    public void StageRock()
    {
        var tmpRock = Instantiate(rockPrefab, rockPos.transform.position, Quaternion.identity);
        tmpRock.transform.DOMoveY(tmpRock.transform.position.y - 10, 2).SetEase(Ease.Linear).OnComplete(() =>
        {
            tmpRock.transform.DOMoveY(tmpRock.transform.position.y + 10, 2).SetEase(Ease.Linear).OnComplete(() =>
             {
                 Destroy(tmpRock.gameObject);
                 if (bikeObj.stage == 5 && !isLose)
                 {
                     StageRock();
                 }
             });
        });
    }

    void SpawnEnemy(int indexPos, int indexEnemy)
    {
        if (indexPos < 3 && indexPos > 0)
        {
            var tmpEnemy = Instantiate(listEnemyPrefab[indexEnemy], listPosEnemy[indexPos].position, Quaternion.identity);
            tmpEnemy.transform.eulerAngles = Vector3.zero;
            tmpEnemy.speed = 4;
        }
        else if (indexPos > 2 && indexPos < 5)
        {
            var tmpEnemy = Instantiate(listEnemyPrefab[indexEnemy], listPosEnemy[indexPos].position, Quaternion.identity);
            tmpEnemy.transform.eulerAngles = new Vector3(0, 180, 0);
            tmpEnemy.speed = 6;
            Destroy(tmpEnemy, 5);
        }
    }

    IEnumerator CountingTime()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            txtTime.text = time.ToString();
            if (time == 85)
            {
                SpawnEnemy(2, Random.Range(0, listEnemyPrefab.Count));
                ShowTutorial1();
            }
            if (bikeObj.isBeginSpawn)
            {
                indexSpawnEnemy++;
                if (indexSpawnEnemy == 10)
                {
                    indexSpawnEnemy = 0;
                    SpawnEnemy(Random.Range(0, listPosEnemy.Count), Random.Range(0, listEnemyPrefab.Count));
                }
            }
            else
            {
                indexSpawnEnemy = 0;
            }

            if (time == 0 && !isWin)
            {
                isLose = true;
                Lose();
            }
        }
    }

    void ShowTutorial1()
    {
        tutorial1.SetActive(true);
        tutorial1.transform.DOMoveY(tutorial1.transform.position.y - 6, 2).SetLoops(-1);
    }

    void HideTutorial()
    {
        if (tutorial1.activeSelf)
        {
            tutorial1.SetActive(false);
            tutorial1.transform.DOKill();
        }
    }

    private void Update()
    {
        if (isCameraFollow)
        {
            mainCamera.transform.Translate(Vector3.right * speedCamera * Time.deltaTime);
            if (indexCameraUp == 1)
            {
                mainCamera.transform.Translate(Vector3.up * speedCamera / 2 * Time.deltaTime);
            }
            else if (indexCameraUp == -1)
            {
                mainCamera.transform.Translate(Vector3.down * speedCamera / 2 * Time.deltaTime);
            }
        }

        if (Input.GetMouseButtonDown(0) && !isLose && !isWin && !isDangRe)
        {
            if (!isTutorial2)
            {
                startMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.transform.position;
            }
            else
            {
                startMousePos = Vector2.zero;
                endMousePos = Vector2.zero;
            }
            if (canvas.transform.GetChild(1).gameObject.activeSelf && !isMouseOverButton)
            {
                canvas.transform.GetChild(1).gameObject.SetActive(false);
                Time.timeScale = 1;
                isTutorial2 = false;
            }
            if (canvas.transform.GetChild(0).gameObject.activeSelf && !isMouseOverButton)
            {
                canvas.transform.GetChild(0).gameObject.SetActive(false);
                canvas.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        if (Input.GetMouseButtonUp(0) && !isLose && !isWin && !isDangRe)
        {
            if (!isTutorial2)
            {
                endMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition) - mainCamera.transform.position;
            }
            else
            {
                startMousePos = Vector2.zero;
                endMousePos = Vector2.zero;
            }
            if(startMousePos.x == endMousePos.x)
            {
                return;
            }
            if (startMousePos.x + 0.1f < endMousePos.x && Mathf.Abs(startMousePos.y - endMousePos.y) < 2)
            {
                if (bikeObj.indexSpeed < 3 && !bikeObj.isBranchState)
                {
                    Debug.Log("Phai");
                    bikeObj.GetRight();
                    if (bikeObj.indexSpeed == 2)
                    {
                        speedCamera = 5;
                        bikeObj.speed = 5;
                    }
                    else if (bikeObj.indexSpeed == 3)
                    {
                        speedCamera = 6;
                        bikeObj.speed = 6;
                    }
                }
            }
            else if (startMousePos.x > endMousePos.x + 0.1f && Mathf.Abs(startMousePos.y - endMousePos.y) < 2)
            {
                if (bikeObj.indexSpeed > 1 && !bikeObj.isBranchState)
                {
                    Debug.Log("Trai");
                    bikeObj.GetLeft();
                    if (bikeObj.indexSpeed == 2)
                    {
                        speedCamera = 5;
                        bikeObj.speed = 5;
                    }
                    else if (bikeObj.indexSpeed == 1)
                    {
                        speedCamera = 4;
                        bikeObj.speed = 4;
                    }
                }
            }
            else if (startMousePos.y + 0.1f < endMousePos.y && Mathf.Abs(startMousePos.x - endMousePos.x) < 2)
            {
                if (!bikeObj.isTop && !bikeObj.isBranchState)
                {
                    Debug.Log("Len");
                    bikeObj.GetTop();
                }
            }
            else if (startMousePos.y > endMousePos.y + 0.1f && Mathf.Abs(startMousePos.x - endMousePos.x) < 2)
            {
                HideTutorial();
                if (bikeObj.isTop && !bikeObj.isBranchState)
                {
                    Debug.Log("Xuong");
                    bikeObj.GetBot();

                }
            }
            if (bikeObj.isBranchState)
            {
                if (startMousePos.y > endMousePos.y + 0.1f)
                {
                    HideTutorial();
                    if (!bikeObj.isMove)
                    {
                        bikeObj.isMove = true;
                        isCameraFollow = true;
                    }

                    Debug.Log("Re Xuong");
                    isDangRe = true;
                    //if (bikeObj.indexSpeed != 3)
                    //{
                    //    bikeObj.GetRight();
                    //}
                    if (bikeObj.isTop)
                    {
                        bikeObj.transform.position = posReXuong[indexBringBranch].position;
                    }
                    else
                    {
                        bikeObj.transform.position = posReXuong[indexBringBranch + 1].position;
                    }
                    bikeObj.transform.eulerAngles = new Vector3(0, 0, -32.349f);
                    indexCameraUp = -1;
                    indexBringBranch += 2;
                    if (bikeObj.stage == 2)
                    {
                        Lose();
                    }
                    //if (bikeObj.indexSpeed == 3)
                    //{
                    isCameraFollow = false;
                    if (!isLose)
                    {
                        Invoke(nameof(DelayCameraFollow), 0.5f);
                    }
                    //}
                }
                if (startMousePos.y + 0.1f < endMousePos.y)
                {
                    Debug.Log("Re Len");
                    isDangRe = true;
                    //if (bikeObj.indexSpeed != 3)
                    //{
                    //    bikeObj.GetRight();
                    //}
                    if (bikeObj.isTop)
                    {
                        bikeObj.transform.position = posReLen[indexBringBranch].position;
                    }
                    else
                    {
                        bikeObj.transform.position = posReLen[indexBringBranch + 1].position;
                    }
                    bikeObj.transform.eulerAngles = new Vector3(0, 0, 32.349f);
                    indexCameraUp = 1;
                    indexBringBranch += 2;
                    if (bikeObj.stage == 1)
                    {
                        Lose();
                    }
                    //if (bikeObj.indexSpeed == 3)
                    //{
                    isCameraFollow = false;
                    if (!isLose)
                    {
                        Invoke(nameof(DelayCameraFollow), 0.5f);
                    }
                    //}
                    
                }
            }
            startMousePos = Vector2.zero;
            endMousePos = Vector2.zero;
        }
    }
}
