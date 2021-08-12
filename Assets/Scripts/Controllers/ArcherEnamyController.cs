using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnamyController : BaseEnemyController
{
    [SerializeField] private GameObject _spine1;
    [SerializeField] private Vector3 _rotationSpine1Offset;
    [SerializeField] private float _timeToBlendLayers = .5f;
    [SerializeField] private GameObject _arrowOrigin;
    [SerializeField] private ArrowController _arrowInstanceReference;

    [SerializeField] private Vector2 _fireShotDesitionRangeTime = new Vector2(0.2f, 1);

    private bool _needToReload;
    private Coroutine _fireRotine;

    protected override void Awake()
    {
        base.Awake();
        _animationEvents.OnEndFire = OnEndFire;
        _animationEvents.OnReload = OnReload;
    }

    protected override void Update()
    {
        if (!_needToReload)
        {
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
    }

    private void LateUpdate()
    {
        if (_attackMode)
        {
            if (_blendCoroutine == null)
            {
                _spine1.transform.LookAt(_playerController.transform);
                _spine1.transform.rotation = Quaternion.Euler(_spine1.transform.rotation.eulerAngles + _rotationSpine1Offset);
                if (_fireRotine == null && !_needToReload)
                    _fireRotine = StartCoroutine(Fire());
            }
        }
    }

    private IEnumerator Fire()
    {
        float waitToAttack = Random.Range(_fireShotDesitionRangeTime.x, _fireShotDesitionRangeTime.y);
        Debug.Log(waitToAttack);
        yield return new WaitForSeconds(waitToAttack);
        _needToReload = true;
        _animator.SetTrigger("Attack");
        _fireRotine = null;
    }

    private void StartBlend(bool backToNormal, bool reload = false)
    {
        if (_blendCoroutine != null)
            StopCoroutine(_blendCoroutine);

        _blendCoroutine = StartCoroutine(SetAtackMode(backToNormal, reload));
    }
    private Coroutine _blendCoroutine;
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
            if(reload)
            {
                _animator.SetTrigger("Reload");
                Debug.Log("Reload");
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
        Debug.Log("OnFire");
        base.OnFire();
        _arrowOrigin.transform.LookAt(_playerController.transform);
        ArrowController instance = Instantiate(_arrowInstanceReference);
        instance.Shot(_arrowOrigin.transform, 1);
        _arrowOrigin.SetActive(false);
    }

    protected void OnEndFire()
    {
        StartBlend(true, true);
    }

    private void OnReload()
    {
        _needToReload = false;
        _arrowOrigin.SetActive(true);
    }
}
