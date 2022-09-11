using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Weapon : MonoBehaviourPun
{
    [SerializeField, Range(0f, 2f)] private float _reloadTime;
    [SerializeField] private BulletSpawn[] _spawns;
    [SerializeField] private Projectile _bullet;
    [SerializeField] private Color[] _playerTurretColors;

    private Player _owner;
    private PhotonView _view;
    private Camera _mainCamera;
    private SpriteRenderer _renderer;
    private float _currReloadTime = 0;

    private int _playerNumber;
    private bool IsReloading => _currReloadTime > 0;

    private void RotateToCursor()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
    }
    private void FireWeapon()
    {
        //compensate for fucked bullet rotation
        foreach (var spawn in _spawns)
        {
            var rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z - spawn.Rotation);
            var projectile = PhotonNetwork.Instantiate(_bullet.name, spawn.Location.position, rotation).GetComponent<Projectile>();
            projectile.Owner = _owner;
        }
    }

    [PunRPC]
    private void UpdateWeaponColor(float r, float g, float b)
    {
        _renderer.color = new Color(r, g, b);
    }

    private void UpdateColorRemote()
    {
        var (r, g, b) = (_playerTurretColors[_playerNumber].r, _playerTurretColors[_playerNumber].g, _playerTurretColors[_playerNumber].b);
        _view.RPC(nameof(UpdateWeaponColor), RpcTarget.AllBuffered, r, g, b);
    }
    private void Update()
    {
        if (!_view.IsMine) return;
        
        _currReloadTime -= Time.deltaTime;
        RotateToCursor();
        
        if (!Input.GetMouseButtonDown(0) || IsReloading) return;
        FireWeapon();
        _currReloadTime = _reloadTime;
    }

    private void Start()
    {
        if (!_view.IsMine) return;
        
        var (r, g, b) = ( _playerTurretColors[_playerNumber].r, _playerTurretColors[_playerNumber].g, _playerTurretColors[_playerNumber].b );
        photonView.RPC(nameof(UpdateWeaponColor), RpcTarget.AllBuffered, r, g, b);
    }
    private void Awake()
    {
        _owner = GetComponentInParent<Player>();
        _view = GetComponent<PhotonView>();
        _renderer = GetComponent<SpriteRenderer>();
        _mainCamera = Camera.main;
        _playerNumber = PhotonNetwork.CountOfPlayersInRooms;
    }
}

[Serializable]
internal struct BulletSpawn
{
    [Range(0, 360)] public int Rotation;
    public Transform Location;
}