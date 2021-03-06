using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAudioController : BaseAudioController
{
    public void Spawn()
    {
        VerifyAudioSource();
        if (_audioLib.MeleeSpawn == null || _audioLib.MeleeSpawn.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.MeleeSpawn[Random.Range(0, _audioLib.MeleeSpawn.Length)]);
    }
    public void Die()
    {
        if (_audioLib.MeleeDie == null || _audioLib.MeleeDie.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.MeleeDie[Random.Range(0, _audioLib.MeleeDie.Length)]);
    }

    public void Hurt()
    {
        if (_audioLib.MeleeHurt == null || _audioLib.MeleeHurt.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.MeleeHurt[Random.Range(0, _audioLib.MeleeHurt.Length)]);
    }
}
