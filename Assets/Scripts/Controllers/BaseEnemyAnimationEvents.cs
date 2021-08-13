using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAnimationEvents : MonoBehaviour
{
    public Action OnFootL;
    public Action OnFootR;
    public Action OnFire;
    public Action OnEndFire;
    public Action OnReload;

    public void FootL()
    {
        OnFootL?.Invoke();
    }

    public void FootR()
    {
        OnFootR?.Invoke();
    }

    public void Fire()
    {
        OnFire?.Invoke();
    }

    public void EndFire()
    {
        OnEndFire?.Invoke();
    }
    public void Reload()
    {
        OnReload?.Invoke();
    }
}
