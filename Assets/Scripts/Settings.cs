using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
   
    private static GameSettingsModel _gameSettings;
    public static GameSettingsModel GameSettings => _gameSettings;
    private const string ConfigKeyString = "GameConfig";

    public static void ApplySettings(GameSettingsModel gameConfig)
    {
        _gameSettings = gameConfig;
        PlayerPrefs.SetString(ConfigKeyString, JsonUtility.ToJson(_gameSettings));
    }

    public static GameSettingsModel ConfigCopy()
    {
        return new GameSettingsModel(_gameSettings);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    { 
        if(PlayerPrefs.HasKey(ConfigKeyString))
        {
            _gameSettings = JsonUtility.FromJson<GameSettingsModel>(PlayerPrefs.GetString(ConfigKeyString));
        }
        else
        {
            _gameSettings = new GameSettingsModel();
        }
    }
}
