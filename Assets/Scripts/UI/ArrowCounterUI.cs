using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;
    public void SetArrowCounter(int arrows)
    {
        _countText.text = "x" + arrows.ToString();
    }
}
