using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "WaveOfFate/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [field: SerializeField]
    public PlayerModel PlayerModel { get; private set; }
}
