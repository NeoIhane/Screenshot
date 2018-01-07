using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("SaverSetting")]
public class SaverSetting
{
    public string Path;
    public string Filename;
    public float Brighness;
    public float Saturation;
    public float Contrast;

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
    public bool Borderless;
    public int Width;
    public int Height;
    public int Top;
    public int Left;

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
    public string IP;
    public int Port;
    public int MaxConnects;
    public bool IsSendDirect;

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