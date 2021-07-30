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
    public void ShowTittle()
    {
        if (_audioLib.ShowTittleSound == null || _audioLib.ShowTittleSound.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.ShowTittleSound[Random.Range(0, _audioLib.ShowTittleSound.Length)]);
    }

    public void CountDown()
    {
        if (_audioLib.CountDownSound == null || _audioLib.CountDownSound.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.CountDownSound[Random.Range(0, _audioLib.CountDownSound.Length)]);
    }

    public void StartWave()
    {
        if (_audioLib.StartWaveSound == null || _audioLib.StartWaveSound.Length == 0)
            return;

        _audioSource.PlayOneShot(_audioLib.StartWaveSound[Random.Range(0, _audioLib.StartWaveSound.Length)]);
    }

}
