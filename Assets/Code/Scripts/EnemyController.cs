using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyController : MonoBehaviour
{
    public enum MonsterType
    {
        Skeleton,
        Goblin,
        Slime
    }

    private AIPath aiPath;
    private Transform target;
    private float horizontalMovement, verticalMovement;
    private Animator animator;
    private bool attack = false;

    // Settings for the monster
    public MonsterType monsterType;
    public float movementSpeed = 1;
    public float detectionRange = 3f;

    public RuntimeAnimatorController skeletonAnimator;
    public Sprite skeletonSprite;
    public RuntimeAnimatorController goblinAnimator;
    public Sprite goblinSprite;
    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = movementSpeed;

        GameObject player = GameObject.FindWithTag("Player");
        target = player.transform;

        animator = GetComponentInChildren<Animator>();
        SpriteRenderer monsterSprite = GetComponentInChildren<SpriteRenderer>();

        if (monsterType.Equals(MonsterType.Skeleton))
        {
            animator.runtimeAnimatorController = skeletonAnimator;
            monsterSprite.sprite = skeletonSprite;
        }
        else if (monsterType.Equals(MonsterType.Goblin))
        {
            Debug.Log("Sneaky Gobbos");
        }
    }

    void Update()
    {
        ResetValuesBeforeFrame();
        EnemyAnimation();
    }

    private void EnemyAnimation()
    {
        
        horizontalMovement = aiPath.desiredVelocity.x;
        verticalMovement = aiPath.desiredVelocity.y;



        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer >= 5f)
        {
            animator.SetBool("laying", true);
            aiPath.canMove = false;
            return;
        }
        else
        {
            
            animator.SetBool("laying", false);
            aiPath.canMove = true;
        }
        if (horizontalMovement < 0f && verticalMovement > 0f)
        {
            if (distanceToPlayer <= detectionRange)
            {
                animator.SetBool("attackTL", true);
                attack = true;
            }
            else
            {
                animator.SetBool("movingTL", true);
                attack = false;
            }

        }
        else if ((horizontalMovement < 0f && verticalMovement == 0f) | (horizontalMovement < 0f && verticalMovement < 0f))
        {
            if (distanceToPlayer <= detectionRange)
            {
                animator.SetBool("attackBL", true);
                attack = true;
            }
            else
            {
                animator.SetBool("movingBL", true);
                attack = false;
            }

        }
        else if ((verticalMovement > 0f && horizontalMovement == 0f) | (horizontalMovement > 0f && verticalMovement > 0f))
        {
            if (distanceToPlayer <= detectionRange)
            {
                animator.SetBool("attackTR", true);
                attack = true;
            }
            else
            {
                animator.SetBool("movingTR", true);
                attack = false;
            }

        }
        else if ((horizontalMovement > 0f && verticalMovement == 0f) | (verticalMovement < 0f && horizontalMovement == 0f) | (horizontalMovement > 0f && verticalMovement < 0f))
        {
            if (distanceToPlayer <= detectionRange)
            {
                animator.SetBool("attackBR", true);
                attack = true;
            }
            else
            {
                animator.SetBool("movingBR", true);
                attack = false;
            }

        }
    }
    public void getAttacked(float damage)
    {
        Debug.Log("I got attacked");
    }


    private void ResetValuesBeforeFrame()
    {
        animator.SetBool("movingTR", false);
        animator.SetBool("movingTL", false);
        animator.SetBool("movingBR", false);
        animator.SetBool("movingBL", false);

        if (!attack)
        {
            animator.SetBool("attackTR", false);
            animator.SetBool("attackTL", false);
            animator.SetBool("attackBR", false);
            animator.SetBool("attackBL", false);
        }
    }
}
