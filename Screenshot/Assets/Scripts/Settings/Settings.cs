using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
//http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer

[XmlRoot("ServerSetting")]
public class ServerSetting
{
    public string IP;
    public int Port;
    public string Path;
    public string Filename;
    public float Brighness;
    public float Saturation;
    public float Contrast;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ServerSetting));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    public static ServerSetting Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ServerSetting));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ServerSetting;
        }
    }
    public static ServerSetting LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ServerSetting));
        return serializer.Deserialize(new StringReader(text)) as ServerSetting;
    }
    public void ResetValue()
    {
        Brighness = 0;
        Saturation = 1;
        Contrast = 1;
    }
}

[XmlRoot("ClientSetting")]
public class ClientSetting
{
    public string IP;
    public int Port;
    public bool Borderless;
    public int Width;
    public int Height;
    public int Top;
    public int Left;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ClientSetting));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    public static ClientSetting Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ClientSetting));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ClientSetting;
        }
    }
    public static ClientSetting LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ClientSetting));
        return serializer.Deserialize(new StringReader(text)) as ClientSetting;
    }
}