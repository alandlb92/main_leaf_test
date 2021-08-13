using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class GameSettingsModel
{
    public float AmbienceAudio;
    public float VfxAudio;
    public float GameTimeMinutes;
    public int CountDownSeconds;

    public GameSettingsModel()
    {
        AmbienceAudio = 1;
        VfxAudio = 1;
        GameTimeMinutes = 3;
        CountDownSeconds = 3;
    }
    public GameSettingsModel(GameSettingsModel config)
    {
        AmbienceAudio = config.AmbienceAudio;
        VfxAudio = config.VfxAudio;
        GameTimeMinutes = config.GameTimeMinutes;
        CountDownSeconds = config.CountDownSeconds;
    }
}
