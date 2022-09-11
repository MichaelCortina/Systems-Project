using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// <param name="Runs:"> after start method of newly instantiated player</param>
    /// <param name="Sends:"> int: ViewID of player </param>
    /// </summary>
    public const byte OnPlayerCreatedEventCode = 1;
    public static Action OnPlayerKilled;
    public Action OnWeaponChanged;

    [SerializeField] private int _HP = 1;
    [SerializeField] private int _score;
    [SerializeField] private Color[] _playerColors;

    private SpriteRenderer _renderer;
    private Collider2D _collider;
    private WeaponManager _weapons;
    
    private int _maxHP;
    private int _playerNumber;

    public static bool IsMineDead { get; private set; }
    public static Player MyPlayer { get; private set; }

    public int Score
    {
        get => _score;
        set => photonView.RPC(nameof(SetScore), RpcTarget.AllBuffered, value);

    }
    public int HP
    {
        get => _HP;
        set => photonView.RPC(nameof(SetHP), RpcTarget.AllBuffered, value);
    }

    public FireMode FireMode
    {
        get => _weapons.FireMode;
        set => _weapons.FireMode = value;
    }

    public Weapon Weapon
    {
        get => _weapons.Current;
        set 
        { 
            _weapons.Current = value;
            OnWeaponChanged?.Invoke();
        }
    }

    public Color Color => _renderer.color;

    [PunRPC]
    private void SetScore(int score)
    {
        _score = score;
        
        if (!photonView.IsMine) return;
        UI.Instance.UpdateScore(score);
        GameMaster.Score = score;
    }
    
    [PunRPC]
    private void SetHP(int hp)
    {
        _HP = hp > _maxHP ? _maxHP : hp;
        
        if (photonView.IsMine)
            UI.Instance.UpdateHealth(_HP);
        if (_HP <= 0)
            OnPlayerDeath();
    }

    [PunRPC]
    private void UpdatePlayerColor(float r, float g, float b)
    {
        _renderer.color = new Color(r, g, b);
    }

    private void UpdateColorRemote()
    {
        var (r, g, b) = ( _playerColors[_playerNumber].r, _playerColors[_playerNumber].g, _playerColors[_playerNumber].b );
        photonView.RPC(nameof(UpdatePlayerColor), RpcTarget.AllBuffered, r, g, b);
    }
    private void OnPlayerDeath()
    {
        if (photonView.IsMine)
        {
            IsMineDead = true;
            OnPlayerKilled?.Invoke();
        }
        gameObject.SetActive(false);
        
        //make DeathScreen visible
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            Physics2D.IgnoreCollision(col.collider, _collider);
    }


    public override void OnEnable()
    {
        base.OnEnable();
        HP = _maxHP;
    }

    private void Start()
    {
        if (!photonView.IsMine) return;

        IsMineDead = false;
        MyPlayer = this;
        _score = GameMaster.Score;
        
        UI.Instance.Initialize(_HP);

        (float r,float g, float b) = ( _playerColors[_playerNumber].r, _playerColors[_playerNumber].g, _playerColors[_playerNumber].b );
        photonView.RPC(nameof(UpdatePlayerColor), RpcTarget.AllBuffered, r, g, b);
        
        PhotonNetwork.RaiseEvent(OnPlayerCreatedEventCode, photonView.ViewID, RaiseEventOptions.Default, SendOptions.SendReliable);
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _weapons = GetComponent<WeaponManager>();
        _playerNumber = PhotonNetwork.CountOfPlayersInRooms;
        _maxHP = _HP;
    }
}