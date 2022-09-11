using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _sliderText;
    [SerializeField] private GameObject deathScreen;
    private string _baseScoreText;
    private int _maxHealth;
    
    private static UI _backingInstance;
    public static UI Instance
    {
        get => _backingInstance;
        set
        {
            if (Instance != null)
            {
                throw new Exception("Cannot instantiate more than one instance of Singleton UI");
            }
            _backingInstance = value;
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = $"{_baseScoreText}{score}";
    }

    public void UpdateHealth(int health)
    {
        _sliderText.text = $"{health}/{_maxHealth}";
        _slider.value = health / (float) _maxHealth;
    }

    public void Initialize(int maxHealth)
    {
        _maxHealth = maxHealth;
        UpdateHealth(maxHealth);
    }
    
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == Player.OnPlayerCreatedEventCode)
        {
            var view = PhotonView.Find((int) photonEvent.CustomData);

            if (!view.IsMine) return;

            var player = view.GetComponent<Player>();
            Initialize(player.HP);
        }
    }
    private void ActivateDeathScreen()
    {
        deathScreen.SetActive(true);
    }
    private void DeactivateDeathScreen()
    {
        deathScreen.SetActive(false);
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        Player.OnPlayerKilled += ActivateDeathScreen;
        SpawnPlayers.OnPlayerRespawn += DeactivateDeathScreen;
    }
    
    private void OnDisable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        Player.OnPlayerKilled -= ActivateDeathScreen;
        SpawnPlayers.OnPlayerRespawn -= DeactivateDeathScreen;
    }

    private void Awake()
    {
        Instance = this;
        _baseScoreText = _scoreText.text;
    }

}
