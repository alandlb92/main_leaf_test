using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : BaseUI
{
    [SerializeField] private TMP_Text _archersCountText;
    [SerializeField] private TMP_Text _meleeCountText;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private Button _exitButton;
    private GameManager _gameManager;

    public void ShowGameOver(int archerCount, int meleesCount)
    {
        Cursor.visible = true;
        Open();
        _archersCountText.text = "ARCHERS " + archerCount;
        _meleeCountText.text = "MELEES " + meleesCount;
        _score.text = "SCORE:" + (archerCount + (meleesCount * 3));
    }

    protected override void Awake()
    {
        base.Awake();
        _gameManager = FindObjectOfType<GameManager>();
        _playAgainButton.onClick.AddListener(PlayAgain);
        _exitButton.onClick.AddListener(Exit);
    }

    private void PlayAgain()
    {
        _gameManager.PlayAgain();
        Close();
    }

    private void Exit()
    {
        Application.Quit();
    }
}
