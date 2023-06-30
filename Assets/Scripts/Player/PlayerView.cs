using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private ParticleSystem _hitEffect;    

    public void PlayOnHitEffect()
    {
        _hitEffect.Play();
    }

    public void SetMaxHealth(float value)
    {
        _healthBar.SetMaxHealth = value;
        _healthBar.SetHealth = _healthBar.SetMaxHealth;
    }

    public void UpdateHealth(float value)
    {        
        _healthBar.SetHealth = value;
    }
}
