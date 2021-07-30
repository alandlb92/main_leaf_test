using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientAudioController : AudioController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _audioSource = Camera.main.GetComponent<AudioSource>();
        _audioSource.clip = _audioLib.BgSound[0];
        _audioSource.loop = true;
        _audioSource.Play();
    }


}
