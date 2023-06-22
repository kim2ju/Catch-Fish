using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Timer : MonoBehaviour
{
    public TextMesh tmpUgui;
    //Animator anim;
    public int timer = 90;
    int temp = 0;
    int forHidden = 0;

    public GameObject microphoneInputObject;
    private MicrophoneInput microphoneInput_;

    public GameObject bluetoothManagerObject;
    private BluetoothManager bluetoothManager_;

    public GameObject playerObject;
    private PlayerMove playerMove_;

    public GameObject alertObject;

    float initial_volume = 0.03f;


    void Awake()
    {
        tmpUgui.text = "TIME: " +  (timer.ToString());
        //anim = GetComponent<Animator>();
        microphoneInput_ = microphoneInputObject.GetComponent<MicrophoneInput>();
        bluetoothManager_ = bluetoothManagerObject.GetComponent<BluetoothManager>();
        playerMove_ = playerObject.GetComponent<PlayerMove>();
    }

    // Start is called before the first frame update
    void Start()
    {
        alertObject.SetActive(false);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (bluetoothManager_.onConnected == true)
        {
            temp += 1;
            if (temp >= 50 && timer > 0)
            {
                timer -= 1;
                tmpUgui.text = "TIME: " + (timer.ToString());
                temp = 0;
            }
            if (timer == 0)
            {
                alertObject.SetActive(true);
                forHidden += 1;
                
                if (forHidden > 100)
                {
                    if (microphoneInput_.volume < initial_volume * 2.5)
                    {
                        HandleEndingBasedOnScore();
                    }
                }
            }
        } else
        {
            initial_volume = microphoneInput_.volume;
        }
    }

    void HandleEndingBasedOnScore()
    {
        // Get the score and determine which ending to show
        int score = playerMove_.score;
        string nextSceneName = "BadEnding";

        if (forHidden > 1500)
        {
            nextSceneName = "HiddenEnding";
        } else if (score > 11)
        {
            nextSceneName = "GoodEnding";
        } else if (score > 6)
        {
            nextSceneName = "NormalEnding";
        } else
        {
            nextSceneName = "BadEnding";
        }

        SceneManager.LoadScene(nextSceneName);
    }
}