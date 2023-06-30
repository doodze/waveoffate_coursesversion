using System;
using UnityEngine;
using UnityEngine.UI;

public enum LevelType
{
    None,
    Slug,
    RockLegion
}
public class SelectedLevelItem : MonoBehaviour
{
    [SerializeField] private LevelType _levelType;
    [SerializeField] private Button _button;

    public event Action<LevelType> LevelSelected;

    private void Awake()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        LevelSelected?.Invoke(_levelType);
    }
}
