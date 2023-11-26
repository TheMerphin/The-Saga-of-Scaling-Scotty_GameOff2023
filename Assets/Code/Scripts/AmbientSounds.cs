using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public Sound[] AmbientSounds;

    [Tooltip("Minimum delay sounds are played in seconds")]
    public float MinDelay;
    [Tooltip("Maximum delay sounds are played in seconds")]
    public float MaxDelay;
    [Tooltip("Maximum distance you want the audiosource to be away of the current gameobject")]
    public float MaxAudioSourceRadius;
    private AudioSource ambientPlayer;

    private void Awake()
    {
        ambientPlayer = GetComponent<AudioSource>();
    }

    void Start()
    {
        Invoke("playAmbientSound", Random.Range(MinDelay, MaxDelay));
    }

    private void playAmbientSound()
    {
            setAudioSourceRandomDirection();
            int ambientIndex = Random.Range(0, AmbientSounds.Length);
            ambientPlayer.volume = AmbientSounds[ambientIndex].volume * (1f + UnityEngine.Random.Range(-AmbientSounds[ambientIndex].volumeVariance / 2f, AmbientSounds[ambientIndex].volumeVariance / 2f));
            ambientPlayer.pitch = AmbientSounds[ambientIndex].pitch * (1f + UnityEngine.Random.Range(-AmbientSounds[ambientIndex].pitchVariance / 2f, AmbientSounds[ambientIndex].pitchVariance / 2f)); ;
            ambientPlayer.PlayOneShot(AmbientSounds[ambientIndex].clip);
            Invoke("playAmbientSound", Random.Range(MinDelay, MaxDelay));
    }

    private void setAudioSourceRandomDirection()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        transform.position = randomDirection * MaxAudioSourceRadius;
    }
}
