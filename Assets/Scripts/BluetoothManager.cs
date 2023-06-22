using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using ArduinoBluetoothAPI;

public class BluetoothManager : MonoBehaviour
{
    BluetoothHelper bluetoothHelper_;
    private string deviceName_;  // ���� �� ���� ���� 1234

    public GameObject sphere_;
    /* --TMP ������Ʈ �ޱ�-- */

    public string received_message_ = "0";
    public bool onConnected = false;

    // Start is called before the first frame update
    void Start()
    {
        /* ���� ���� �������� ���� ���� �־�� �մϴ�! */

        Debug.Log("Waiting..");
        /* --tmp text�� ����ϱ�-- */

        deviceName_ = "catchfish";  // ���� 6���� "HC-05", ���� 4���� "HC-06" �Է�

        try
        {
            bluetoothHelper_ = BluetoothHelper.GetInstance(deviceName_);
            bluetoothHelper_.OnConnected += OnBluetoothConnected;
            bluetoothHelper_.OnConnectionFailed += OnBluetoothConnectionFailed;
            bluetoothHelper_.OnDataReceived += OnBluetoothMessageReceived;

            bluetoothHelper_.setTerminatorBasedStream("\n");    // \n�� �ٹٲ��� �ǹ���. �ø��� ���ο��� �� �ٲ� ������ �����͸� �и��ؼ� �޾ƿ�

            if (bluetoothHelper_.isDevicePaired())
            {
                Debug.Log("Paired");
                /* --tmp text�� ����ϱ�-- */
            }

            bluetoothHelper_.Connect();
        }
        catch (Exception ex)
        {
            sphere_.GetComponent<Renderer>().material.color = Color.yellow;
            Debug.Log(ex.Message);
        }
    }

    void OnBluetoothConnected()
    {
        sphere_.GetComponent<Renderer>().material.color = Color.green;
        Debug.Log("Connected");
        /* --tmp text�� ����ϱ�-- */
        try
        {
            bluetoothHelper_.StartListening();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    void OnBluetoothConnectionFailed()
    {
        sphere_.GetComponent<Renderer>().material.color = Color.red;
        Debug.Log("Connection Failed");
        /* --tmp text�� ����ϱ�-- */
    }

    void OnBluetoothMessageReceived()
    {
        received_message_ = bluetoothHelper_.Read();
        sphere_.GetComponent<Renderer>().material.color = Color.blue;
        onConnected = true;
        /* --tmp text�� ����ϱ�-- */
    }

    void OnDestroy()
    {
        if (bluetoothHelper_ != null)
            bluetoothHelper_.Disconnect();
    }

    void OnApplicationQuit()
    {
        if (bluetoothHelper_ != null)
            bluetoothHelper_.Disconnect();
    }
}
