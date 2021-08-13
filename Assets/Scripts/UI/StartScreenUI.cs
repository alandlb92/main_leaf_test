using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenUI : BaseUI
{
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _exitButton;
    private SceneLoader _sceneLoader;
    private SettingsUI _settignsUI;

    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        _gameStartButton.onClick.AddListener(StartGame);
        _settingsButton.onClick.AddListener(OpenSettingsScreen);
        _exitButton.onClick.AddListener(ExitGame);
        _sceneLoader = transform.parent.GetComponentInChildren<SceneLoader>();
        _settignsUI = transform.parent.GetComponentInChildren<SettingsUI>();
    }

    private void StartGame()
    {
        Close();
        _sceneLoader.LoadScene("GamePlayScene");
    }

    private void OpenSettingsScreen()
    {
        Close();
        _settignsUI.Open();
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
