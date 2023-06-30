using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Options")]
    [Space]
    [SerializeField] private float _firstSpawnDelay;
    [SerializeField] private float _enemiesSpawnDelay;    
    [SerializeField] private float _eliteEnemiesSpawnDelay;    
    [SerializeField] private float _spawnRadius;
    [SerializeField] private List<string> _firstLevelIds;
    [SerializeField] private List<string> _secondLevelIds;
    [SerializeField] private List<string> _eliteEnemyIds;    
    [Header("Player Link")][Space]
    private Transform _playerPosition;
    private List<EnemyBehaviour> _enemies;
    private Dictionary<string, EnemyBehaviour> _loadedBehavioursCache;
    private Dictionary<string, EnemyConfig> _loadedConfigsCache;
    private ObjectPool<EnemyBehaviour> _enemiesPool;
    private Transform _closestEnemy;
    private string _nextEnemyId;
    private bool _shouldSpawnEnemies = false;
    private float _enemySpawnTime;    
    private LevelType _levelType;
    private List<string> _enemyToSpawnIds;

    public event Action<Transform> EnemyKilledPosition;
    public event Action<Transform> ClosestEnemyChanged;

    private void Awake()
    {
        _enemyToSpawnIds = new List<string>();
        _loadedBehavioursCache = new Dictionary<string, EnemyBehaviour>();
        _loadedConfigsCache = new Dictionary<string, EnemyConfig>();       
        _enemies = new List<EnemyBehaviour>();
        _enemiesPool = new ObjectPool<EnemyBehaviour>(CreateEnemy, actionOnGet: OnGetEnemy, actionOnRelease: OnRealeseEnemy);
        PreloadResources();
    }

    private void Update()
    {       
        float time = MathF.Round(Time.timeSinceLevelLoad, 1);        

        if (_shouldSpawnEnemies)
        {   
            switch (_levelType)
            {
                case LevelType.None:
                    break;
                case LevelType.Slug:
                    _enemyToSpawnIds = _firstLevelIds;
                    break;
                case LevelType.RockLegion:
                    _enemyToSpawnIds = _secondLevelIds;
                    break;                
            }
            
            if (time - _enemySpawnTime > _enemiesSpawnDelay)
            {
                _enemySpawnTime = time;
                _enemiesSpawnDelay -= 1f * Time.deltaTime;

                if (_enemiesSpawnDelay <= 0.001)
                {
                    _enemiesSpawnDelay = 0;
                }
                
                SpawnEnemies(_enemyToSpawnIds);
            }   
        }

        if (_enemies == null)
            return;

        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i].gameObject.activeInHierarchy)
            {
                _enemies[i].MoveToPlayer(_playerPosition);
            }            
        }

        Transform closestEnemy = ClosestEnemy();

        if (_closestEnemy != closestEnemy)
        {
            _closestEnemy = closestEnemy;
            ClosestEnemyChanged?.Invoke(_closestEnemy);
        }
    }

    public void StartSpawning()
    {
        _shouldSpawnEnemies = true;
    }

    public void StopSpawning()
    {
        _shouldSpawnEnemies = false;
    }

    private void PreloadResources()
    {
        PreloadEnemies(_firstLevelIds);
        PreloadEnemies(_secondLevelIds);               
    }

    private void PreloadEnemies(List<string> ids)
    {
        EnemyBehaviour enemyBehaviourTemplate;
        EnemyConfig enemyConfigTemplate;

        foreach (var id in ids)
        {
            enemyBehaviourTemplate = Resources.Load<EnemyBehaviour>($"EnemyPrefabs/{id}");
            enemyConfigTemplate = Resources.Load<EnemyConfig>($"EnemyModels/{id}");
            _loadedBehavioursCache.Add(id, enemyBehaviourTemplate);
            _loadedConfigsCache.Add(id, enemyConfigTemplate);
        }
    }

    private void SpawnEnemies(List<string> enemyIds)
    {
        _nextEnemyId = enemyIds[Random.Range(0, enemyIds.Count)];

        EnemyConfig enemyConfigTemplate = _loadedConfigsCache[_nextEnemyId];

        var enemyModel = GetEnemyModel(enemyConfigTemplate);
        
        var enemy = _enemiesPool.Get();

        enemy.Initialize(enemyModel);
        enemy.EnemyKilled += OnEnemyKilled;
    }

    private EnemyBehaviour CreateEnemy()
    {
        EnemyBehaviour enemyBehaviourTemplate = _loadedBehavioursCache[_nextEnemyId];

        var enemyBehaviour = Instantiate(enemyBehaviourTemplate);
        
        return enemyBehaviour;
    }

    private void OnGetEnemy(EnemyBehaviour enemy)
    {   
        Vector3 randomPos = GetRandomEnemySpawnPosition();
        
        enemy.transform.position = new Vector3(randomPos.x, enemy.transform.position.y, randomPos.z);
        enemy.transform.rotation = Quaternion.identity;
        enemy.gameObject.SetActive(true);
        _enemies.Add(enemy);
    }

    private void OnRealeseEnemy(EnemyBehaviour enemy)
    {        
        enemy.EnemyKilled -= OnEnemyKilled;         
        enemy.gameObject.SetActive(false);
        _enemies.Remove(enemy);
    }

    private void OnEnemyKilled(EnemyBehaviour enemy)
    {        
        EnemyKilledPosition?.Invoke(enemy.transform);        
        _enemiesPool.Release(enemy);
    }    

    private Vector3 GetRandomEnemySpawnPosition()
    {
        Vector3 randomPos = Random.insideUnitSphere * _spawnRadius;
        randomPos += _playerPosition.position;

        Vector3 direction = randomPos - _playerPosition.position;
        direction.Normalize();

        float dotProduct = Vector3.Dot(_playerPosition.forward, direction);
        float dotProductAngle = Mathf.Acos(dotProduct / _playerPosition.forward.magnitude * direction.magnitude);

        randomPos.x = Mathf.Cos(dotProductAngle) * _spawnRadius + _playerPosition.position.x;
        randomPos.z = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * _spawnRadius + _playerPosition.position.z;

        return randomPos;
    }

    private EnemyModel GetEnemyModel(EnemyConfig config)
    {
        var enemyModel = new EnemyModel();
        enemyModel.Health = config.EnemyModel.Health;
        enemyModel.AttackDamage = config.EnemyModel.AttackDamage;
        enemyModel.AttackDelay = config.EnemyModel.AttackDelay;
        enemyModel.AttackRange = config.EnemyModel.AttackRange;
        enemyModel.MovementSpeed = config.EnemyModel.MovementSpeed;

        return enemyModel;
    }
    

    public Transform ClosestEnemy()
    {
        Transform closestEnemy = null;
        
        float minDistance = Mathf.Infinity;

        Vector3 currentPos = _playerPosition.position;

        for (int i = 0; i < _enemies.Count; i++)
        {
            float distance = Vector3.Distance(_enemies[i].transform.position, currentPos);

            if (distance < minDistance)
            {
                closestEnemy = _enemies[i].transform;
                minDistance = distance;
            }
        }        

        return closestEnemy;
    }

    public void SetPlayerPosition(Transform player)
    {
        _playerPosition = player;
    }

    public void SetLevelType(LevelType levelType)
    {
        _levelType = levelType;
    }
}
