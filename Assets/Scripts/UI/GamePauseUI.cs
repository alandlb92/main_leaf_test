using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : BaseUI
{
    [SerializeField] private Button _resumeGameButton;
    [SerializeField] private Button _mainMenuButton;
    private GameManager _gameManager;
    private SceneLoader _sceneLoader;

    protected override void Awake()
    {
        base.Awake();
        _gameManager = FindObjectOfType<GameManager>();
        _resumeGameButton.onClick.AddListener(ResumeGame);
        _mainMenuButton.onClick.AddListener(MainMenu);
        _sceneLoader = transform.parent.GetComponentInChildren<SceneLoader>();
    }

    private void ResumeGame()
    {
        _gameManager.PauseGame();
    }

    private void MainMenu()
    {
        _gameManager.PauseGame();
        Close();
        _sceneLoader.LoadScene("StartScene");
    }

}
