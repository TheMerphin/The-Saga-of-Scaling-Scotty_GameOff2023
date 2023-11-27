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
    private int stepIndex = -1;

    private void Awake()
    {
        stepPlayer = gameObject.AddComponent<AudioSource>();
    }

    public void DoStep()
    {
        if (Floortypes == null)
        {
            Floortypes = GameObject.Find("FloorType").GetComponent<Tilemap>();
        }

        if (Floortypes != null)
        {
            currentFloortype = Floortypes.GetTile(new Vector3Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), 0)).name;
            if (currentFloortype.Equals("Floortypes_Stone")) { stepIndex = 0; }
            if (currentFloortype.Equals("Floortypes_Wood")) { stepIndex = 1; }
            if (currentFloortype.Equals("Floortypes_Sand")) { stepIndex = 2; }
            if (currentFloortype.Equals("Floortypes_Water")) { stepIndex = 3; }
            stepPlayer.volume = Stepsounds[stepIndex].volume * (1f + Random.Range(-Stepsounds[stepIndex].volumeVariance / 2f, Stepsounds[stepIndex].volumeVariance / 2f));
            stepPlayer.pitch = Stepsounds[stepIndex].pitch * (1f + Random.Range(-Stepsounds[stepIndex].pitchVariance / 2f, Stepsounds[stepIndex].pitchVariance / 2f));
            stepPlayer.PlayOneShot(Stepsounds[stepIndex].clip);
        }
    }
}
