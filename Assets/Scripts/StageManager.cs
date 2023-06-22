using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public float speed;
    public float fadeDuration;
    public Transform[] backgrounds;

    float topPosY;
    float bottomPosY;
    float xScreenHalfSize;
    float yScreenHalfSize;
    int currentIndex = 0;
    int nextIndex = 0;

    bool isBackgroundChanging = false;
    bool isFirstBackgroundChange = true;

    public GameObject timerObject;
    private Timer timer_;

    public AudioClip audio1, audio2, audio3;
    AudioSource audioSource;

    void Start()
    {
        timer_ = timerObject.GetComponent<Timer>();

        yScreenHalfSize = Camera.main.orthographicSize;
        xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;

        topPosY = yScreenHalfSize * 2;
        bottomPosY = -(yScreenHalfSize * 2 * backgrounds.Length);

        // 배경 이미지의 초기 위치 설정
        for (int i = 0; i < backgrounds.Length; i++)
        {
            Vector3 startPos = new Vector3(0f, topPosY + i * yScreenHalfSize * 2, 0f);
            backgrounds[i].localPosition = startPos;
        }

        currentIndex = 0;
        nextIndex = (currentIndex + 1) % backgrounds.Length;

        if (isFirstBackgroundChange)
        {
            isFirstBackgroundChange = false;
            FadeInBackground(currentIndex);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audio1;
        audioSource.Play();
    }

    void Update()
    {
        // 배경 이미지 이동
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].localPosition = new Vector3(0, -speed, 0) * Time.deltaTime;

            if (backgrounds[i].localPosition.y < bottomPosY)
            {
                Vector3 nextPos = backgrounds[i].localPosition;
                nextPos = new Vector3(nextPos.x, nextPos.y + topPosY * backgrounds.Length, nextPos.z);
                backgrounds[i].localPosition = nextPos;
            }
        }

        if ((timer_.timer == 60 || timer_.timer == 30) && !isBackgroundChanging) // 30초마다 배경 변경
        {
            currentIndex = nextIndex;
            nextIndex = (currentIndex + 1) % backgrounds.Length;

            FadeInBackground(currentIndex);

            if (timer_.timer == 60)
            {
                audioSource.clip = audio2;
                audioSource.Play();
            } else
            {
                audioSource.clip = audio3;
                audioSource.Play();
            }
        }
    }

    void FadeInBackground(int index)
    {
        StartCoroutine(FadeBackground(index));
    }

    IEnumerator FadeBackground(int index)
    {
        isBackgroundChanging = true;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            SpriteRenderer spriteRenderer = backgrounds[i].GetComponent<SpriteRenderer>();
            if (i == index)
            {
                spriteRenderer.enabled = true;
                StartCoroutine(FadeBackgroundIn(spriteRenderer, fadeDuration));
            }
            else
            {
                StartCoroutine(FadeBackgroundOut(spriteRenderer, fadeDuration));
            }
        }

        yield return new WaitForSeconds(fadeDuration);

        isBackgroundChanging = false;
    }

    IEnumerator FadeBackgroundIn(SpriteRenderer spriteRenderer, float duration)
    {
        Color startColor = spriteRenderer.color;
        Color endColor = startColor;
        endColor.a = 1f;

        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(timer / duration);
            spriteRenderer.color = Color.Lerp(startColor, endColor, normalizedTime);
            yield return null;
        }
    }

    IEnumerator FadeBackgroundOut(SpriteRenderer spriteRenderer, float duration)
    {
        Color startColor = spriteRenderer.color;
        Color endColor = startColor;
        endColor.a = 0f;

        float timer = 0f;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(timer / duration);
            spriteRenderer.color = Color.Lerp(startColor, endColor, normalizedTime);
            yield return null;
        }

        spriteRenderer.enabled = false;
    }
}
