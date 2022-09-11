using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class PowerUp : MonoBehaviour
{
    protected PhotonView View;
    
    [PunRPC]
    protected abstract void OnBulletCollision(int viewID);
    
    [PunRPC]
    protected abstract void OnPlayerCollision(int viewID);

    [PunRPC]
    protected virtual void Disable()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var viewID = col.gameObject.GetComponent<PhotonView>().ViewID;
        
        if (col.gameObject.CompareTag("Player"))
            View.RPC(nameof(OnPlayerCollision), RpcTarget.AllBuffered, viewID);
        if (col.gameObject.CompareTag("Bullet"))
            View.RPC(nameof(OnBulletCollision), RpcTarget.AllBuffered, viewID);
        
        View.RPC(nameof(Disable), RpcTarget.AllBuffered, null);
    }
    
    protected virtual void Awake()
    {
        View = GetComponent<PhotonView>();
    }
}
