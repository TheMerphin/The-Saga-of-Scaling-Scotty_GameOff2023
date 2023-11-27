using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider volumeSlider;
    private AudioManager audioManager;

    private void Awake()
    {
        volumeSlider = GetComponent<Slider>();
    }

    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        volumeSlider.onValueChanged.AddListener(OnValueChanged);
        volumeSlider.value = 0;
    }

    private void OnValueChanged(float volume)
    {
        audioManager.ChangeVolume(volume);
    }
}
