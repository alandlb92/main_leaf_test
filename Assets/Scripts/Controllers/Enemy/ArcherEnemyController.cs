using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemyController : BaseEnemyController
{
    [SerializeField] private GameObject _spine1;
    [SerializeField] private Vector3 _rotationSpine1Offset;
    [SerializeField] private float _timeToBlendLayers = .5f;
    [SerializeField] private GameObject _arrowOrigin;
    [SerializeField] private ArrowController _arrowInstanceReference;

    [SerializeField] private Vector2 _fireShotDesitionRangeTime = new Vector2(0.2f, 1);
    [SerializeField] private float _aimDistanceAdjustmentAmount = 2.5f;

    private bool _needToReload;
    private Coroutine _fireRotine;
    private Coroutine _blendCoroutine;
    private Coroutine _verifyReloadCorotine;
    private Vector3 _arrowOriginStartLocalPosition;
    private List<ArrowController> _arrows = new List<ArrowController>();
    private int _currentArrowIndex = 0;

    private ArcherAudioController AudioController => (ArcherAudioController)_audioController;
    private const int MAX_ARROW_INSTANCES = 5;

    public override void HideBody()
    {
        base.HideBody();
        _arrows.ForEach(a =>
        {
            a.transform.parent = null;
            a.transform.position = Vector3.zero;
        });
    }


    public override void Initialize(Action<BaseEnemyController.Type> OnDie, int count)
    {
        base.Initialize(OnDie, count);
        OnReload();
        AudioController.Spawn();

    }

    protected override void Awake()
    {
        base.Awake();
        _type = Type.ARCHER;
        _arrowOriginStartLocalPosition = _arrowOrigin.transform.localPosition;
        _animationEvents.OnEndFire = OnEndFire;
        _animationEvents.OnReload = OnReload;
    }

    protected override void Die()
    {
        base.Die();
        AudioController.Die();
    }

    protected override void Update()
    {
        if (IsDead)
            return;

        if (!_needToReload)
        {
            if (_verifyReloadCorotine != null)
            {
                StopCoroutine(_verifyReloadCorotine);
                _verifyReloadCorotine = null;
            }

            base.Update();
            if (_attackMode && !_needToReload)
            {
                FaceTarget(_playerController.transform.position);
                if (_blendCoroutine == null && _animator.GetLayerWeight(1) == 0)
                    StartBlend(false);
            }
            else if (_blendCoroutine == null && _animator.GetLayerWeight(1) == 1)
                StartBlend(true);
        }
        else
        {
            _animator.SetFloat("Velocity", _navMeshAgent.velocity.magnitude / _navMeshAgent.speed);
            VerifyReload();
        }

    }

    private void LateUpdate()
    {
        if (IsDead)
            return;

        if (_attackMode)
        {
            if (_blendCoroutine == null)
            {
                _spine1.transform.LookAt(_playerController.transform);
                _spine1.transform.rotation = Quaternion.Euler(GetCalculatedAdjustmentDistance(true) + _spine1.transform.rotation.eulerAngles + _rotationSpine1Offset);
                if (_fireRotine == null && !_needToReload)
                    _fireRotine = StartCoroutine(Fire());
            }
        }
    }

    private void VerifyReload()
    {
        if (_verifyReloadCorotine == null)
            _verifyReloadCorotine = StartCoroutine(RunVerifyReload());
    }

    private IEnumerator RunVerifyReload()
    {
        yield return new WaitForSeconds(2.5f);
        yield return SetAtackMode(true, true);
        _verifyReloadCorotine = null;
    }

    private Vector3 GetCalculatedAdjustmentDistance(bool isSpine)
    {
        float distance = Vector3.Distance(this.transform.position, _playerController.transform.position);
        if (distance > 5)
            if (isSpine)
                return new Vector3(0, 0, -(distance / _aimDistanceAdjustmentAmount));
            else
                return new Vector3(-(distance / _aimDistanceAdjustmentAmount), 0, 0);
        else
            return Vector3.zero;
    }

    private IEnumerator Fire()
    {
        float waitToAttack = UnityEngine.Random.Range(_fireShotDesitionRangeTime.x, _fireShotDesitionRangeTime.y);
        yield return new WaitForSeconds(waitToAttack);
        _needToReload = true;
        _animator.SetTrigger("Attack");
        _fireRotine = null;
    }

    private void StartBlend(bool backToNormal, bool reload = false)
    {
        _audioController.BowPrepare();
        if (_blendCoroutine != null)
            StopCoroutine(_blendCoroutine);

        _blendCoroutine = StartCoroutine(SetAtackMode(backToNormal, reload));
    }

    private IEnumerator SetAtackMode(bool backToNormal, bool reload = false)
    {
        if (backToNormal)
        {
            for (float t = (_timeToBlendLayers * _animator.GetLayerWeight(1)); t > 0; t -= Time.deltaTime)
            {
                _animator.SetLayerWeight(1, t / _timeToBlendLayers);
                yield return null;
            }
            _animator.SetLayerWeight(1, 0);
            _attackMode = false;
            if (reload)
            {
                _animator.SetTrigger("Reload");
            }

        }
        else
        {
            for (float t = 0; t < _timeToBlendLayers; t += Time.deltaTime)
            {
                _animator.SetLayerWeight(1, t / _timeToBlendLayers);
                yield return null;
            }
            _animator.SetLayerWeight(1, 1);
        }
        _blendCoroutine = null;
    }

    protected override void OnFire()
    {
        base.OnFire();
        _audioController.BowRelease();
        _arrowOrigin.transform.LookAt(_playerController.transform);
        _arrowOrigin.transform.rotation = Quaternion.Euler(GetCalculatedAdjustmentDistance(false) + _arrowOrigin.transform.rotation.eulerAngles);

        if (_arrows.Count < MAX_ARROW_INSTANCES)
        {
            ArrowController instance = Instantiate(_arrowInstanceReference);
            instance.Shot(_arrowOrigin.transform, 1);
            _arrows.Add(instance);
        }
        else
        {
            _arrows[_currentArrowIndex].Shot(_arrowOrigin.transform, 1);
            _currentArrowIndex++;
            if (_currentArrowIndex > _arrows.Count - 1)
                _currentArrowIndex = 0;
        }

        _arrowOrigin.SetActive(false);
    }

    protected void OnEndFire()
    {
        StartBlend(true, true);
    }

    private void OnReload()
    {
        _needToReload = false;
        _arrowOrigin.transform.localPosition = _arrowOriginStartLocalPosition;
        _arrowOrigin.SetActive(true);
    }
}
