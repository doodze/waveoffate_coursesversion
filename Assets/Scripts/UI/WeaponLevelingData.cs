public struct WeaponLevelingData
{
    public readonly WeaponType weaponType;
    public readonly int weaponLevel;

    public WeaponLevelingData(WeaponType weaponType, int weaponLevel)
    {
        this.weaponType = weaponType;
        this.weaponLevel = weaponLevel;
    }
}
