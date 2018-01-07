using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class netClient : NetworkDiscovery {
    [SerializeField]
    RawImage image;
    // Use this for initialization
    void Start () {
        Initialize();
        StartAsClient();
	}
	
	public override void OnReceivedBroadcast(string fromAddress,string data)
    {
        Debug.Log(data);
    }
}

