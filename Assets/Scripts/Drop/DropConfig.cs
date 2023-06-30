using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "WaveOfFate/DropConfig")]
public class DropConfig : ScriptableObject
{
    [field: SerializeField]
    public DropModel DropModel { get; private set; }
}
