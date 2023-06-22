using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroungMove : MonoBehaviour
{
    public float speed;
    public GameObject[] background1;
    public GameObject[] background2;
    public GameObject[] background3;

    private GameObject[] currentBackground;
    private int currentIndex = 0;
    private float leftPosX;
    private float rightPosX;
    private float xScreenHalfSize;
    private float yScreenHalfSize;
    private bool isFirstBackgroundChange = true;

    public GameObject timerObject;
    private Timer timer_;

    // Start is called before the first frame update
    void Start()
    {

        timer_ = timerObject.GetComponent<Timer>();

        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        leftPosX = -(xScreenHalfSize * 2);
        rightPosX = xScreenHalfSize * 2 * background1.Length;

        // 시작할 때 background1에서 시작
        currentBackground = background1;
        SetActiveBackground(currentBackground, true);

        // 나머지 배경은 비활성화
        SetActiveBackground(background2, false);
        SetActiveBackground(background3, false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((timer_.timer == 60 || timer_.timer == 30) && isFirstBackgroundChange)
        {
            isFirstBackgroundChange=false;
            // 다음 배열로 변경
            if (currentBackground == background1)
            {
                SetActiveBackground(currentBackground, false);
                currentBackground = background2;
            }
            else if (currentBackground == background2)
            {
                SetActiveBackground(currentBackground, false);
                currentBackground = background3;
            }
            else
            {
                SetActiveBackground(currentBackground, false);
                currentBackground = background1;
            }

            // 인덱스 초기화
            currentIndex = 0;

            SetActiveBackground(currentBackground, true);
        } else
        {
            isFirstBackgroundChange = false;
        }

        for (int i = 0; i < currentBackground.Length; i++)
        {
            currentBackground[i].transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;

            // 화면 가장자리에 도달하면 다음 요소로 변경
            if (currentBackground[i].transform.position.x < leftPosX)
            {
                Vector3 nextPos = currentBackground[i].transform.position;
                nextPos = new Vector3(nextPos.x + rightPosX * 2, nextPos.y, nextPos.z);
                currentBackground[i].transform.position = nextPos;

                // 다음 요소로 변경
                currentIndex = (currentIndex + 1) % currentBackground.Length;
            }
        }
    }

    // 선택된 배경을 활성화 또는 비활성화합니다.
    private void SetActiveBackground(GameObject[] background, bool isActive)
    {
        foreach (GameObject obj in background)
        {
            obj.SetActive(isActive);
        }
    }
}
