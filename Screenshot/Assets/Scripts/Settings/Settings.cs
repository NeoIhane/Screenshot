using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
//http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer

[XmlRoot("SaverSetting")]
public class SaverSetting
{
    public string Path = "";
    public string Filename = "Screenshot";
    public float Brighness = 0;
    public float Saturation = 1;
    public float Contrast = 1;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(SaverSetting));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    public static SaverSetting Load(string path)
    {
        var serializer = new XmlSerializer(typeof(SaverSetting));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as SaverSetting;
        }
    }
    public static SaverSetting LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(SaverSetting));
        return serializer.Deserialize(new StringReader(text)) as SaverSetting;
    }
    public void ResetValue()
    {
        Brighness = 0;
        Saturation = 1;
        Contrast = 1;
    }
}

[XmlRoot("ReaderSetting")]
public class ReaderSetting
{
    public bool Borderless = false;
    public int Width = 500;
    public int Height = 500;
    public int Top = 0;
    public int Left = 0;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ReaderSetting));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    public static ReaderSetting Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ReaderSetting));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ReaderSetting;
        }
    }
    public static ReaderSetting LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ReaderSetting));
        return serializer.Deserialize(new StringReader(text)) as ReaderSetting;
    }
}
[XmlRoot("NetworkSetting")]
public class NetworkSetting
{
    public string IP = "127.0.0.1";
    public int Port = 7777;
    public int MaxConnects = 10;
    public bool IsSendDirect = false;

    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(NetworkSetting));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }
    public static NetworkSetting Load(string path)
    {
        var serializer = new XmlSerializer(typeof(NetworkSetting));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as NetworkSetting;
        }
    }
    public static NetworkSetting LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(NetworkSetting));
        return serializer.Deserialize(new StringReader(text)) as NetworkSetting;
    }
}