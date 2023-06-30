using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehaviour : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";    

    [SerializeField] private EnemyView _enemyView;    

    protected EnemyModel enemyModel;

    public event Action<EnemyBehaviour> EnemyKilled;   

    public void Initialize(EnemyModel model)
    {
        enemyModel = model;
    }

    public virtual void MoveToPlayer(Transform player)
    { 
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        transform.position = Vector3.MoveTowards(transform.position, player.position, enemyModel.MovementSpeed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(PLAYER_TAG) && other.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour playerBehaviour))
        {
            OnPlayerTriggered(playerBehaviour);
        }

        if (other.gameObject.TryGetComponent<IDamagedealer>(out IDamagedealer damageDealer))
        {
            //Debug.Log($"Урон от триггера в размере: {damageDealer.Damage} от: {damageDealer}");
            TakeDamage(damageDealer.Damage);
            _enemyView.PlayOnHitEffect();
            AudioManager.Instance.Play("EnemyDamaged");
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG) && other.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour playerBehaviour))
        {
            OnPlayerTriggered(playerBehaviour);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {     
        if (collision.gameObject.TryGetComponent<IDamagedealer>(out IDamagedealer damageDealer))
        {            
            TakeDamage(damageDealer.Damage);
            _enemyView.PlayOnHitEffect();
            AudioManager.Instance.Play("EnemyDamaged");
            //Debug.Log($"Урон в размере: {damageDealer.Damage} от: {damageDealer}");
        }
    }

    private void TakeDamage(float damageValue)
    {        
        enemyModel.Health -= damageValue;
        //Debug.Log($"В меня попали. Осталось: {enemyModel.Health} ХП");
        if (enemyModel.Health <= 0)
        {
            //Debug.Log("Умер");            
            EnemyKilled?.Invoke(this);
        }
    }

    protected abstract void OnPlayerTriggered(PlayerBehaviour playerBehaviour);    
}
