using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussGun : Weapon, IDamagedealer
{
    public override WeaponType WeaponType => WeaponType.GaussGun;
    
    public float Damage { get; set; }
   
    public override void LevelUp()
    {
        CurrentLevel++;
    }

    public override void Shoot(Transform enemy)
    {
        
    }
}
