using System;
using System.Collections;
//using System.Runtime.InteropServices;
//using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class ScreenshotReader : MonoBehaviour 
{
    [SerializeField]
    RawImage image;
    ClientSetting clientSetting = new ClientSetting();
    int port = 4444;
    string ip = "127.0.0.1";


    NetworkClient client;

    [RPC]
    void SendTextures(byte[] receivedByte)
    {
        Texture2D receivedTexture = null;
        receivedTexture = new Texture2D(2048, 2048);
        receivedTexture.LoadImage(receivedByte);
        image.texture = receivedTexture;
    }

    void SetupClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect,OnConnected);
        client.Connect(ip,port);
    }
    void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connnected");
    }
    // Use this for initialization
    void Start () 
    {
        LoadFileSetting();
        SetScreen();
        AutoFixImage();

        SetupClient();
    }
    // Update is called once per frame
    void Update()
    {
        AutoFixImage();
    }



    void LoadFileSetting()
    {
        //Debug.Log(Path.Combine(Application.absoluteURL, "ClientSetting.xml").ToString());
        clientSetting = ClientSetting.Load(Path.Combine(Application.absoluteURL, "ClientSetting.xml"));
        
    }
    void SetScreen()
    {
        Screen.SetResolution(clientSetting.Width, clientSetting.Height, false);
    }
    void SaveScreenSetting()
    {
       clientSetting.Save(Path.Combine(Application.absoluteURL, "ClientSetting.xml"));
    }
    void AutoFixImage()
    {
        //if (clientSetting.Width != Screen.width || clientSetting.Height != Screen.height)
        //{
            if (Screen.width > Screen.height)
            {
                image.rectTransform.sizeDelta = new Vector2((float)Screen.width, (float)Screen.width);
            }
            else
            {
                image.rectTransform.sizeDelta = new Vector2((float)Screen.height, (float)Screen.height);
            }
            clientSetting.Width = Screen.width;
            clientSetting.Height = Screen.height;
        //}
    }

}
