using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingWeapon : Weapon
{
    [SerializeField] private Transform _player;    
    [SerializeField] private List<RotatingWeaponElement> _weaponElements;
    [Header("Weapon Stats")][Space]
    [SerializeField] private float _damage;
    [SerializeField] private float _damageLevelUp = 10f;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotationSpeedLevelUp = 100f;

    public override WeaponType WeaponType => WeaponType.RotatingGun;

    private void Awake()
    {        
        AudioManager.Instance.Play("RotatingGunAwake");
    }       

    private void Update()
    {     
        transform.eulerAngles = Vector3.zero;
        transform.position = new Vector3(_player.position.x, transform.position.y, _player.position.z);

        foreach (var element in _weaponElements)
        {
            element.Damage = _damage;
            element.transform.RotateAround(transform.position, Vector3.up, _rotationSpeed * Time.deltaTime);
        }
        Debug.Log($"Damage: {_damage} Shoot delay: {_rotationSpeed}");
    }

    public override void LevelUp()
    {
        AudioManager.Instance.Play("RotatingGunAwake");
        _rotationSpeed += _rotationSpeedLevelUp;
        _damage += _damageLevelUp;
        CurrentLevel++;

    }

    public override void Shoot(Transform enemy)
    {

    }
}
