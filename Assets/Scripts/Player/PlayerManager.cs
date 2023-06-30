using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private const string PLAYER_ID = "Player";

    private Dictionary<string, PlayerBehaviour> _loadedBehaviourCache;
    private Dictionary<string, PlayerConfig> _loadedConfigChache;

    private PlayerBehaviour _playerBehaviour;
    private PlayerConfig _playerConfig;
    private Transform _playerPosition;

    public event Action<Transform> PlayerPositionChanged;
    public event Action PlayerKilled;

    private void Awake()
    {
        _loadedBehaviourCache = new Dictionary<string, PlayerBehaviour>();
        _loadedConfigChache = new Dictionary<string, PlayerConfig>();
        PreloadResources();
    }

    private void Update()
    {
        Transform position = _playerBehaviour.transform;

        if (_playerPosition != position)
        {
            _playerPosition = position;
            PlayerPositionChanged?.Invoke(_playerPosition);
        }
    }

    public void SpawnPlayer()
    {
        PlayerBehaviour playerBehaviourTemplate = _loadedBehaviourCache[PLAYER_ID];
        PlayerConfig playerConfigTemplate = _loadedConfigChache[PLAYER_ID];

        _playerBehaviour = Instantiate(playerBehaviourTemplate);       

        var playerModel = new PlayerModel();

        playerModel.Health = playerConfigTemplate.PlayerModel.Health;

        _playerBehaviour.Initialize(playerModel);
        _playerBehaviour.PlayerHealthChanged += OnPlayerHealthChanged;
    }

    private void PreloadResources()
    {
        _playerBehaviour = Resources.Load<PlayerBehaviour>($"PlayerPrefab/{PLAYER_ID}");
        _playerConfig = Resources.Load<PlayerConfig>($"PlayerModel/{PLAYER_ID}");
        _loadedBehaviourCache.Add(PLAYER_ID, _playerBehaviour);
        _loadedConfigChache.Add(PLAYER_ID, _playerConfig);
    }

    private void OnPlayerHealthChanged(float health)
    {
        if (health <= 0)
        {
            PlayerKilled?.Invoke();
        }
    }

    public void SetClosestEnemy(Transform enemy)
    {
        _playerBehaviour.SetClosestEnemy(enemy);
    }

    public List<WeaponLevelingData> GetNotLeveledWeapons(int count)
    {
        return _playerBehaviour.GetNotLeveledWeapons(count);
    }

    public void LevelUpWeapon(WeaponType weapon)
    {
        _playerBehaviour.LevelUpWeapon(weapon);
    }

    public void StartAttacking()
    {
        _playerBehaviour.StartAttacking();
    }

    public void StopAttacking()
    {
        _playerBehaviour.StopAttacking();
    } 
}
