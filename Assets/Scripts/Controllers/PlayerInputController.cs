using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private bool _mouse0IsClicked;
    public bool Mouse0Clicked { get => _mouse0IsClicked; }
    public Action OnPressedSpace;

    public void ResetInput()
    {
        _mouse0IsClicked = false;
    }

    private void Update()
    {
        _mouse0IsClicked = Input.GetMouseButton(0);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnPressedSpace?.Invoke();
        }
    }
}
