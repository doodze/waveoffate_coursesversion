using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "WaveOfFate/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [field: SerializeField]
    public EnemyModel EnemyModel { get; private set; }
}