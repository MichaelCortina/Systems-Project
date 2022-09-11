using Photon.Pun;
using System;

public class RandomWeaponPowerUp : RespawningPowerUp
{
    private Random _rand;

    [PunRPC]
    protected override void OnBulletCollision(int viewID)
    {
        OnCollision(
            PhotonView
            .Find(viewID)
            .GetComponent<Projectile>()
            .Owner);
    }

    [PunRPC]
    protected override void OnPlayerCollision(int viewID)
    {
        OnCollision(
            PhotonView
            .Find(viewID)
            .GetComponent<Player>());
    }

    private void OnCollision(Player player)
    {
        player.FireMode = (FireMode)_rand.Next(5);
    }

    protected override void Awake()
    {
        base.Awake();
        _rand = new Random();
    }
}
