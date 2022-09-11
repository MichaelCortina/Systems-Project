using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using PhotonPlayer = Photon.Realtime.Player;
using UnityEngine;

public class GameMaster : MonoBehaviourPunCallbacks
{    
    public static int Score { get; set; }

    public override void OnPlayerLeftRoom(PhotonPlayer otherPlayer)
    {
        PhotonNetwork.DestroyPlayerObjects(otherPlayer);
    }
}
