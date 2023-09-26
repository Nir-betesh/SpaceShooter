using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioClip _audioExplosionClip;
    private AudioSource _audioExplosionSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioExplosionSource = GetComponent<AudioSource>();
        if (_audioExplosionSource == null)
            Debug.LogError("MY ERROR: _audioExplosionSource is NULL! Astroid::Start()");
        else
            _audioExplosionSource.clip = _audioExplosionClip;

        _audioExplosionSource.Play();
        Destroy(this.gameObject, 3.0f);
    }
}
