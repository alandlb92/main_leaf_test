using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] private int _maxLife = 10;
    [SerializeField] private float _jumpForce = 100;
    [SerializeField] private Transform _aim;
    private PlayerMovement _playerMovement;
    private PlayerInputController _playerInputController;
    private Camera _camera;
    private BowController _weapon;
    private PlayerAudioController _audioController;
    private GameManager _gameManager;
    private HudUI _hudUI;
    private int _currentLife;
    private Rigidbody _rigidbody;
    private bool _isDied;
    private bool _inGround;

    public bool IsDead { get => _isDied; }
    public PlayerMovement PlayerMovement { get => _playerMovement; }
    public PlayerInputController PlayerInputController { get => _playerInputController; }
    public PlayerAudioController PlayerAudioController { get => _audioController; }
    public Transform CameraTransform { get => _camera.transform; }
    public Transform AimTransform { get => _aim.transform; }
    public HudUI HudUI { get => _hudUI; }

    public void RecoverLife()
    {
        _currentLife = _maxLife;
        _hudUI.SetHeart(_maxLife, _currentLife);
        _isDied = false;
    }
    public void RecoverArrows()
    {
        _weapon.RecoverArrows();
    }

    public void Enable(bool enable)
    {
        _playerMovement.enabled = enable;
        _playerInputController.enabled = enable;
        _audioController.enabled = enable;
        _rigidbody.freezeRotation = true;
    }

    public void TakeDamage(int howMuch)
    {
        _audioController.Hurt();
        _currentLife -= howMuch;
        _hudUI.SetHeart(_maxLife, _currentLife);
        if (_currentLife <= 0)
            Die();
    }

    private void Die()
    {
        _isDied = true;
        _gameManager.PlayerDie();
        _audioController.Die();
        Enable(false);
        _rigidbody.freezeRotation = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationY;
    }

    public void Reset(Transform playerStartPoint)
    {
        _weapon.ResetBow();
        _playerInputController.ResetInput();
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.freezeRotation = true;
        transform.position = playerStartPoint.position;
        transform.rotation = playerStartPoint.rotation;
    }

    private void Awake()
    {
        _audioController = GetComponent<PlayerAudioController>();
        _weapon = GetComponentInChildren<BowController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInputController = GetComponent<PlayerInputController>();
        _camera = GetComponentInChildren<Camera>();
        _hudUI = FindObjectOfType<HudUI>();
        _gameManager = FindObjectOfType<GameManager>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        _playerInputController.OnPressedSpace = Jump;
    }

    private void Start()
    {
        _weapon.Initialize(this);
        _currentLife = _maxLife;
        _hudUI.SetHeart(_maxLife, _currentLife);
    }

    private void Jump()
    {
        if (_inGround && PlayerMovement.enabled)
        {
            Debug.Log("Jump");
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _inGround = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Terrain")
            _inGround = true;
    }
}
