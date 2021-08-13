using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private bool _hitSomething = false;
    private Rigidbody _rigidbody;
    private MeshRenderer _meshRenderer;
    private BoxCollider _collider;
    private ArrowAudioController _audioController;
    [SerializeField] private float _force = 15;

    public void Shot(Transform startOrigin, float forceMultiply)
    {
        if (forceMultiply > 0.5f)
        {
            if(_audioController == null)
                _audioController = GetComponent<ArrowAudioController>();

            _audioController.ArrowShot();
        }

        this.transform.parent = null;
        _meshRenderer.enabled = true;
        _rigidbody.isKinematic = false;
        _rigidbody.constraints = RigidbodyConstraints.None;
        this.transform.position = startOrigin.position;
        this.transform.rotation = startOrigin.rotation;
        _collider.enabled = true;
        _rigidbody.velocity = this.transform.forward.normalized * _force * forceMultiply;

        CorotineUtils.WaitFixedUpdateAndExecute(this, () =>
        {
            _hitSomething = false;
        });
    }

    public void SetVisible(bool visible)
    {
        _meshRenderer.enabled = visible;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponent<BoxCollider>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _meshRenderer.enabled = false;
        _collider.enabled = false;
        _audioController = GetComponent<ArrowAudioController>();
    }

    private void FixedUpdate()
    {
        if (!_hitSomething)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_hitSomething || collision.GetComponent<ArrowController>() != null 
            || collision.GetComponent<MeleeEnemyWeaponController>() != null 
            || collision.gameObject.CompareTag("Loot"))
            return;

        IDamage damage = collision.gameObject.GetComponentInParent<IDamage>();
        if (damage != null)
            damage.TakeDamage(1);

        _audioController.ArrowInpact();
        _collider.enabled = false;
        _hitSomething = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        this.transform.parent = collision.transform;
    }
}
