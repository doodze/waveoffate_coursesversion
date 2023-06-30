using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private List<Weapon> _weapons;

    private PlayerModel _playerModel;
    private Transform _enemy;

    private bool _shouldAttack = true;

    public event Action<float> PlayerHealthChanged;    

    private void Update()
    {
        foreach (var weapon in _weapons)
        {
            if (weapon.IsWeaponActive && _shouldAttack)
            {
                weapon.Shoot(_enemy);
            }
        }
    }

    public void Initialize(PlayerModel model)
    {
        _playerModel = model;
        _playerView.SetMaxHealth(_playerModel.Health);
    }

    public void StartAttacking()
    {
        _shouldAttack = true;
    }

    public void StopAttacking()
    {
        _shouldAttack = false;
    }

    public void TakeDamage(float damage)
    {
        _playerModel.Health -= damage;
        _playerView.UpdateHealth(_playerModel.Health);
        _playerView.PlayOnHitEffect();
        PlayerHealthChanged?.Invoke(_playerModel.Health);
        AudioManager.Instance.Play("PlayerDamaged");
    }

    public void LevelUpWeapon(WeaponType weaponType)
    {
        var weapon = _weapons.Find(w => w.WeaponType == weaponType);
        weapon.SetWeaponActive();
        weapon.LevelUp();
    }

    public List<WeaponLevelingData> GetNotLeveledWeapons(int count)
    {
        List<WeaponLevelingData> selectedWeapons = new List<WeaponLevelingData>();

        var notLeveledWeapons = _weapons.Where(w => w.CurrentLevel != w.MaxLevel).ToList();

        if (notLeveledWeapons.Count >= count)
        {
            for (int i = 0; i < count; i++)
            {
                var randomWeapon = notLeveledWeapons[Random.Range(0, notLeveledWeapons.Count)];
                var weaponLevelingData = new WeaponLevelingData(randomWeapon.WeaponType, randomWeapon.CurrentLevel);
                selectedWeapons.Add(weaponLevelingData);
                notLeveledWeapons.Remove(randomWeapon);
            }
        }
        else
        {
            foreach (var weapon in notLeveledWeapons)
            {
                var weaponLevelingData = new WeaponLevelingData(weapon.WeaponType, weapon.CurrentLevel);
                selectedWeapons.Add(weaponLevelingData);
            }
        }

        return selectedWeapons;                
    }
    public void SetClosestEnemy(Transform enemy)
    {
        _enemy = enemy;
    }
}
