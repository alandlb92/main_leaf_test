using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    protected AudioSource _audioSource;
    protected static AudioLib _audioLib;

    protected virtual void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioLib == null)
            _audioLib = Resources.Load<AudioLib>("AudioLib");
    }

    public void BowRelease()
    {
        if (_audioLib.BowRelease == null || _audioLib.BowRelease.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.BowRelease[Random.Range(0, _audioLib.BowRelease.Length)]);
    }
    public void BowPrepare()
    {
        if (_audioLib.BowPrepare == null || _audioLib.BowPrepare.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.BowPrepare[Random.Range(0, _audioLib.BowPrepare.Length)]);
    }
}
