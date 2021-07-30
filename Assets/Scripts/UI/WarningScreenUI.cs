using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WarningScreenUI : BaseUI
{
    [SerializeField] private AnimationCurve _tittleMotionCurve;
    [SerializeField] private RectTransform _tittleRectTrasnform;
    [SerializeField] private TMP_Text _tittleText;
    [SerializeField] private TMP_Text _countDownText;
    [SerializeField] private int _countDow = 3;

    private AmbientAudioController _ambientAudioController;

    public void ShowWarnning(string tittle, Action callBack)
    {
        _tittleText.text = tittle;
        Open();
        StartCoroutine(RunShowWarning(callBack));
    }

    protected override void Awake()
    {
        base.Awake();
        _ambientAudioController = FindObjectOfType<AmbientAudioController>();
    }

    private IEnumerator RunShowWarning(Action callBack)
    {
        _countDownText.text = "";
        yield return EnterWarning();
        yield return new WaitForSeconds(1);
        yield return LeaveWarning();
        yield return CountDown();
        _countDownText.text = "Die";
        callBack?.Invoke();
        yield return new WaitForSeconds(1);
        Close();
    }

    private IEnumerator EnterWarning()
    {
        _ambientAudioController.ShowTittle();
        _tittleRectTrasnform.SetLeft(-Screen.width);
        _tittleRectTrasnform.SetRight(Screen.width);

        float position = Screen.width;
        float totalTime = _tittleMotionCurve.keys.Last().time;
        for (float t = 0;t < totalTime; t+= Time.deltaTime)
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
        for(int i = _countDow;i > 0;i--)
        {
            _countDownText.text = i.ToString();
            _ambientAudioController.CountDown();
            yield return new WaitForSeconds(1);
        }
    }
}
