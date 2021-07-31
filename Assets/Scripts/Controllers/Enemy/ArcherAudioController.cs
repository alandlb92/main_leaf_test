using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAudioController : BaseAudioController
{
    public void Die()
    {
        if (_audioLib.ArcherDie == null || _audioLib.ArcherDie.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.ArcherDie[Random.Range(0, _audioLib.ArcherDie.Length)]);
    }
}
