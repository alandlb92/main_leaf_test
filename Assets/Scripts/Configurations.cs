using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Configurations
{
   
    private static GameConfigModel _gameConfig;
    public static GameConfigModel GameConfig => _gameConfig;
    private const string ConfigKeyString = "GameConfig";

    public static void ApplyConfiguration(GameConfigModel gameConfig)
    {
        _gameConfig = gameConfig;
        PlayerPrefs.SetString(ConfigKeyString, JsonUtility.ToJson(_gameConfig));
    }

    public static GameConfigModel ConfigCopy()
    {
        return new GameConfigModel(_gameConfig);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    { 
        if(PlayerPrefs.HasKey(ConfigKeyString))
        {
            _gameConfig = JsonUtility.FromJson<GameConfigModel>(PlayerPrefs.GetString(ConfigKeyString));
        }
        else
        {
            _gameConfig = new GameConfigModel();
        }
    }
}
