using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class RespawningPowerUp : PowerUp
{
    [SerializeField] private float _respawnTime;

    public Action<RespawningPowerUp> OnPowerUpDisabled;

    public bool IsRespawning { get; private set; }
    public float RespawnTime => _respawnTime;
    
    public virtual void Enable()
    {
        IsRespawning = false;
        gameObject.SetActive(true);
    }

    [PunRPC]
    protected override void Disable()
    {
        OnPowerUpDisabled?.Invoke(this);
        IsRespawning = true;
        gameObject.SetActive(false);
    }
}