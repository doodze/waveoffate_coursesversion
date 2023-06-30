using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBehaviour : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";

    private DropModel _dropModel;
    
    public event Action<DropBehaviour> DropPicked;

    public float DropValue => _dropModel.DropValue;

    public void Initialize(DropModel model)
    {
        _dropModel = model;
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PLAYER_TAG) && other.TryGetComponent<PlayerBehaviour>(out PlayerBehaviour playerBehaviour))
        {
            DropPicked?.Invoke(this);            
        }
    }

    private void OnDisable()
    {
        AudioManager.Instance.Play("CoinUp");
    }
}
