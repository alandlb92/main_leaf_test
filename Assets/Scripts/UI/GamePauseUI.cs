using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : BaseUI
{
    [SerializeField] private Button _resumeGameButton;
    [SerializeField] private Button _exitGameButton;
    private GameManager _gameManager;

    protected override void Awake()
    {
        base.Awake();
        _gameManager = FindObjectOfType<GameManager>();
        _resumeGameButton.onClick.AddListener(ResumeGame);
        _exitGameButton.onClick.AddListener(ExitGame);
    }

    private void ResumeGame()
    {
        _gameManager.PauseGame();
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
