using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
public class testloadimage : MonoBehaviour {
    public RawImage image;
    public Text tx;
    public string url;
    public string filename;
    public Texture2D tex2D;
	// Use this for initialization
	void Start ()
    {
        //StartCoroutine(Upload());
        //StartCoroutine(UploadPNG("http://localhost/upload.php", "test", tex2D));
        //StartCoroutine(LoadTextureFromUrl("file:///C:/Users/NeoIhane/Documents/GitHub/Screenshot"));
        //tx.text = Path.GetDirectoryName(Application.dataPath);
        StartCoroutine(LoadTextureFromUrl(url));	
    }

    public void UpdatedTexture(Texture2D texture2D)
    {
        image.texture = texture2D as Texture;
    }
    IEnumerator LoadTextureFromUrl(string url)
    {
        Texture2D tex2d;
        tex2d = new Texture2D(4, 4, TextureFormat.DXT1, false);
        Debug.Log("load: " + url);
        string newUrl = CheckPath(url) + "/" +filename + ".png";
        if (url != null)
        {
            using (WWW www = new WWW(newUrl))
            {
                yield return www;
                www.LoadImageIntoTexture(tex2d);
                UpdatedTexture(tex2d);
            }
        }
    }
    string CheckPath(string url)
    {
        string newUrl = "";
        if (url.Length > 3)
        {
            Debug.Log("url.Length > 3");
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
                else if (url.Substring(0, 8) == "https://")
                {
                    Debug.Log("Maybe Local Path with https://");
                    newUrl = url;
                }
            }
            if (url.Substring(1, 2) == ":/")
            {
                Debug.Log("Maybe Local Path");
                newUrl = "file://" + url;
            }
        }
        else if (url == "")
        {
            Debug.Log("Return Application Path");
            newUrl = Path.GetDirectoryName(Application.dataPath);
        }
        else
        {
            Debug.Log("file path [" + url + "] is not correct!");
            return null;
        }
        Debug.Log("new path is " + newUrl);
        
        return newUrl;
    }
    IEnumerator UploadPNG(string url, string filename, Texture2D tex2d)
    {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();

        byte[] bytes = tex2d.EncodeToPNG();

        // Create a Web Form
        WWWForm form = new WWWForm();
        Debug.Log(url + filename);
        form.AddBinaryData("image", bytes, filename + ".png", "image/png");
        // Upload to a cgi script
        using (var w = UnityWebRequest.Post(url, form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                print(w.error);
            }
            else
            {
                print("Finished Uploading Screenshot");
            }
        }
    }
    IEnumerator Upload()
    {
        byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
        UnityWebRequest www = UnityWebRequest.Put("http://localhost/upload", myData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Upload complete!");
        }
    }
}
