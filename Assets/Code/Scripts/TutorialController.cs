using System;
using System.Collections;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public Canvas[] tutorialPrompts;

    private Func<bool>[] stageConditions;

    private Action[] stageActions;

    private PlayerController player;
    private int tutorialStage = 0;

    [Header("Stage 0")]
    [SerializeField]
    private Collider2D stageExitCollider;

    [Header("Stage 1 - Items")]
    [SerializeField]
    private LockController door1;

    [Header("Stage 2 - Enemies")]
    [SerializeField]
    private LockController door2;

    [Header("Stage 3 - Traps")]
    [SerializeField]
    private LockController door3;

    [Header("Stage 4 - Scaling")]
    [SerializeField]
    private LockController door4;
    [SerializeField]
    private GameObject pitTrapPrefab;
    private PitTrap pitTrap;

    private void Awake()
    {
        stageConditions = new Func<bool>[]
        {
            () => player.transform.position.y >= 6f,
            () => !door1.enabled,
            () => !door2.enabled,
            () => !door3.enabled,
            () => !door4.enabled,
        };

        stageActions = new Action[]
{
            () => { stageExitCollider.enabled = true; },
            () => { },
            () => { },
            () => { pitTrap = Instantiate(pitTrapPrefab, new Vector3(41.5f, 41.5f , 0f), Quaternion.identity).GetComponent<PitTrap>(); pitTrap.EntityRespawnPosition = new Vector2(34.5f, 41.5f); },
            () => { player.GetComponent<ItemManager>().ClearInventory(); },
};
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        tutorialPrompts[0].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        tutorialPrompts[tutorialStage].transform.position = new Vector2(player.transform.position.x - 3.5f, player.transform.position.y + 0.4f);

        if (tutorialStage == 4 && pitTrap != null && !pitTrap.Active)
        {
            StartCoroutine(ReplacePitTrap());
        }
    }

    private IEnumerator ReplacePitTrap()
    {
        Destroy(pitTrap.gameObject);
        pitTrap = null;
        yield return new WaitForSeconds(1);
        pitTrap = Instantiate(pitTrapPrefab, new Vector3(41.5f, 41.5f, 0f), Quaternion.identity).GetComponent<PitTrap>();
        pitTrap.EntityRespawnPosition = new Vector2(34.5f, 41.5f);
    }

    private void LateUpdate()
    {
        ProceedToNextStage();
    }

    void ProceedToNextStage()
    {
        if (tutorialStage < stageConditions.Length && stageConditions[tutorialStage]())
        {
            tutorialPrompts[tutorialStage].enabled = false;
            stageActions[tutorialStage].Invoke();
            tutorialStage++;
            tutorialPrompts[tutorialStage].enabled = true;
        }
    }
}
