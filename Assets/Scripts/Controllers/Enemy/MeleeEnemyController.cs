using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : BaseEnemyController
{
    [SerializeField] private Vector2 _atackDesitionRangeTime = new Vector2(0.2f, 1);

    private MeleeEnemyWeaponController _meleeEnemyWeaponController;
    private bool _canHit;
    private Coroutine _atackRotine;

    public override void TakeDamage()
    {
        _animator.ResetTrigger("Atack1");
        _animator.ResetTrigger("Atack2");
        _animator.SetTrigger("TakeDamage");
        base.TakeDamage();
    }

    protected override void Awake()
    {
        base.Awake();
        _meleeEnemyWeaponController = GetComponentInChildren<MeleeEnemyWeaponController>();
        _meleeEnemyWeaponController.OnWeaponTriggerStay = OnWeaponTriggerStay;
        _animationEvents.OnStartHit = OnStartHit;
        _animationEvents.OnEndtHit = OnEndHit;
    }

    protected override void Update()
    {
        if (IsDead || !_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_Walk_Run"))
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        base.Update();

        if (_attackMode)
        {
            _canHit = false;
            FaceTarget(_playerController.transform.position);
            if (_atackRotine == null && Vector3.Distance(transform.position, _playerController.transform.position) <= _distanceToApproachTheEnemy)
                _atackRotine = StartCoroutine(AtackRotine());
        }
    }

    private IEnumerator AtackRotine()
    {
        float waitToAttack = Random.Range(_atackDesitionRangeTime.x, _atackDesitionRangeTime.y);
        yield return new WaitForSeconds(waitToAttack);
        bool atack1 = Random.Range(0, 2) == 0 ? true : false;
        bool atack2 = Random.Range(0, 5) == 0 ? true : false;

        if (atack1)
            _animator.SetTrigger("Atack1");
        if (atack2)
            _animator.SetTrigger("Atack2");

        _atackRotine = null;
    }

    private void OnStartHit()
    {
        _canHit = true;
    }

    private void OnEndHit()
    {
        _canHit = false;
    }

    private void OnWeaponTriggerStay(Collider collision)
    {
        MeleeEnemyController hitMelee = collision.GetComponentInParent<MeleeEnemyController>();
        if (!_canHit || hitMelee != null)
            return;

        IDamage damage = collision.gameObject.GetComponentInParent<IDamage>();
        if (damage != null && damage != (IDamage) this)
        {
            _canHit = false;
            damage.TakeDamage();
        }
    }
}
