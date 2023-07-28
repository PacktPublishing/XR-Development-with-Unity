using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource _soundSource;
    public string tag;

    // Start is called before the first frame update
    void Start()
    {
        _soundSource = GetComponent<AudioSource>();
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(tag))
        {
            _soundSource.PlayOneShot(soundClip);
        }
    }
}
