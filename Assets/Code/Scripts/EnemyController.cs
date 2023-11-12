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
    private bool gotHit = false;

    // Settings for the monster
    public MonsterType monsterType;
    public float movementSpeed = 1;
    public float detectionRange = 3f;
    public float health;


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

        if (Input.GetKeyDown(KeyCode.F))
        {
            getAttacked(0);
        }

    }

    private void EnemyAnimation()
    {
        
        horizontalMovement = aiPath.velocity.x;
        verticalMovement = aiPath.velocity.y;
        float attackRange = 1f;

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer >= detectionRange)
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
            animator.SetBool("TL", true);
            if (gotHit)
            {
                animator.SetTrigger("getsHit");
                gotHit = false;

            }

            if (distanceToPlayer <= attackRange)
            {
                
                animator.SetBool("attack", true);
                attack = true;
            }
            else
            {
                animator.SetBool("moving", true);
                attack = false;
            }

        }
        else if ((horizontalMovement < 0f && verticalMovement == 0f) | (horizontalMovement < 0f && verticalMovement < 0f))
        {
            animator.SetBool("BL", true);
            if (gotHit)
            {
                animator.SetTrigger("getsHit");
                gotHit = false;

            }

            if (distanceToPlayer <= attackRange)
            {
                animator.SetBool("attack", true);
                attack = true;
            }
            else
            {
                animator.SetBool("moving", true);
                attack = false;
            }

        }
        else if ((verticalMovement > 0f && horizontalMovement == 0f) | (horizontalMovement > 0f && verticalMovement > 0f))
        {
            animator.SetBool("TR", true);
            if (gotHit)
            {
                animator.SetTrigger("getsHit");
                gotHit = false;

            }

            if (distanceToPlayer <= attackRange)
            {
                animator.SetBool("attack", true);
                attack = true;
            }
            else
            {
                animator.SetBool("moving", true);
                attack = false;
            }

        }
        else if ((horizontalMovement > 0f && verticalMovement == 0f) | (verticalMovement < 0f && horizontalMovement == 0f) | (horizontalMovement > 0f && verticalMovement < 0f))
        {

            animator.SetBool("BR", true);
            if (gotHit)
            {
                animator.SetTrigger("getsHit");
                gotHit = false;
            }

            if (distanceToPlayer <= attackRange)
            {
                animator.SetBool("attack", true);
                attack = true;
            }
            else
            {
                animator.SetBool("moving", true);
                attack = false;
            }

        }
    }
    public void getAttacked(float damage)
    {
        gotHit = true;
        Debug.Log("I got attacked");
    }


    private void ResetValuesBeforeFrame()
    {
        animator.SetBool("TR", false);
        animator.SetBool("TL", false);
        animator.SetBool("BR", false);
        animator.SetBool("BL", false);

        
        animator.SetBool("moving", false);

        if (!attack)
        {
            animator.SetBool("attack", false);
        }
    }
}
