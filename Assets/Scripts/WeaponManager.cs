using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Types")]
    [SerializeField] private Weapon _singleShot;
    [SerializeField] private Weapon _twinShot;
    [SerializeField] private Weapon _burstShot;
    [SerializeField] private Weapon _rearShot;
    [SerializeField] private Weapon _chaos;
    
    private Weapon _current;
    private FireMode _fireMode;


    public Weapon Current
    {
        get => _current;
        set
        {
            _current.gameObject.SetActive(false);
            value.gameObject.SetActive(true);
            _current = value;
        }
    }

    public FireMode FireMode
    {
        get => _fireMode;
        set
        {
            Current = ToWeapon(value);
            _fireMode = value;
        }
    }

    private Weapon ToWeapon(FireMode fireMode) =>
        fireMode switch
        {
            FireMode.SingleShot => _singleShot,
            FireMode.TwinShot => _twinShot,
            FireMode.BurstShot => _burstShot,
            FireMode.RearShot => _rearShot,
            FireMode.Chaos => _chaos,
            _ => throw new ArgumentOutOfRangeException(nameof(fireMode), fireMode, null)
        };

    private void Awake()
    {
        _current = _singleShot;
    }
}
