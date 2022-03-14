using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioClip _sfxClipExplosion;
    private AudioSource _sfxExplosion;

    // Start is called before the first frame update
    void Start()
    {
        _sfxExplosion = GetComponent<AudioSource>();
        if (_sfxExplosion == null)
        {
            Debug.LogError("Explosion::Start() Called. _sfxExplosion is NULL.");
        }
        else
        {
            _sfxExplosion.clip = _sfxClipExplosion;
        }
        _sfxExplosion.Play(0);
        Destroy(this.gameObject, 2.75f);
    }

}
