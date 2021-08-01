using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WarningScreenUI : BaseUI
{
    public enum Choose
    {
        LIFE,
        ARROWS
    }

    [SerializeField] private AnimationCurve _tittleMotionCurve;
    [SerializeField] private RectTransform _tittleRectTrasnform;
    [SerializeField] private GameObject _lifeOrArrows;
    [SerializeField] private TMP_Text _tittleText;
    [SerializeField] private TMP_Text _countDownText;
    [SerializeField] private int _countDow = 3;

    private AmbientAudioController _ambientAudioController;
    private bool _waiForChoose = false;
    private Action<Choose> OnChooseCallBack;

    public void ShowWarnning(string tittle, Action callBack, Action<Choose> chooseCallBack, bool showChoose)
    {
        _waiForChoose = showChoose;
        _tittleText.text = tittle;
        OnChooseCallBack = chooseCallBack;
        Open();
        StartCoroutine(RunShowWarning(callBack, showChoose));
    }

    protected override void Awake()
    {
        base.Awake();
        _ambientAudioController = FindObjectOfType<AmbientAudioController>();
    }

    private void Update()
    {
        if (_waiForChoose)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                OnChooseCallBack?.Invoke(Choose.LIFE);
                _waiForChoose = false;
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                OnChooseCallBack?.Invoke(Choose.ARROWS);
                _waiForChoose = false;
            }
        }
    }

    private IEnumerator RunShowWarning(Action callBack, bool showChoose)
    {
        _countDownText.text = "";
        _tittleRectTrasnform.SetLeft(-Screen.width);
        _tittleRectTrasnform.SetRight(Screen.width);
        _lifeOrArrows.SetActive(false);

        if (showChoose)
            yield return ChoseLifeOrArrows();

        yield return EnterWarning();
        yield return new WaitForSeconds(1);
        yield return LeaveWarning();
        yield return CountDown();
        _countDownText.text = "Die";
        callBack?.Invoke();
        yield return new WaitForSeconds(1);
        Close();
    }

    public IEnumerator ChoseLifeOrArrows()
    {
        _lifeOrArrows.SetActive(true);
        while (_waiForChoose)
        {
            yield return null;
        }
        _lifeOrArrows.SetActive(false);
    }

    private IEnumerator EnterWarning()
    {
        _ambientAudioController.ShowTittle();
        _tittleRectTrasnform.SetLeft(-Screen.width);
        _tittleRectTrasnform.SetRight(Screen.width);

        float position = Screen.width;
        float totalTime = _tittleMotionCurve.keys.Last().time;
        for (float t = 0; t < totalTime; t += Time.deltaTime)
        {
            position = Screen.width - (_tittleMotionCurve.Evaluate(t) * Screen.width);
            _tittleRectTrasnform.SetLeft(-position);
            _tittleRectTrasnform.SetRight(position);
            yield return null;
        }
        _tittleRectTrasnform.SetLeft(0);
        _tittleRectTrasnform.SetRight(0);
    }
    private IEnumerator LeaveWarning()
    {
        _tittleRectTrasnform.SetLeft(0);
        _tittleRectTrasnform.SetRight(0);

        float position = 0;
        float totalTime = _tittleMotionCurve.keys.Last().time;
        for (float t = 0; t < totalTime; t += Time.deltaTime)
        {
            position = (_tittleMotionCurve.Evaluate(t) * Screen.width);
            _tittleRectTrasnform.SetLeft(position);
            _tittleRectTrasnform.SetRight(-position);
            yield return null;
        }
        _tittleRectTrasnform.SetLeft(Screen.width);
        _tittleRectTrasnform.SetRight(-Screen.width);
    }

    private IEnumerator CountDown()
    {
        for (int i = _countDow; i > 0; i--)
        {
            _countDownText.text = i.ToString();
            _ambientAudioController.CountDown();
            yield return new WaitForSeconds(1);
        }
    }
}
