using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUI : BaseUI
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private float _damageTime = 1;

    private int _lastHeartCounter;
    private Coroutine _takeDamageCoroutine;
    private HeartCounterUI _heartCounterUI;
    private ArrowCounterUI _arrowCounterUI;

    protected override void Awake()
    {
        base.Awake();
        _image.enabled = false;
        _heartCounterUI = GetComponentInChildren<HeartCounterUI>();
        _arrowCounterUI = GetComponentInChildren<ArrowCounterUI>();
    }

    public void TakeDamage()
    {
        _image.enabled = true;
        if (_takeDamageCoroutine != null)
            StopCoroutine(_takeDamageCoroutine);

        _takeDamageCoroutine = CorotineUtils.WaiSecondsAndExecute(this, _damageTime, () =>
        {
            _image.enabled = false;
            _takeDamageCoroutine = null;
        });
    }
    
    public void SetHeart(int max, int current)
    {
        _heartCounterUI.SetHealth(max, current);
        if (_lastHeartCounter > current)
            TakeDamage();

        _lastHeartCounter = current;
    }
    
    public void SetArrows(int count)
    {
        _arrowCounterUI.SetArrowCounter(count);
    }

    public void SetTime(float time)
    {
        var duration = TimeSpan.FromSeconds(time);        
        _timeText.text = $"{string.Format("{0:00}", duration.Minutes)}:{string.Format("{0:00}", duration.Seconds)}";
    }
}
