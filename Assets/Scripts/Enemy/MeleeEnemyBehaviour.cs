using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyBehaviour : EnemyBehaviour
{
    [SerializeField] private float _damageDelayValue;

    private float _damageDelay;
    
    protected override void OnTriggerStay(Collider other)
    {
        if (Time.realtimeSinceStartup - _damageDelay > _damageDelayValue)
        {
            _damageDelay = Time.realtimeSinceStartup;
            base.OnTriggerStay(other);
        }
       
    }
    protected override void OnPlayerTriggered(PlayerBehaviour playerBehaviour)
    {
        playerBehaviour.TakeDamage(enemyModel.AttackDamage);
    }

    private void OnDisable()
    {
        AudioManager.Instance.Play("EnemyDamaged");
    }
}
