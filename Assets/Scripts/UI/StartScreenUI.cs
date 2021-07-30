using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenUI : BaseUI
{
    [SerializeField] private Button _gameStartButton;
    private GameManager _gameManager;

    protected override void Awake()
    {
        base.Awake();
        _gameManager = FindObjectOfType<GameManager>();
        _gameStartButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        _gameManager.StartGame();
        Close();
    }
}
