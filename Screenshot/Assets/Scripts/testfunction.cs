using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.PostProcessing;
//using UnityEditor;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR 
using UnityEditor;
#endif
public class testfunction : MonoBehaviour {


    public Camera camera;
    public RenderTexture rt;

	// Use this for initialization
	void Start () 
    {
        //Setting Windows
        //ref: https://docs.unity3d.com/400/Documentation/ScriptReference/PlayerSettings.html
        //PlayerSettings.resizableWindow = true;
        //string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        Screen.SetResolution(512,512,false);

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        //string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
        //var png = rt.EncodeToPNG();
        //File.WriteAllBytes(path + "testpng", png);



#endif


        /*ServerSetting ss = new ServerSetting();
        ss.Brighness = 1;
        ss.Contrast = 1;
        ss.Saturation = 0;
        ss.Path = Application.absoluteURL;//Application.persistentDataPath;
        ss.Filename = "ServerSetting.xml";
        Debug.Log(ss.Path+ss.Filename);
        ss.Save(Path.Combine(ss.Path, ss.Filename));

        ClientSetting cs = new ClientSetting();
        cs.Borderless = true;
        cs.Width = 100;
        cs.Height = 100;
        cs.Top = 0;
        cs.Left = 0;

        cs.Save(Path.Combine(ss.Path, "ClientSetting.xml"));*/

        Debug.Log(Display.displays[0].systemWidth);
      
	}
    public PostProcessingProfile profile;
    public enum EDIT_MODE { EV, HUE, SAT, CONTRAST, NONE };
    public EDIT_MODE emode = EDIT_MODE.HUE;
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.Return))
        {
            //ScreenCapture.CaptureScreenshot("testpng.png",2);

            var png = GetRTPixels(camera.targetTexture).EncodeToPNG();
            File.WriteAllBytes("rt.png", png);
            Debug.Log("enter");
        }
        if (Input.GetKey(KeyCode.B))
        {
            emode = EDIT_MODE.EV;
        }
        if (Input.GetKey(KeyCode.S))
        {
            emode = EDIT_MODE.SAT;
        }
        if (Input.GetKey(KeyCode.C))
        {
            emode = EDIT_MODE.CONTRAST;
        }
        if (Input.GetKey(KeyCode.R))
        {
            ColorGradingModel.Settings gm = profile.colorGrading.settings;
            gm.basic.hueShift = 0f;
            gm.basic.saturation = 1f;
            gm.basic.contrast = 1f;
            profile.colorGrading.settings = gm;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float max = 0;
            float min = 0;
            float _value = 0;
            ColorGradingModel.Settings gm = profile.colorGrading.settings;

            switch(emode)
            {
                case EDIT_MODE.HUE:
                    max = 180;
                    min = -180;
                    _value = gm.basic.hueShift;
                    if (_value >= max) _value = max;
                    else _value++;
                    gm.basic.hueShift = _value;
                    break;
                case EDIT_MODE.SAT:
                    max = 2;
                    min = 0;
                    _value = gm.basic.saturation;
                    if (_value >= max) _value = max;
                    else _value+=0.1f;
                    gm.basic.saturation = _value;
                    break;
                case EDIT_MODE.CONTRAST:
                    max = 2;
                    min = 0;
                    _value = gm.basic.contrast;
                    if (_value >= max) _value = max;
                    else _value+=0.1f;
                    gm.basic.contrast = _value;
                    break;
            }
            profile.colorGrading.settings = gm;

        }else if (Input.GetKey(KeyCode.RightArrow))
        {
            float max = 0;
            float min = 0;
            float _value = 0;
            ColorGradingModel.Settings gm = profile.colorGrading.settings;

            switch (emode)
            {
                case EDIT_MODE.HUE:
                    max = 180;
                    min = -180;
                    _value = gm.basic.hueShift;
                    if (_value <= min) _value = min;
                    else _value--;
                    gm.basic.hueShift = _value;
                    break;
                case EDIT_MODE.SAT:
                    max = 2;
                    min = 0;
                    _value = gm.basic.saturation;
                    if (_value <= min) _value = min;
                    else _value -= 0.1f;
                    gm.basic.saturation = _value;
                    break;
                case EDIT_MODE.CONTRAST:
                    max = 2;
                    min = 0;
                    _value = gm.basic.contrast;
                    if (_value <= min) _value = min;
                    else _value -= 0.1f;
                    gm.basic.contrast = _value;
                    break;
            }
            profile.colorGrading.settings = gm;
        }
	}
    //use unity version 2017.3.0f3
    //Assets: PostProcessing, StandardAsset 
    //https://docs.unity3d.com/Manual/PostProcessingWritingEffects.html
    //https://docs.unity3d.com/ScriptReference/Color.HSVToRGB.html
    //ref:https://docs.unity3d.com/ScriptReference/RenderTexture-active.html
    //https://answers.unity.com/questions/1355103/modifying-the-new-post-processing-stack-through-co.html
    //https://stackoverflow.com/questions/35277880/how-to-send-a-photo-over-network-with-unity
    //https://docs.unity3d.com/ScriptReference/Screen.SetResolution.html
    //https://docs.unity3d.com/ScriptReference/Display.html
    //https://support.unity3d.com/hc/en-us/articles/115001276723-Fullscreen-options-Exclusive-Mode-vs-Fullscreen-Window-Borderless-
    static public Texture2D GetRTPixels(RenderTexture rt)
    {
        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set the supplied RenderTexture as the active one
        RenderTexture.active = rt;

        // Create a new Texture2D and read the RenderTexture image into it
        Texture2D tex = new Texture2D(rt.width, rt.height,TextureFormat.ARGB32,false);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.filterMode = FilterMode.Point;
        // Restorie previously active render texture
        RenderTexture.active = currentActiveRT;
        return tex;
    }
}
