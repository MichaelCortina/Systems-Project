using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField _joinInput;
    [SerializeField] private TMP_InputField _createInput;
    [SerializeField] private TMP_Dropdown _sceneSelection;
    
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(_createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_joinInput.text);
    }
    
    public override void OnJoinedRoom()
    {
        var sceneName = _sceneSelection.captionText.text;
        PhotonNetwork.LoadLevel(sceneName);
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
}
