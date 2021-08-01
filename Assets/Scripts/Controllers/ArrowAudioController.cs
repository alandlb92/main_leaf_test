using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAudioController : BaseAudioController
{
    public void ArrowInpact()
    {
        if (_audioLib.ArrowInpact == null || _audioLib.ArrowInpact.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.ArrowInpact[Random.Range(0, _audioLib.ArrowInpact.Length)]);
    }

    public void ArrowShot()
    {
        VerifyAudioSource();
        if (_audioLib.ArrowShot == null || _audioLib.ArrowShot.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.ArrowShot[Random.Range(0, _audioLib.ArrowShot.Length)]);
    }

}
