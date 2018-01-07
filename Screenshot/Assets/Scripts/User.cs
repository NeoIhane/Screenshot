using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class User : ScreenshotNetWork
{
    public GameObject showScreenshot;
    public ScreenshotSaver screenshotSaver;
    public string filePath;
    public string filename;
   
    void Start()
    {
        
        if (isServer)
        {
            screenshotSaver = GameObject.Find("Server").GetComponent<ScreenshotSaver>();
            filePath = screenshotSaver.saverSetting.Path;
            filename = screenshotSaver.saverSetting.Filename;
            sendPath(filePath, filename);
        }else
        {
            Debug.Log("connect");
            showScreenshot = GameObject.Find("ShowScreenshot");
        }
    }
    void Update()
    {
        if (isServer)
        {
            if (screenshotSaver.fileUpdate)
            {
                if (screenshotSaver.isSendDirect)
                {
                    byte[] bytePng = screenshotSaver.picture_capture.EncodeToPNG();
                    if (bytePng.Length > 3200)
                    {
                        List<byte[]> splitfile = PNGSplit.Split(bytePng, 3200);
                        for (int i = 0; i < splitfile.Count; i++)
                        {
                            SendPNG(splitfile[i], 3200, i, splitfile.Count - 1);
                        }
                    }
                }
                else
                {
                    filePath = screenshotSaver.saverSetting.Path;
                    filename = screenshotSaver.saverSetting.Filename;
                    sendPath(filePath, filename);
                }
                screenshotSaver.fileUpdate = false;
            }
        }
    }
    public override void UpdatedTexture(Texture2D texture2D)
    {
        base.UpdatedTexture(texture2D);
        showScreenshot.GetComponent<RawImage>().texture = texture2D as Texture;
    }
 
}
