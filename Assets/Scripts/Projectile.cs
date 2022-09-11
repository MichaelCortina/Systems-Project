using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : Destructible
{
    [SerializeField] private float _force;
    [SerializeField] private bool _friendlyFire;
    [SerializeField] private int _maxRicochets;
    [SerializeField, Range(0, 1f)] private float _ricochetBufferTime;

    private PhotonView _view;
    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private Collider2D _collider;
    
    private int _currRicochets;
    private float _currRicochetBufferTime;

    private Player _backingOwner;
    
    public Player Owner
    {
        get => _backingOwner;
        set => _view.RPC(nameof(SetOwner), RpcTarget.AllBuffered, value.photonView.ViewID);
    }

    [PunRPC]
    private void SetOwner(int viewId)
    {
        _backingOwner = PhotonView.Find(viewId)?.GetComponent<Player>();
        _renderer.color = Owner.Color;
    }

    protected void Update()
    {
        _currRicochetBufferTime -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.CompareTag("Player"))
        {
            var player = col.gameObject.GetComponent<Player>();

            if (_friendlyFire || Owner != player)
            {
                OnPlayerCollision(player);
                _view.RPC(nameof(DestroyThis), RpcTarget.AllBuffered, null);
            }
        }
        
        if (_currRicochetBufferTime <= 0)
            _currRicochets++;
        _currRicochetBufferTime = _ricochetBufferTime;

        if (_currRicochets == _maxRicochets)
            _view.RPC(nameof(DestroyThis),RpcTarget.AllBuffered, null);
    }

    private void OnPlayerCollision(Player player)
    {
        player.HP--;
        if (Owner == null) return;
        if (player.Equals(Owner)) //subtract score if you hit yourself
            Owner.Score--;
        else                      //add if you hit an opponent
            Owner.Score++;
    }
    
    private void Start()
    {        
        _rb.AddForce(transform.up * _force);
    }

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }
}