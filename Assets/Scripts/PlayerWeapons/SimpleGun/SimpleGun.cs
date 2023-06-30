using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SimpleGun : Weapon
{
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletStartPosition;    
    [SerializeField] private float _damage;
    [SerializeField] private float _levelUpDamage = 10f;
    [SerializeField] private float _shootDelayValue;
    [SerializeField] private float _levelUpShootDelay = 0.1f;

    private ObjectPool<Bullet> _bulletsPool;
    private float _shootDelay;


    private void Awake()
    {
        IsWeaponActive = true;
        _bulletsPool = new ObjectPool<Bullet>(CreateBullet, actionOnGet: OnGetBullet, actionOnRelease: OnReleaseBullet);        
    }

    private void Update()
    {
        _bulletStartPosition.eulerAngles = Vector3.zero;
        _bulletStartPosition.RotateAround(transform.position, Vector3.up, 250f * Time.deltaTime);
        
    }

    public override WeaponType WeaponType => WeaponType.SimpleGun;    

    public override void LevelUp()
    {
        AudioManager.Instance.Play("SimpleGunAwake");
        _damage += _levelUpDamage;
        _shootDelay -= _levelUpShootDelay;
        CurrentLevel++;        
    }

    public override void Shoot(Transform enemy)
    {
        if (enemy == null)
            return;

        if (Time.realtimeSinceStartup - _shootDelay > _shootDelayValue)
        {
            _shootDelay = Time.realtimeSinceStartup;           
            Bullet newBullet = _bulletsPool.Get();
            newBullet.Damage = _damage;
            newBullet.BulletHits += HandleBulletHit;
            newBullet.transform.LookAt(enemy);
            //GetComponent<AudioSource>().pitch = Random.Range(0.9f, 1.1f);
            AudioManager.Instance.Play("SimpleGun");
        }
    }

    private Bullet CreateBullet()
    {
        return Instantiate(_bulletPrefab, _bulletStartPosition.position, _bulletStartPosition.rotation);
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.transform.position = _bulletStartPosition.position;
        bullet.transform.rotation = _bulletStartPosition.rotation;
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void HandleBulletHit(Bullet bullet)
    {
        bullet.BulletHits -= HandleBulletHit;
        _bulletsPool.Release(bullet);
    }  
}
