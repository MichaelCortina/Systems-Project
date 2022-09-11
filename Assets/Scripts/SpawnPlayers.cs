using System;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform[] _spawnLocations;

    public static Action OnPlayerRespawn;

    private int _playerCount;
    private int _myPlayerID;
    
    private GameObject SpawnPlayer()
    {
        return PhotonNetwork.Instantiate(_playerPrefab.name, _spawnLocations[_playerCount].position, Quaternion.identity);
    }
    
    private void Update()
    {
        //respawn
        if (Input.GetKeyDown(KeyCode.R) && Player.IsMineDead)
        {
            //photonView.RPC(nameof(RespawnPlayer), RpcTarget.AllBuffered, _myPlayerID);
            SpawnPlayer();
            OnPlayerRespawn?.Invoke();
            //code to deactivate UI with death screen
        }
    }

    [PunRPC]
    private void RespawnPlayer(int viewID)
    {
        var player = PhotonView.Find(viewID);
        player.gameObject.SetActive(true);
        player.transform.position = _spawnLocations[_playerCount].position;
    }

    private void Start()
    {
        _playerCount = PhotonNetwork.CountOfPlayersInRooms;
        _myPlayerID = SpawnPlayer().GetComponent<PhotonView>().ViewID;
    }
}
