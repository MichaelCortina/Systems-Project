using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private List<RespawningPowerUp> _powerUps;
    private List<RespawningPowerUp> _disabled;
    private List<float> _respawnTimes;
        
    private void OnPowerUpDestroyed(RespawningPowerUp powerUp)
    {
        _disabled.Add(powerUp);
        _respawnTimes.Add(powerUp.RespawnTime);
    }
    
    private void Update()
    {
        for (var i = 0; i < _disabled.Count; i++)
        {
            _respawnTimes[i] -= Time.deltaTime;

            if (_respawnTimes[i] > 0) continue;

            _disabled[i].Enable();
            _disabled.RemoveAt(i);
            _respawnTimes.RemoveAt(i);
            i--;
        }
    }

    private void OnDisable()
    {
        foreach (var powerUp in _powerUps)
        {
            powerUp.OnPowerUpDisabled += OnPowerUpDestroyed;
        }
    }
    
    private void OnEnable()
    {
        foreach (var powerUp in _powerUps)
        {
            powerUp.OnPowerUpDisabled += OnPowerUpDestroyed;
        }
    }

    private void Awake()
    {
        _powerUps = new List<RespawningPowerUp>(GetComponentsInChildren<RespawningPowerUp>(true));
        _disabled = new List<RespawningPowerUp>();
        _respawnTimes = new List<float>();
    }
}