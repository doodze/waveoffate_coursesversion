using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioactiveAura : Weapon
{
    public override WeaponType WeaponType => WeaponType.RadioactiveAura;

    public override void LevelUp()
    {
        CurrentLevel++;
    }

    public override void Shoot(Transform enemy)
    {
        
    }
}
