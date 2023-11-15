using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        volumeSlider = gameObject.GetComponent<Slider>();
        

        // Setze den aktuellen Wert des Sliders auf die aktuelle Lautstärke des AudioManager
        volumeSlider.value = audioManager.mixerGroup.audioMixer.GetFloat("Volume", out float volume) ? volume : 0f;

        // Füge den OnValueChanged-Listener hinzu, um die Lautstärke zu aktualisieren, wenn sich der Slider ändert
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    
    }
    

    // Methode, um die Lautstärke zu ändern, wenn sich der Slider-Wert ändert
    void ChangeVolume(float volume)
    {
        // Aktualisiere die Lautstärke im AudioManager
        audioManager.mixerGroup.audioMixer.SetFloat("Volume", volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
