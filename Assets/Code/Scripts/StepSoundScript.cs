using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class StepSoundScript : MonoBehaviour
{
    [Tooltip("order: stone,wood,sand,water.")]
    public Sound[] Stepsounds;
    [Tooltip("please attach the tilemap \"Floortype\" here.")]
    public Tilemap Floortypes;


    private AudioSource stepPlayer;
    private string currentFloortype;

    private void Awake()
    {
        stepPlayer = gameObject.AddComponent<AudioSource>();
    }

    public void DoStep()
    {
        if (Floortypes != null)
        {
            currentFloortype = Floortypes.GetTile(new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0)).name;
            if (currentFloortype.Equals("Floortypes_Stone"))
            {
                stepPlayer.volume = Stepsounds[0].volume * (1f + UnityEngine.Random.Range(-Stepsounds[0].volumeVariance / 2f, Stepsounds[0].volumeVariance / 2f));
                stepPlayer.pitch = Stepsounds[0].pitch * (1f + UnityEngine.Random.Range(-Stepsounds[0].pitchVariance / 2f, Stepsounds[0].pitchVariance / 2f));
                stepPlayer.PlayOneShot(Stepsounds[0].clip);
            }
            if (currentFloortype.Equals("Floortypes_Wood"))
            {
                stepPlayer.volume = Stepsounds[1].volume * (1f + UnityEngine.Random.Range(-Stepsounds[1].volumeVariance / 2f, Stepsounds[1].volumeVariance / 2f));
                stepPlayer.pitch = Stepsounds[1].pitch * (1f + UnityEngine.Random.Range(-Stepsounds[1].pitchVariance / 2f, Stepsounds[1].pitchVariance / 2f));
                stepPlayer.PlayOneShot(Stepsounds[1].clip);
            }
            if (currentFloortype.Equals("Floortypes_Sand"))
            {
                stepPlayer.volume = Stepsounds[2].volume * (1f + UnityEngine.Random.Range(-Stepsounds[2].volumeVariance / 2f, Stepsounds[2].volumeVariance / 2f));
                stepPlayer.pitch = Stepsounds[2].pitch * (1f + UnityEngine.Random.Range(-Stepsounds[2].pitchVariance / 2f, Stepsounds[2].pitchVariance / 2f));
                stepPlayer.PlayOneShot(Stepsounds[2].clip);
            }
            if (currentFloortype.Equals("Floortypes_Water"))
            {
                stepPlayer.volume = Stepsounds[3].volume * (1f + UnityEngine.Random.Range(-Stepsounds[3].volumeVariance / 2f, Stepsounds[3].volumeVariance / 2f));
                stepPlayer.pitch = Stepsounds[3].pitch * (1f + UnityEngine.Random.Range(-Stepsounds[3].pitchVariance / 2f, Stepsounds[3].pitchVariance / 2f));
                stepPlayer.PlayOneShot(Stepsounds[3].clip);
            }
        }
    }
}
