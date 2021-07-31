using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class BaseEnemyController : MonoBehaviour, IDamage
{
    public bool IsDead => !_animator.enabled;

    [SerializeField] private int _startLife = 1;
    private int _currentLife;

    [SerializeField] protected float _distanceToApproachTheEnemy = 20;
    [SerializeField] private int _priority = 50;
    protected NavMeshAgent _navMeshAgent;
    protected Rigidbody[] _ragDollRigdybodys;
    protected Collider[] _ragDollColliders;
    protected Animator _animator;
    protected PlayerController _playerController;
    protected BaseEnemyAnimationEvents _animationEvents;
    protected BaseAudioController _audioController;
    protected bool _attackMode;
    protected bool _waitDistanceToApproachTheEnemy = false;

    private Action OnDie;

    public virtual void Initialize(Action OnDie, int count)
    {
        this.OnDie = OnDie;
        _animator.enabled = true;
        _currentLife = _startLife;
        _navMeshAgent.avoidancePriority = _priority + count;
        Awake();
    }

    public virtual void TakeDamage()
    {
        if (IsDead)
            return;

        _currentLife--;
        if (_currentLife <= 0)
            Die();
    }

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _playerController = FindObjectOfType<PlayerController>();
        _ragDollRigdybodys = GetComponentsInChildren<Rigidbody>();
        _ragDollColliders = GetComponentsInChildren<Collider>();
        _animationEvents = GetComponentInChildren<BaseEnemyAnimationEvents>();
        _audioController = GetComponent<BaseAudioController>();
        _animationEvents.OnFootL = OnFootL;
        _animationEvents.OnFootR = OnFootR;
        _animationEvents.OnFire = OnFire;
        EnableRagDoll(false);
    }

    protected virtual void Update()
    {
        if (IsDead)
            return;

        _animator.SetFloat("Velocity", _navMeshAgent.velocity.magnitude / _navMeshAgent.speed);
        _navMeshAgent.SetDestination(_playerController.transform.position);

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance || _waitDistanceToApproachTheEnemy)
        {
            _navMeshAgent.isStopped = true;
            _attackMode = true;
            _waitDistanceToApproachTheEnemy = _navMeshAgent.remainingDistance <= _distanceToApproachTheEnemy;
        }
        else
        {
            _navMeshAgent.isStopped = false;
            _attackMode = false;
        }
    }

    protected void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, .1f);
    }

    private void EnableRagDoll(bool enable)
    {
        _ragDollRigdybodys.ToList().ForEach(r => r.isKinematic = !enable);
    }

    protected virtual void Die()
    {
        _navMeshAgent.avoidancePriority = 0;
        _animator.enabled = false;
        EnableRagDoll(true);
        OnDie?.Invoke();
    }

    private void OnFootL()
    {
        _audioController.Step();
    }

    private void OnFootR()
    {
        _audioController.Step();
    }

    protected virtual void OnFire()
    {

    }
}
