﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System;
/*#if UNITY_EDITOR
using UnityEditor;
#endif*/
public class ScreenshotSaver : MonoBehaviour
{
    public NetworkSetting networkSetting = new NetworkSetting();
    public SaverSetting saverSetting = new SaverSetting();
    public PostProcessingProfile profile;
    public enum EDIT_MODE { BRIGHTNESS, SATURATION, CONTRAST};
    public EDIT_MODE editMode = EDIT_MODE.SATURATION;

    [SerializeField]
    Camera renderCamera;
    [SerializeField]
    public Texture2D picture_capture;

    public bool isSendDirect = true;

    public NetworkManager networkManager;
    public bool fileUpdate = false;

    public AudioSource audio;
    public AudioClip audio_select;
    public AudioClip audio_error;
    public bool muted = false;

    public string error = "";

    public void SetupServer()
    {
        networkManager.networkAddress = networkSetting.IP;
        networkManager.networkPort = networkSetting.Port;
        networkManager.maxConnections = networkSetting.MaxConnects;
        networkManager.StartServer();

        isSendDirect = networkSetting.IsSendDirect;
    }

    public void LoadFileSetting()
    {
        //serverSetting.Save("ServerSetting.xml");
        networkSetting = NetworkSetting.Load("NetworkSetting.xml");
        saverSetting = SaverSetting.Load("SaverSetting.xml");
    }
    public void BrightnessMoveValue(float value)
    {
        saverSetting.Brighness += value;
        SetBSC(ref saverSetting.Brighness, ref saverSetting.Saturation, ref saverSetting.Contrast);
    }
    public void SaturationMoveValue(float value)
    {
        saverSetting.Saturation += value;
        SetBSC(ref saverSetting.Brighness, ref saverSetting.Saturation, ref saverSetting.Contrast);
    }
    public void ContrastMoveValue(float value)
    {
        saverSetting.Contrast += value;
        SetBSC(ref saverSetting.Brighness, ref saverSetting.Saturation, ref saverSetting.Contrast);
    }
    public void ResetBSC()
    {
        saverSetting.ResetValue();
        SetBSC(ref saverSetting.Brighness, ref saverSetting.Saturation, ref saverSetting.Contrast);
    }
    public void SetBSC(ref float brightness, ref float saturation, ref float contrast)
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
        saverSetting.Path = path;
    }
    IEnumerator SavePicture()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            error = "";
            picture_capture = GetRTPixels(renderCamera.targetTexture);
            var png = picture_capture.EncodeToPNG();

            File.WriteAllBytes(saverSetting.Path + "/" + saverSetting.Filename + ".png", png);
            //File.WriteAllBytes(serverSetting.Filename + ".png", png);
            Debug.Log("Save :" + saverSetting.Path + "/" + saverSetting.Filename + ".png");
            fileUpdate = true;
        }catch (Exception ex)
        {
            error = ex.ToString();
        }
    }


	void Start ()
    {
        //serverSetting.Save("ServerSetting.xml");
        try
        {
            error = "";
            LoadFileSetting();
            SetBSC(ref saverSetting.Brighness, ref saverSetting.Saturation, ref saverSetting.Contrast);
            SetupServer();
        }catch(Exception ex)
        {
            error = ex.ToString();
        }
    }
    private void OnGUI()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            string debug = "";
            debug = networkManager.networkAddress + ":" + networkManager.networkPort
                + " " + "file path:" + saverSetting.Path + " " + "client connect:" + networkManager.IsClientConnected()
                +" error:"+ error;
            GUI.TextArea(new Rect(0, 0, Screen.width, Screen.height), debug);
        }
    }
    void Update ()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F2))//create new xml file
        {
            saverSetting.Path = Path.GetDirectoryName(Application.dataPath);
            networkSetting.Save("NetworkSetting.xml");
            saverSetting.Save("SaverSetting.xml");
        }

            switch (editMode)
        {
            case EDIT_MODE.BRIGHTNESS:
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    BrightnessMoveValue(-0.1f);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    BrightnessMoveValue(0.1f);
                }
                break;
            case EDIT_MODE.CONTRAST:
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    ContrastMoveValue(-0.1f);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    ContrastMoveValue(0.1f);
                }
                break;
            case EDIT_MODE.SATURATION:
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    SaturationMoveValue(-0.1f);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    SaturationMoveValue(0.1f);
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (CheckPhp(saverSetting.Path))
            {
                StartCoroutine(UploadPNG(saverSetting.Path, saverSetting.Filename, GetRTPixels(renderCamera.targetTexture)));
                Debug.Log("save to web server");
            }
            else
            {
                StartCoroutine(SavePicture());
                Debug.Log("save normal");
            }
            PlaySoundSelect();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            audio.mute = !audio.mute;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            editMode = EDIT_MODE.SATURATION;
            PlaySoundSelect();
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            editMode = EDIT_MODE.BRIGHTNESS;
            PlaySoundSelect();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            editMode = EDIT_MODE.CONTRAST;
            PlaySoundSelect();
        }
        else if (Input.GetKeyDown(KeyCode.R))//reset Brighness, Saturation and Contrast value
        {
            ResetBSC();
            PlaySoundSelect();
            Debug.Log("Reset Brighness, Saturation and Contrast value");
        }
/*      else if (Input.GetKey(KeyCode.LeftControl)&&Input.GetKeyDown(KeyCode.Space))//change path with open dialog
        {
#if UNITY_EDITOR 
            string path = EditorUtility.SaveFolderPanel("Load png Textures", "", "");
            Debug.Log(path);
            if(path!="")
            {
                serverSetting.Path = path;
                serverSetting.Save("ServerSetting.xml");
            }
            PlaySoundSelect();
#endif
        }*/
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadFileSetting();
            Debug.Log("reload path form setting sile");
            PlaySoundSelect();
        }
       
    }
    public void PlaySoundSelect()
    {
        audio.PlayOneShot(audio_select);
    }
    public void PlaySoundError()
    {
        audio.PlayOneShot(audio_error);
    }
    static public Texture2D GetRTPixels(RenderTexture rt)
    {
        RenderTexture currentActiveRT = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.filterMode = FilterMode.Point;
        RenderTexture.active = currentActiveRT;
        return tex;
    }
    public bool CheckPhp(string path)
    {
        if (path.Length >= 11)
        {
            if (path.Substring(path.Length - 4, 4) == ".php")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    IEnumerator UploadPNG(string url,string filename,Texture2D tex2d)
    {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();
      
        byte[] bytes = tex2d.EncodeToPNG();

        // Create a Web Form
        WWWForm form = new WWWForm();
        Debug.Log(url + filename);
        form.AddBinaryData("image", bytes, filename+".png", "image/png");
        // Upload to a cgi script
        using (var w = UnityWebRequest.Post(url, form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                print(w.error);
                PlaySoundError();
            }
            else
            {
                print("Finished Uploading Screenshot");
            }
        }
        fileUpdate = true;
    }

}