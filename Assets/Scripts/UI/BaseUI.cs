using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private Canvas _root;

    protected virtual void Awake()
    {
        _root = GetComponent<Canvas>();
    }

    public virtual void Open()
    {
        _root.enabled = true;
    }

    public void Close()
    {
        _root.enabled = false;
    }
}
