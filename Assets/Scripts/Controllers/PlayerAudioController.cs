using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : AudioController
{
    public void Hurt()
    {
        if (_audioLib.PlayerHurt == null || _audioLib.PlayerHurt.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.PlayerHurt[Random.Range(0, _audioLib.PlayerHurt.Length)]);
    }
}
