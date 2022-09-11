using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;

public class BurstShot : PowerUp
{
    [SerializeField] private int _BulletNumberHorizontal = 1;

    [PunRPC]
    protected override void OnBulletCollision(int viewID)
    {
        var bullet = PhotonView.Find(viewID).GetComponent<Projectile>();

        bullet.Owner.FireMode = FireMode.BurstShot;
        //bullet.Owner.HP += _healAmount / 2;
    }

    [PunRPC]
    protected override void OnPlayerCollision(int viewID)
    {
        var player = PhotonView.Find(viewID).GetComponent<Player>();

        player.FireMode = FireMode.BurstShot;
        //player.HP += _healAmount;
    }
}
