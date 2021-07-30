using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] private Transform _aim;
    private PlayerMovement _playerMovement;
    private PlayerInputController _playerInputController;
    private Camera _camera;
    private BaseWeaponController[] _weapons;
    private PlayerAudioController _audioController;

    public PlayerMovement PlayerMovement { get => _playerMovement; }
    public PlayerInputController PlayerInputController { get => _playerInputController; }
    public PlayerAudioController PlayerAudioController { get => _audioController; }
    public Transform CameraTransform { get => _camera.transform; }
    public Transform AimTransform { get => _aim.transform; }

    public void Enable(bool enable)
    {
        _playerMovement.enabled = enable;
        _playerInputController.enabled = enable;
        _audioController.enabled = enable;        
    }

    public void TakeDamage()
    {
        _audioController.Hurt();
    }

    private void Awake()
    {
        _audioController = GetComponent<PlayerAudioController>();
        _weapons = GetComponentsInChildren<BaseWeaponController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInputController = GetComponent<PlayerInputController>();
        _camera = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        _weapons[0].Initialize(this);
    }
}
