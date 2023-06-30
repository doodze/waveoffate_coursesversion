using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _player;

    [SerializeField] private float _zOffset;
    [SerializeField] private float _smoothTime = 0.25f;

    private Vector3 _velocity = Vector3.zero;    

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position,
            new Vector3(_player.position.x, transform.position.y, _player.position.z + _zOffset), ref _velocity, _smoothTime);
    }

    public void SetPlayerPosition(Transform player)
    {
        _player = player;
    }
}
