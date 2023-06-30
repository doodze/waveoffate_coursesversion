using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string JOYSTICK_TAG = "Joystick";

    [SerializeField] private Rigidbody _playerRigidBody;
    [SerializeField] private Animator _animator;

    [SerializeField] private float _playerSpeed;

    private Joystick _joystick;
    private float _velocity = 0;
    private float _acceleration = 2.5f;

    public float PlayerSpeed => _playerSpeed;

    private void Awake()
    {
        _joystick = GameObject.FindGameObjectWithTag(JOYSTICK_TAG).GetComponent<Joystick>();
    }

    private void FixedUpdate()
    {
        float x = _joystick.Horizontal * _playerSpeed;
        float z = _joystick.Vertical * _playerSpeed;

        _playerRigidBody.velocity = new Vector3(x, _playerRigidBody.velocity.y, z);
        
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            transform.rotation = Quaternion.LookRotation(_playerRigidBody.velocity);

        if (_velocity < 1f)
            _velocity += Time.deltaTime * _acceleration;

        if (!_joystick.IsOnDrag)
            _velocity = 0;

        _animator.SetFloat("Blend", _velocity);
    }
}
