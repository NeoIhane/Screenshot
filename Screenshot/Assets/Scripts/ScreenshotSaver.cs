using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Networking;
public class ScreenshotSaver : NetworkBehaviour
{
    ServerSetting serverSetting = new ServerSetting();
    public PostProcessingProfile profile;
    public enum EDIT_MODE { BRIGHTNESS, SATURATION, CONTRAST, NONE };
    public EDIT_MODE editMode = EDIT_MODE.NONE;

    [SerializeField]
    Camera renderCamera;
    [SerializeField]
    Texture2D picture_capture;

    #region networking
    NetworkManager netMngr;
    NetworkClient client;
    int port = 4444;

    public void SetupServer()
    {
        NetworkServer.Listen(port);

    }
   /*[Command]
    void CmdTest()
    {
        
    }*/

    void SendUpdateScreenshot()
    {
        //networkView.RPC("Send", RPCMode.Others, picture_capture.EncodeToPNG());
    }
    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log(" connected from " + player.ipAddress + ":" + player.port);
    }

    #endregion
    public void LoadFileSetting()
    {
        serverSetting = ServerSetting.Load("ServerSetting.xml");
    }
    public void SetBSC(float brightness,float saturation,float contrast)
    {
        if (brightness > 100) brightness = 100;
        if (brightness < -100) brightness = -100;

        if (saturation > 1) saturation = 1;
        if (saturation < -1) saturation = -1;

        if (contrast > 1) contrast = 1;
        if (contrast < -1) contrast = -1;

        ColorGradingModel.Settings colorGrading = profile.colorGrading.settings;
        colorGrading.basic.postExposure = brightness;
        colorGrading.basic.saturation = saturation;
        colorGrading.basic.contrast = contrast;
        profile.colorGrading.settings = colorGrading;
    }
    public void ChangePath(string path)
    {
        serverSetting.Path = path;
    }
    public void SavePicture()
    {
        picture_capture = GetRTPixels(renderCamera.targetTexture);
        var png = picture_capture.EncodeToPNG();
        File.WriteAllBytes(serverSetting.Path + serverSetting.Filename + ".png", png);
       
    }


	// Use this for initialization
	void Start ()
    {
        SetupServer();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}



    //
    static public Texture2D GetRTPixels(RenderTexture rt)
    {
        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set the supplied RenderTexture as the active one
        RenderTexture.active = rt;

        // Create a new Texture2D and read the RenderTexture image into it
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.filterMode = FilterMode.Point;
        // Restorie previously active render texture
        RenderTexture.active = currentActiveRT;
        return tex;
    }
}
