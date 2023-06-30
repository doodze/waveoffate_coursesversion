using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour, IDamagedealer
{   
    public event Action<Bullet> BulletHits;

    private const float _bulletSpeed = 50f;

    public float Damage { get; set; }

    private void Update()
    {
        transform.position += transform.forward * _bulletSpeed * Time.deltaTime;
    }   

    private void OnTriggerEnter(Collider other)
    {        
        BulletHits?.Invoke(this);
    }
}
