using UnityEngine;
using System;

[Serializable]
public class EnemyModel
{
    [field: SerializeField]
    public float Health { get; set; }
    [field: SerializeField]
    public float AttackDamage { get; set; }
    [field: SerializeField]
    public float AttackDelay { get; set; }
    [field: SerializeField]
    public float AttackRange { get; set; }
    [field: SerializeField]
    public float MovementSpeed { get; set; }
}
