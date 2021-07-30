using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyWeaponController : MonoBehaviour
{
    public Action<Collider> OnWeaponTriggerStay;

    private void OnTriggerStay(Collider other)
    {
        OnWeaponTriggerStay?.Invoke(other);
    }
}
