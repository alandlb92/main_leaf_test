using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : BaseWeaponController
{
    [SerializeField] private float _timeToBlendLayers = .5f;
    [SerializeField] private Transform _arrowOrigin;
    [SerializeField] private GameObject _arrowMesh;
    [SerializeField] private ArrowController _arrowInstanceReference;

    private List<ArrowController> _arrows = new List<ArrowController>();
    private bool _atackLayer;
    private bool _reloaded = true;
    private Coroutine _blendCoroutine;
    private float _arrowForce;

    private int _currentArrowIndex = 0;
    private const int MAX_ARROW_INSTANCES = 10;
    private const float MIN_ARROW_FORCE = 0.2f;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (_reloaded)
        {
            if(Input.GetMouseButtonUp(0))
            {
                    _arrowForce = 0;

                float clipTime = Mathf.Clamp(_animator.GetCurrentAnimatorStateInfo(1).normalizedTime,
                    0, _animator.GetCurrentAnimatorClipInfo(1)[0].clip.length);
                float amount = clipTime / _animator.GetCurrentAnimatorClipInfo(1)[0].clip.length;

                if (_animator.GetCurrentAnimatorClipInfo(1)[0].clip.name == "PrepareBow")
                    _arrowForce = 0;
                else
                    _arrowForce = amount;

                _arrowForce = Mathf.Clamp(_arrowForce, MIN_ARROW_FORCE, 1);
            }

            _animator.SetBool("Mouse0", _playerController.PlayerInputController.Mouse0Clicked);
            if (_playerController.PlayerInputController.Mouse0Clicked && !_atackLayer)
            {
                StartBlend(false);
                _playerController.PlayerAudioController.BowPrepare();
                _atackLayer = true;
            }
            else if (_atackLayer)
            {
                _arrowOrigin.LookAt(_playerController.AimTransform);
            }
        }
    }

    private void Shot()
    {
        _playerController.PlayerAudioController.BowRelease();
        _arrowMesh.SetActive(false);
        StartBlend(true);
        _reloaded = false;
        _atackLayer = false;
        _arrowOrigin.gameObject.SetActive(false);
        if (_arrows.Count < MAX_ARROW_INSTANCES)
        {
            ArrowController arrowInstance = Instantiate(_arrowInstanceReference);
            arrowInstance.Shot(_arrowOrigin, _arrowForce);
            _arrows.Add(arrowInstance);
        }
        else
        {
            _arrows[_currentArrowIndex].Shot(_arrowOrigin, _arrowForce);
            _currentArrowIndex++;
            if (_currentArrowIndex > _arrows.Count - 1)
                _currentArrowIndex = 0;
        }
    }

    private void StartBlend(bool backToNormal)
    {
        if (_blendCoroutine != null)
            StopCoroutine(_blendCoroutine);

        _blendCoroutine = StartCoroutine(BlendLayers(backToNormal));
    }

    private IEnumerator BlendLayers(bool backToNormal)
    {
        if (backToNormal)
        {
            for (float t = (_timeToBlendLayers * _animator.GetLayerWeight(1)); t > 0; t -= Time.deltaTime)
            {
                _animator.SetLayerWeight(1, t / _timeToBlendLayers);
                yield return null;
            }
            _animator.SetLayerWeight(1, 0);
            _animator.SetTrigger("Reload");
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

    public void EndReload()
    {
        _reloaded = true;
        _arrowMesh.SetActive(true);
    }
}
