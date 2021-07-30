using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLib", menuName = "ScriptableObjects/AudioLib", order = 1)]
public class AudioLib : ScriptableObject
{
    [SerializeField] private AudioClip[] _playerHurt;
    [SerializeField] private AudioClip[] _archerHurt;
    [SerializeField] private AudioClip[] _meleeHurt;
    [SerializeField] private AudioClip[] _bowRelease;
    [SerializeField] private AudioClip[] _bowPrepare;
    [SerializeField] private AudioClip[] _bgSound;

    public AudioClip[] PlayerHurt => _playerHurt;
    public AudioClip[] ArcherHurt => _archerHurt;
    public AudioClip[] MeleeHurt => _meleeHurt;
    public AudioClip[] BowRelease => _bowRelease;
    public AudioClip[] BowPrepare => _bowPrepare;
    public AudioClip[] BgSound => _bgSound;
}
