using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeItem : MonoBehaviour
{    
    [SerializeField] Button _button;
    [SerializeField] private Image _sprite;
    [SerializeField] private TextMeshProUGUI _type;      
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private List<GameObject> _upgradeStars;

    public event Action<WeaponType> WeaponUpgraded;

    private WeaponType _weaponType;    

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClicked);          
    }

    public void ShowUpgrade(WeaponUIContent content, WeaponLevelingData data)
    {      
        _sprite.sprite = content.sprite;
        _type.text = content.weaponType.ToString();                
        _weaponType = content.weaponType;
        _upgradeStars[data.weaponLevel].SetActive(true);
    }

    private void OnButtonClicked()
    {
        WeaponUpgraded?.Invoke(_weaponType);        
    }
}
