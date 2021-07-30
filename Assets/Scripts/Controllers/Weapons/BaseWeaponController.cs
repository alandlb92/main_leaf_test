using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeaponController : MonoBehaviour
{
    protected PlayerController _playerController;
    protected Animator _animator;

    public virtual void Initialize(PlayerController playerController)
    {
        _playerController = playerController;
        Awake();
    }
    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (_playerController.PlayerMovement == null)
            return;

        _animator.SetFloat("Velocity", _playerController.PlayerMovement.Velocity);
    }
}
