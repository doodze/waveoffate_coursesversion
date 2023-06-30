using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class DropManager : MonoBehaviour
{
    [SerializeField] private List<string> _dropIds;

    private List<DropBehaviour> _drop;
    private Dictionary<string, DropBehaviour> _loadedBehavioursCache;
    private Dictionary<string, DropConfig> _loadedConfigsCache;
    private ObjectPool<DropBehaviour> _dropPool;
    private string _nextDropId;
    private Transform _killedEnemyPosition;

    public event Action<float> PickedDropValue;

    private void Awake()
    {
        _loadedBehavioursCache = new Dictionary<string, DropBehaviour>();
        _loadedConfigsCache = new Dictionary<string, DropConfig>();
        _drop = new List<DropBehaviour>();
        _dropPool = new ObjectPool<DropBehaviour>(CreateDrop, actionOnGet: OnGetDrop, actionOnRelease: OnRealeseDrop);
        PreloadResources();
    }
    
    private void PreloadResources()
    {
        PreloadDrop(_dropIds);
    }

    private void PreloadDrop(List<string> ids)
    {
        DropBehaviour dropBehaviourTemplate;
        DropConfig dropConfigTemplate;

        foreach (var id in ids)
        {
            dropBehaviourTemplate = Resources.Load<DropBehaviour>($"DropPrefabs/{id}");
            dropConfigTemplate = Resources.Load<DropConfig>($"DropModels/{id}");
            _loadedBehavioursCache.Add(id, dropBehaviourTemplate);
            _loadedConfigsCache.Add(id, dropConfigTemplate);
        }
    }

    public void SpawnDrop()
    {
        SetDropIds(_dropIds);
    }

    private void SetDropIds(List<string> dropIds)
    {
        _nextDropId = dropIds[Random.Range(0, dropIds.Count)];
        _dropPool.Get();
    }

    private DropBehaviour CreateDrop()
    {
        DropBehaviour dropBehaviourTemplate = _loadedBehavioursCache[_nextDropId];

        var dropBehaviour = Instantiate(dropBehaviourTemplate);

        return dropBehaviour;
    }

    private void OnGetDrop(DropBehaviour drop)
    {
        DropConfig dropConfigTemplate = _loadedConfigsCache[_nextDropId];

        var dropModel = GetDropModel(dropConfigTemplate);

        drop.Initialize(dropModel);
        drop.DropPicked += OnDropPicked;

        Vector3 dropPosition = _killedEnemyPosition.position;

        drop.transform.position = new Vector3(dropPosition.x, drop.transform.position.y, dropPosition.z);
        drop.transform.rotation = Quaternion.identity;
        drop.gameObject.SetActive(true);
        _drop.Add(drop);
    }

    private void OnRealeseDrop(DropBehaviour drop)
    {
        drop.DropPicked -= OnDropPicked;
        drop.gameObject.SetActive(false);
        _drop.Remove(drop);
    }

    private void OnDropPicked(DropBehaviour drop)
    {
        PickedDropValue?.Invoke(drop.DropValue);
        _dropPool.Release(drop);
    }

    private DropModel GetDropModel(DropConfig config)
    {
        var dropModel = new DropModel();
        dropModel.DropValue = config.DropModel.DropValue;       

        return dropModel;
    }

    public void SetKilledEnemyPosition(Transform enemy)
    {
        _killedEnemyPosition = enemy;
    }
}
