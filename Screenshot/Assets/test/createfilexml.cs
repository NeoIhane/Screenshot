using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createfilexml : MonoBehaviour {
    public NetworkSetting networkSetting = new NetworkSetting();
    public SaverSetting saverSetting = new SaverSetting();
    public ReaderSetting readerSetting = new ReaderSetting();
    // Use this for initialization
    void Start () {
        networkSetting.IP = "127.0.0.1";
        networkSetting.Port = 7777;
        networkSetting.IsSendDirect = false;
        networkSetting.MaxConnects = 10;
        networkSetting.Save("NetworkSetting.xml");

        saverSetting.Path = "";
        saverSetting.ResetValue();
        saverSetting.Filename = "Screenshot";
        saverSetting.Save("SaverSetting.xml");

        readerSetting.Top = 0;
        readerSetting.Left = 0;
        readerSetting.Width = 300;
        readerSetting.Height = 300;
        readerSetting.Borderless = false;
        readerSetting.Save("ReaderSetting.xml");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
