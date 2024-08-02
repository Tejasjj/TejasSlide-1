using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Experimental.Rendering;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings(); // Connect to Photon
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Photon Master Server");
        PhotonNetwork.JoinLobby(); // Join the default lobby to recieve room list updates
    }
    //public override void OnJoinedLobby()
    //{
    //    base.OnJoinedLobby();
    //    Debug.Log("Successfully joined the lobby.");
    //    //PhotonNetwork.JoinOrCreateRoom("test", null, null);
    //}

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        Debug.Log("Disconnected from photon: " + cause);
    }
}
