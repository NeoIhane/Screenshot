using System;
using System.Collections;
//using System.Runtime.InteropServices;
//using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
public class ScreenshotReader : MonoBehaviour
{
    [SerializeField]
    RawImage image;
    ReaderSetting readerSetting = new ReaderSetting();
    NetworkSetting networkSetting = new NetworkSetting();
    public NetworkManager networkManager;

    void SetupClient()
    {
        networkManager.networkAddress = networkSetting.IP;
        networkManager.networkPort = networkSetting.Port;
        networkManager.maxConnections = networkSetting.MaxConnects;
        networkManager.StartClient();   
    }
    



    public void UpdateImage(Texture2D picture_capture)
    {
        image.texture = picture_capture;
    }
    // Use this for initialization
    void Start () 
    {
        LoadFileSetting();
        //SetScreen();
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
        networkSetting = NetworkSetting.Load(Path.Combine(Application.absoluteURL, "NetworkSetting.xml"));
        
    }
    void SetScreen()
    {
        Screen.SetResolution(readerSetting.Width, readerSetting.Height, false);
    }
    void SaveScreenSetting()
    {
        readerSetting.Save(Path.Combine(Application.absoluteURL, "ReaderSetting.xml"));
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
                readerSetting.Width = Screen.width;
                readerSetting.Height = Screen.height;
        //}
    }

}
