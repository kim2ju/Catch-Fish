using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class EndingController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoPlayer videoPlayer2;

    int videoNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer2.loopPointReached += OnVideoEnd;
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (videoNum == 0)
        {
            videoPlayer2.Play();
            videoNum += 1;
        } else
        {
            SceneManager.LoadScene("Ready");
        }
    }

}
