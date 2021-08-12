using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootAudioController : BaseAudioController
{
    public void PopUp()
    {
        if (_audioLib.PopUp == null || _audioLib.PopUp.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.PopUp[Random.Range(0, _audioLib.PopUp.Length)]);
    }

    public void PickUp()
    {
        if (_audioLib.PickUp == null || _audioLib.PickUp.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.PickUp[Random.Range(0, _audioLib.PickUp.Length)]);
    }
}
