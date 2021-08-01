using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLib", menuName = "ScriptableObjects/AudioLib", order = 1)]
public class AudioLib : ScriptableObject
{
    [SerializeField] private AudioClip[] _playerHurt;
    [SerializeField] private AudioClip[] _archerHurt;
    [SerializeField] private AudioClip[] _meleeHurt;
    [SerializeField] private AudioClip[] _meleeSpawn;
    [SerializeField] private AudioClip[] _archerDie;
    [SerializeField] private AudioClip[] _archerSpawn;
    [SerializeField] private AudioClip[] _meleeDie;
    [SerializeField] private AudioClip[] _bowRelease;
    [SerializeField] private AudioClip[] _bowReleaseEmpty;
    [SerializeField] private AudioClip[] _bowPrepare;
    [SerializeField] private AudioClip[] _bgSound;
    [SerializeField] private AudioClip[] _showTittleSound;
    [SerializeField] private AudioClip[] _countDownSound;
    [SerializeField] private AudioClip[] _startWaveSound;
    [SerializeField] private AudioClip[] _stepSound;
    [SerializeField] private AudioClip[] _arrowShot;
    [SerializeField] private AudioClip[] _arrowInpact;
    [SerializeField] private AudioClip[] _playerDie;

    public AudioClip[] PlayerHurt => _playerHurt;
    public AudioClip[] ArcherHurt => _archerHurt;
    public AudioClip[] MeleeHurt => _meleeHurt;
    public AudioClip[] MeleeSpawn => _meleeSpawn;
    public AudioClip[] ArcherSpawn => _archerSpawn;
    public AudioClip[] ArcherDie => _archerDie;
    public AudioClip[] MeleeDie => _meleeDie;
    public AudioClip[] BowRelease => _bowRelease;
    public AudioClip[] BowReleaseEmpty => _bowReleaseEmpty;
    public AudioClip[] BowPrepare => _bowPrepare;
    public AudioClip[] BgSound => _bgSound;
    public AudioClip[] ShowTittleSound => _showTittleSound;
    public AudioClip[] CountDownSound => _countDownSound;
    public AudioClip[] StartWaveSound => _startWaveSound;
    public AudioClip[] StepSound => _stepSound;
    public AudioClip[] ArrowShot => _arrowShot;
    public AudioClip[] ArrowInpact => _arrowInpact;
    public AudioClip[] PlayerDie => _playerDie;
}
