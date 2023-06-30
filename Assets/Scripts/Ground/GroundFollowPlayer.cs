using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFollowPlayer : MonoBehaviour
{
    [SerializeField] private Material _stoneMat;
    [SerializeField] private Material _lavaMat;

    private Transform _player;
    private LevelType _levelType;    

    private void Start()
    {
        GetComponent<Renderer>().material.mainTextureOffset = Vector2.zero;
    }

    private void Update()
    {
        switch (_levelType)
        {
            case LevelType.None:
                break;
            case LevelType.Slug:
                GetComponent<Renderer>().material = _stoneMat;                
                break;
            case LevelType.RockLegion:
                GetComponent<Renderer>().material = _lavaMat;                
                break;
        }
        
        transform.position = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
        GetComponent<Renderer>().material.mainTextureOffset = (new Vector2(_player.transform.position.x, _player.transform.position.z) * -1) / 10 / 2;
    }

    private void OnDisable()
    {
        GetComponent<Renderer>().material.mainTextureOffset = Vector2.zero;
    }

    public void SetPlayerPosition(Transform player)
    {
        _player = player;
    }

    public void SetLevelType(LevelType level)
    {
        _levelType = level;
    }
}
