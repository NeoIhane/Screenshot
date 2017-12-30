using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class ScreenshotReader : MonoBehaviour 
{
    [SerializeField]
    RawImage image;
    ClientSetting clientSetting = new ClientSetting();
	// Use this for initialization
	void Start () 
    {
        LoadFileSetting();
        AutoFixImage();
	}
	
	// Update is called once per frame
	void Update () 
    {
        AutoFixImage();
        if (Input.GetKey(KeyCode.S))
        {
            SaveScreenSetting();
        }
	}



    void LoadFileSetting()
    {
        ClientSetting.Load(Path.Combine(Application.absoluteURL, "ClientSetting.xml"));
        //SetScreen();
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
        if (clientSetting.Width != Screen.width || clientSetting.Height != Screen.height)
        {
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
        }
    }

}
