using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUI
{
    [SerializeField] private TMP_Dropdown _gameTimeDropDown;
    [SerializeField] private Slider _countDownSlider;
    [SerializeField] private Slider _ambienceAudioSlider;
    [SerializeField] private Slider _vfxAudioSlider;
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _backButton;

    [SerializeField] private float[] _gameTimeOptions;
    private GameSettingsModel _settings;
    private StartScreenUI _startScreenUI;
    private SliderValue[] _sliderValues;

    public override void Open()
    {
        base.Open();
        _settings = Settings.ConfigCopy();
        _gameTimeDropDown.value = GetIndex(_settings.GameTimeMinutes);
        _countDownSlider.value = _settings.CountDownSeconds;
        _ambienceAudioSlider.value = _settings.AmbienceAudio;
        _vfxAudioSlider.value = _settings.VfxAudio;
        foreach(SliderValue sliderValue in _sliderValues)
        {
            sliderValue.UpdateText();
        }

    }

    protected override void Awake()
    {
        _sliderValues = GetComponentsInChildren<SliderValue>();
        _startScreenUI = transform.parent.GetComponentInChildren<StartScreenUI>();
        _applyButton.onClick.AddListener(OnApply);
        _backButton.onClick.AddListener(OnBack);
        base.Awake();
        _gameTimeDropDown.ClearOptions();
        foreach(float t in _gameTimeOptions)
        {
            _gameTimeDropDown.options.Add(new TMP_Dropdown.OptionData(t.ToString()));
        }

        _gameTimeDropDown.onValueChanged.AddListener(OnChangeGameTime);
        _countDownSlider.onValueChanged.AddListener(OnChangeCountDown);
        _ambienceAudioSlider.onValueChanged.AddListener(OnChangeAmbienceAudio);
        _vfxAudioSlider.onValueChanged.AddListener(OnChangeVfxAudio);
    }

    private int GetIndex(float value)
    {
        for(int i = 0;i < _gameTimeOptions.Length;i++)
        {
            if (_gameTimeOptions[i] == value)
                return i;
        }
        return 0;
    }

    private void OnBack()
    {
        Close();
        _startScreenUI.Open();
    }

    private void OnApply()
    {
        Close();
        Settings.ApplySettings(_settings);
        _startScreenUI.Open();
    }

    private void OnChangeGameTime(int index)
    {
        _settings.GameTimeMinutes = _gameTimeOptions[index];
    }

    private void OnChangeCountDown(float value)
    {
        _settings.CountDownSeconds = (int) value;
    }

    private void OnChangeAmbienceAudio(float value)
    {
        _settings.AmbienceAudio = value;
    }

    private void OnChangeVfxAudio(float value)
    {
        _settings.VfxAudio = value;
    }
}
