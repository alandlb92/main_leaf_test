using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private bool _isPercentage;
    private TMP_Text _text;

    public void UpdateText()
    {
        if (_isPercentage)
            _text.text = (int)(_slider.value * 100) + "%";
        else
            _text.text = _slider.value.ToString();
    }

    private void Awake()
    {
        _text = this.GetComponent<TMP_Text>();
        _slider.onValueChanged.AddListener(OnChangeValue);
    }

    private void OnChangeValue(float value)
    {
        UpdateText();
    }
}
