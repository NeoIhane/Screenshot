﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class startServer : MonoBehaviour {
    public NetworkManager networkManager;
    // Use this for initialization
    void Start () {
        networkManager.StartServer();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
