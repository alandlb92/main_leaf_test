using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WavesConfig", menuName = "ScriptableObjects/WavesConfig", order = 1)]
public class WavesScriptable : ScriptableObject
{
    public WaveConfig[] WaveConfigs => _waveConfigs;

    [SerializeField] private WaveConfig[] _waveConfigs;

}

[Serializable]
public class WaveConfig
{
    public string Name;
    public int Archer;
    public int Melee;
}
