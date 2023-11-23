using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Stage 1")]
    [SerializeField]
    private DoorController door1;

    [Header("Stage 2")]
    [SerializeField]
    private EnemyController enemy1;

    private void Awake()
    {
        stageConditions = new Func<bool>[]
        {
            () => player.transform.position.y >= 6f,
            () => !door1.enabled,
            () => enemy1.health <= 0
        };

        stageActions = new Action[]
{
            () => { stageExitCollider.enabled = true; },
            () => { },
            () => { }
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
        tutorialPrompts[tutorialStage].transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.4f);
    }

    private void LateUpdate()
    {
        ProceedToNextStage();
    }

    void ProceedToNextStage()
    {
        if (stageConditions[tutorialStage]())
        {
            tutorialPrompts[tutorialStage].enabled = false;
            stageActions[tutorialStage].Invoke();
            tutorialStage++;
            tutorialPrompts[tutorialStage].enabled = true;
        }
    }
}
