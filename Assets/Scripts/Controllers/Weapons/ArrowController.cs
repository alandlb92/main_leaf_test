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
    [SerializeField] private float _force = 15;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _collider = GetComponent<BoxCollider>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        _meshRenderer.enabled = false;
        _collider.enabled = false;
    }

    private void Start()
    {
    }

    void FixedUpdate()
    {
        if (!_hitSomething)
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
    }

    public void Shot(Transform startOrigin, float forceMultiply)
    {
        this.transform.parent = null;
        _meshRenderer.enabled = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
        this.transform.position = startOrigin.position;
        this.transform.rotation = startOrigin.rotation;
        _rigidbody.velocity = this.transform.forward.normalized * _force * forceMultiply;
        _collider.enabled = true;        
        _hitSomething = false;
    }

    public void SetVisible(bool visible)
    {
        _meshRenderer.enabled = visible;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (_hitSomething || collision.GetComponent<ArrowController>() != null)
            return;

        IDamage damage = collision.gameObject.GetComponentInParent<IDamage>();
        if(damage != null)
            damage.TakeDamage();

        _hitSomething = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        this.transform.parent = collision.transform;
    }
}
