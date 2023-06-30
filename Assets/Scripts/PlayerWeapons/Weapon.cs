using UnityEngine;

public enum WeaponType
{
    None,
    Melee,
    SimpleGun,
    RotatingGun,
    GaussGun,
    RadioactiveAura
}

public abstract class Weapon : MonoBehaviour
{    
    public abstract WeaponType WeaponType { get; } 

    public int CurrentLevel { get; protected set; }

    public int MaxLevel = 5;

    public abstract void LevelUp(); 

    public abstract void Shoot(Transform enemy);

    public virtual bool IsWeaponActive { get; protected set; }

    public virtual void SetWeaponActive()
    {
        IsWeaponActive = true;
        gameObject.SetActive(IsWeaponActive);
    }
}
