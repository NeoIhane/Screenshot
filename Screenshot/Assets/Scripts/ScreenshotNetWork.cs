using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System;
public class ScreenshotNetWork : NetworkBehaviour
{
    [Server]
    public void sendPath(string url, string filename)
    {
        if(url=="")
            url = Path.GetDirectoryName(Application.dataPath);//Path of SaverScreenshot App

        RpcRecievePath(url,filename);
        Debug.Log("Rpc server:" + url);
    }
    [ClientRpc]
    public void RpcRecievePath(string url, string filename)
    {
        Debug.Log("Rpc client:" + url);
        StartCoroutine(LoadTextureFromUrl(url, filename));
    }
    public IEnumerator LoadTextureFromUrl(string url, string filename)
    {
        Texture2D tex2d;
        tex2d = new Texture2D(4, 4, TextureFormat.DXT1, false);
        Debug.Log("url " + url);

        string newUrl = CheckPath(url);
        if (newUrl != null)
        {
            using (WWW www = new WWW(newUrl + "/" + filename + ".png"))
            {
                yield return www;
                try
                {
                    www.LoadImageIntoTexture(tex2d);
                    UpdatedTexture(tex2d);
                }catch (Exception ex)
                {
                    UpdatedTexture(null);
                    File.WriteAllText(Application.dataPath + "LoadImageError.txt", ex.ToString());
                }
            }
        }
    }
   public static string CheckPath(string url)
    {
        string newUrl = "";
        if(url.Length>2)
        {
            if (url.Length > 7)
            {
                if (url.Substring(0, 7) == "http://")
                {
                    Debug.Log("Maybe www Path");
                    newUrl = url;
                }
                else if (url.Substring(0, 7) == "file://")
                {
                    Debug.Log("Maybe Local Path with file://");
                    newUrl = url;
                }

                if (url.Substring(1, 1) == ":")
                {
                    Debug.Log("Maybe save from Local Path");
                    newUrl = url;
                }
           
            }else
            {
                Debug.Log("Other Path");
                newUrl = url;
            }
        }
        else if (url=="")
        {
            Debug.Log("Return Application Path");
            newUrl = Path.GetDirectoryName(Application.dataPath);
        }
        else
        {
            Debug.Log("file path ["+ url +"] is not correct!");
            return null;
        }
        Debug.Log("new path is " + newUrl);
        return newUrl;
    }
    public virtual void UpdatedTexture(Texture2D texture2D)
    {

    }

    List<byte[]> bytePng = new List<byte[]>();
    [Server]
    public void SendPNG(byte[] receivedByte, int size, int i, int iend)
    {
        RpcPNG(receivedByte, size, i, iend);
        Debug.Log("Rpc server" + receivedByte.Length.ToString());
    }
    [ClientRpc]
    public void RpcPNG(byte[] receivedByte, int size, int i, int iend)
    {
        Debug.Log("Rpc i " + i.ToString());
        if (i != iend)
        {
            bytePng.Add(receivedByte);
        }
        else
        {
            bytePng.Add(receivedByte);

            byte[] b = PNGSplit.Together(bytePng, size);

            Texture2D receivedTexture = null;
            receivedTexture = new Texture2D(1, 1);
            receivedTexture.LoadImage(b);
            Debug.Log("Rpc " + b.Length.ToString());
            UpdatedTexture(receivedTexture);
            bytePng.Clear();
        }

    }

}

