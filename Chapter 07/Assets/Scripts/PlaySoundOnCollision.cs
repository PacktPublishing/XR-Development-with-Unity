using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnCollision : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource _soundSource;
    public string tag;

    public bool enableVelocity = true;
    public float minimumVelocity = 0;
    public float maximumVelocity = 3;

    public ParticleSystem fogParticleSystem;  // Reference to the Particle System

    // Start is called before the first frame update
    void Start()
    {
        _soundSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(tag))
        {
            VelocityEstimator velEstimator = other.GetComponent<VelocityEstimator>();
            if (velEstimator && enableVelocity)
            {
                float velocityMagnitude = velEstimator.GetVelocityEstimate().magnitude;
                float soundVolume = Mathf.InverseLerp(minimumVelocity, maximumVelocity, velocityMagnitude);

                _soundSource.PlayOneShot(soundClip, soundVolume);

                // Convert the velocity into an integer number of particles
                int numParticles = Mathf.RoundToInt(velocityMagnitude * 10f);

                // Create an EmitParams instance
                ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams();

                // Emit the particles
                fogParticleSystem.Emit(emitParams, numParticles);
            }
            else
            {
                _soundSource.PlayOneShot(soundClip);
            }
        }
    }
}
