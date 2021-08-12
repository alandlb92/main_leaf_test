using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenUI : BaseUI
{
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _configurationButton;
    [SerializeField] private Button _exitButton;
    private SceneLoader _sceneLoader;

    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _gameStartButton.onClick.AddListener(StartGame);
        _configurationButton.onClick.AddListener(OpenConfigurationScreen);
        _exitButton.onClick.AddListener(ExitGame);
        _sceneLoader = transform.parent.GetComponentInChildren<SceneLoader>();
    }

    private void StartGame()
    {
        Close();
        _sceneLoader.LoadScene("GamePlayScene");
    }

    private void OpenConfigurationScreen()
    {
        Close();
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
