using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;

public class Chaos : RespawningPowerUp
{

    [PunRPC]
    protected override void OnBulletCollision(int viewID)
    {
        var bullet = PhotonView.Find(viewID).GetComponent<Projectile>();

        bullet.Owner.FireMode = FireMode.Chaos;
    }

    [PunRPC]
    protected override void OnPlayerCollision(int viewID)
    {
        var player = PhotonView.Find(viewID).GetComponent<Player>();

        player.FireMode = FireMode.RearShot;
    }
}
