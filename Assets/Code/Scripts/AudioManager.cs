using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public List<Sound> sounds;

    public Slider volumeSlider;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}

		//volumeSlider = FindFirstObjectByType<Slider>();

		if (volumeSlider != null)
		{
			float volume;

			if (mixerGroup.audioMixer.GetFloat("Volume", out volume))
			{
				volumeSlider.value = volume;
			}
			else
			{
				Debug.LogError("Failed to get the volume value from the AudioMixer.");
			}

			volumeSlider.onValueChanged.AddListener(ChangeVolume);
		}
		else
		{
			Debug.LogError("Volume Slider not found. Please assign the Volume Slider in the inspector.");
		}

	}

	public void setVolume(float volume) 
	{
	Debug.Log(volume);
			
	}

	// Methode, um die Lautst�rke zu �ndern, wenn sich der Slider-Wert �ndert
	public void ChangeVolume(float volume)
    {
        // Aktualisiere die Lautst�rke im AudioManager
        mixerGroup.audioMixer.SetFloat("Volume", volume);
    }

    public void AddSound(Sound sound)
	{
		sounds.Add(sound);

        sound.source = gameObject.AddComponent<AudioSource>();
        sound.source.clip = sound.clip;
        sound.source.loop = sound.loop;

        sound.source.outputAudioMixerGroup = mixerGroup;
    }

	public void Play(string sound)
	{
		Sound s = sounds.Find(item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}

	public void Stop(string sound)
	{
		StopSound(sound, false, 0f, 0f);
	}

	public void Stop(string sound, bool fadeOut, float fadeOutAfter, float fadeOutOver)
	{
		StopSound(sound, fadeOut, fadeOutAfter, fadeOutOver);
	}

	private void StopSound(string sound, bool fadeOut, float fadeOutAfter, float fadeOutOver)
	{
		Sound s = sounds.Find(item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

        if (fadeOut)
        {
			StartCoroutine(FadeOut(s, fadeOutAfter, fadeOutOver));
        }
        else
        {
			s.source.Stop();
		}
	}

	private IEnumerator FadeOut(Sound s, float fadeOutAfter, float fadeOutOver)
    {
		yield return new WaitForSeconds(fadeOutAfter);

		var steps = 200;
		var soundSteps = s.source.volume / steps;

		for (int i = 0; i < steps; i++)
        {
			s.source.volume -= soundSteps;
			yield return new WaitForSeconds(fadeOutOver / steps);
        }

		s.source.Stop();
	}
}
