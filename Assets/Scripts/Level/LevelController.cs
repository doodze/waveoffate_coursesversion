using Unity.VisualScripting;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GroundFollowPlayer _ground;
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private DropManager _dropManager;
    [SerializeField] private UIManager _uiManager;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        _playerManager.PlayerPositionChanged += OnPlayerPositionChanged;
        
        _enemyManager.EnemyKilledPosition += OnEnemyKilledPosition;
        _enemyManager.ClosestEnemyChanged += OnClosestEnemyChanged;        
        _dropManager.PickedDropValue += OnPickedDropValue;
        _uiManager.LevelSelected += OnGameLevelSelected;
        _uiManager.LevelStarted += OnLevelStarted;
        _uiManager.LevelIncreased += OnLevelIncreased;
        _uiManager.WeaponUpgraded += OnWeaponUpgraded;
        _uiManager.GamePaused += OnGamePaused;
        _uiManager.GameUnpaused += OnGameUnpaused;
        _playerManager.PlayerKilled += OnPlayerKilled;
    }

    private void OnPlayerPositionChanged(Transform player)
    {
        _camera.SetPlayerPosition(player);
        _ground.SetPlayerPosition(player);
        _enemyManager.SetPlayerPosition(player);
    }

    private void OnClosestEnemyChanged(Transform enemy)
    {
        _playerManager.SetClosestEnemy(enemy);
    }

    private void OnEnemyKilledPosition(Transform enemy)
    {
        _dropManager.SetKilledEnemyPosition(enemy);
        _dropManager.SpawnDrop();
        _uiManager.UpdateKilledEnemiesCounter();
    }

    private void OnPickedDropValue(float value)
    {
        _uiManager.UpdateBarValue(value);
    }

    private void OnLevelStarted()
    {
        _uiManager.ChangeToGameUI();
        _playerManager.SpawnPlayer();       
        _enemyManager.StartSpawning();
    }

    private void OnGameLevelSelected(LevelType levelType)
    {
        _enemyManager.SetLevelType(levelType);
        _ground.SetLevelType(levelType);
    }

    private void OnLevelIncreased()
    {
        _uiManager.SetNotLeveledWeapons(_playerManager.GetNotLeveledWeapons(_uiManager.WeaponsCount));
    }

    private void OnWeaponUpgraded(WeaponType weapon)
    {
        _playerManager.LevelUpWeapon(weapon);
    }

    private void OnPlayerKilled()
    {
        _uiManager.ShowScore();
    }

    private void OnGamePaused()
    {
        _enemyManager.StopSpawning();
        _playerManager.StopAttacking();
    }
    private void OnGameUnpaused()
    {
        _enemyManager.StartSpawning();
        _playerManager.StartAttacking();
    }   
}
