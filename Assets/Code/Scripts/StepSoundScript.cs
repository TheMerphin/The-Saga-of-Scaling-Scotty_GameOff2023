using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public void doStep()
    {
        if (Floortypes != null)
        {
            currentFloortype = Floortypes.GetTile(new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0)).name;
            if (currentFloortype.Equals("Floortypes_Stone"))
            {
                stepPlayer.PlayOneShot(Stepsounds[0].clip);
            }
            if (currentFloortype.Equals("Floortypes_Wood"))
            {
                stepPlayer.PlayOneShot(Stepsounds[1].clip);
            }
            if (currentFloortype.Equals("Floortypes_Sand"))
            {
                stepPlayer.PlayOneShot(Stepsounds[2].clip);
            }
            if (currentFloortype.Equals("Floortypes_Water"))
            {
                stepPlayer.PlayOneShot(Stepsounds[3].clip);
            }
        }
    }
}
