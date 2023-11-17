using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StepSoundScript : MonoBehaviour
{
    [Tooltip("order: stone,wood,sand,water.")]
    public Sound[] stepsounds;
    [Tooltip("please attach the tilemap \"Floortype\" here.")]
    public Tilemap floortypes;

    private AudioSource stepplayer;
    private string currentFloortype;

    private void Awake()
    {
        stepplayer = gameObject.AddComponent<AudioSource>();
    }

    public void doStep()
    {
       if(floortypes != null)
        {
            currentFloortype = floortypes.GetTile(new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0)).name;
            if (currentFloortype == "Floortypes_Stone")
            {
                Sound currentStep = stepsounds[0];
                stepplayer.PlayOneShot(currentStep.clip);
                //print(currentFloortype); //Debug
            }
            if (currentFloortype == "Floortypes_Wood")
            {
                Sound currentStep = stepsounds[1];
                stepplayer.PlayOneShot(currentStep.clip);
                //print(currentFloortype); //Debug
            }
            if (currentFloortype == "Floortypes_Sand")
            {
                Sound currentStep = stepsounds[2];
                stepplayer.PlayOneShot(currentStep.clip);
                //print(currentFloortype); //Debug
            }
            if (currentFloortype == "Floortypes_Water")
            {
                Sound currentStep = stepsounds[3];
                stepplayer.PlayOneShot(currentStep.clip);
                //print(currentFloortype); //Debug
            }
        }
    }
}
