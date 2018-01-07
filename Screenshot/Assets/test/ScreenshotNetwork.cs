using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class ScreenshotNetwork : NetworkBehaviour
{
    NetworkClient client;
    public int connectedPlayers = 0;
    int port = 4444;
    string ip = "127.0.0.1";
    /*
    [Command]
    void CmdVideoTextureUpdate(byte[] data)
    {
        RpcUpdateVideoTexture(data);
    }

    [ClientRpc]
    void RpcUpdateVideoTexture(byte[] data)
    {
        if (isLocalPlayer)
            return;
        Texture2D picture_capture = new Texture2D(2048,2048);
        picture_capture.LoadImage(data);
        picture_capture.Apply();
        GetComponent<ScreenshotReader>().UpdateImage(picture_capture);
    }*/

   /* [Command]
    void CmdVideoTextureUpdate(Texture2D data)
    {
        RpcUpdateVideoTexture(data);
    }

    [ClientRpc]
    void RpcUpdateVideoTexture(Texture2D data)
    {
        if (isLocalPlayer)
            return;
        GetComponent<ScreenshotReader>().UpdateImage(data);
    }

    public void SetupServer()
    {
        NetworkServer.Listen(port);
        NetworkServer.RegisterHandler(MsgType.Connect, OnConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        NetworkServer.RegisterHandler(MsgType.Error, OnError);
    }
    void SetupClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnected);
        client.Connect(ip, port);
    }
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Client Connected");
        CmdVideoTextureUpdate(GetComponent<ScreenshotSaver>().picture_capture);
    }

    public void OnDisconnected(NetworkMessage netMsg)
    {
        Debug.Log("Disconnected");
    }

    public void OnError(NetworkMessage netMsg)
    {
        Debug.Log("Error while connecting");
    }


    // Use this for initialization
    void Start ()
    {
		if(isServer)
        {
            SetupServer();
        }
        else
        {
            SetupClient();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.B))
        {
           
        }
    }*/
}
