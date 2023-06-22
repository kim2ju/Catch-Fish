using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    public float sensitivity = 100f;

    private AudioSource audioSource;
    public float volume = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Get available microphones
        string[] devices = Microphone.devices;
        if (devices.Length > 0)
        {
            // Start recording from the first available microphone
            audioSource.clip = Microphone.Start(devices[0], true, 1, AudioSettings.outputSampleRate);
            audioSource.loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate volume based on the microphone input
        volume = GetVolume();

        // Use the volume value as desired (e.g., display it, trigger events, etc.)
    }

    float GetVolume()
    {
        float[] samples = new float[audioSource.clip.samples];
        audioSource.clip.GetData(samples, 0);

        float sum = 0f;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }

        float average = sum / samples.Length;
        float volume = average * sensitivity;

        return volume;
    }
}