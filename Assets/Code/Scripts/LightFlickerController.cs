using UnityEngine;


/**
 * File Name: LightFlicker.cs
 * Description: Script for flickering a light source; the default configuration aims to mimic fire
 *                  light
 * 
 * Authors: LaneFox [Unity Forum Profile], Will Lacey
 * Date Created: March 17, 2021
 * 
 * Additional Comments: 
 *      The original version of this file can be found here:
 *      https://answers.unity.com/questions/742466/camp-fire-light-flicker-control.html on the 
 *      forum "Camp fire light flicker control" written by LaneFox; this file has been updated it
 *      to better fit this project
 *      
 *      Line Length: 100 Characters
 **/
public class LightFlickerController : MonoBehaviour
{
    /************************************************************/
    #region Variables

    [Tooltip("maximum possible intensity the light can flicker to")]
    [SerializeField, Min(0)] float maxIntensity = 150f;

    [Tooltip("minimum possible intensity the light can flicker to")]
    [SerializeField, Min(0)] float minIntensity = 50f;

    [Tooltip("maximum frequency of often the light will flicker in seconds")]
    [SerializeField, Min(0)] float maxFlickerFrequency = 1f;

    [Tooltip("minimum frequency of often the light will flicker in seconds")]
    [SerializeField, Min(0)] float minFlickerFrequency = 0.1f;

    [Tooltip("how fast the light will flicker to it's next intensity (a very high value will)" +
        "look like a dying lightbulb while a lower value will look more like an organic fire")]
    [SerializeField, Min(0)] public float strength = 5f;

    float baseIntensity;
    float nextIntensity;

    float flickerFrequency;
    float timeOfLastFlicker;

    #endregion
    /************************************************************/
    #region Class Functions

    private UnityEngine.Rendering.Universal.Light2D LightSource => GetComponent<UnityEngine.Rendering.Universal.Light2D>();

    #endregion
    /************************************************************/
    #region Class Functions

    #region Unity Functions

    private void OnValidate()
    {
        if (maxIntensity < minIntensity) minIntensity = maxIntensity;
        if (maxFlickerFrequency < minFlickerFrequency) minFlickerFrequency = maxFlickerFrequency;
    }

    private void Awake()
    {
        baseIntensity = LightSource.intensity;

        timeOfLastFlicker = Time.time;
    }

    private void Update()
    {
        if (timeOfLastFlicker + flickerFrequency < Time.time)
        {
            timeOfLastFlicker = Time.time;
            nextIntensity = Random.Range(minIntensity, maxIntensity);
            flickerFrequency = Random.Range(minFlickerFrequency, maxFlickerFrequency);
        }

        Flicker();
    }

    #endregion

    #region Other Light Functions

    private void Flicker()
    {
        LightSource.intensity = Mathf.Lerp(
            LightSource.intensity,
            nextIntensity,
            strength * Time.deltaTime
        );
    }

    public void Reset()
    {
        LightSource.intensity = baseIntensity;
    }

    #endregion

    #endregion
}
