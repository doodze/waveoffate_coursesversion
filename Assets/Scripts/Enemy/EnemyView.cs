using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private ParticleSystem _dieEffect;

    public void PlayOnHitEffect()
    {
        _hitEffect.Play();
    }
    public void PlayOnDieEffect()
    {
        _dieEffect.Play();
    }
}
