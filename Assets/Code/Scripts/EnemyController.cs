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
    private bool idle = true;
    private bool moving = false;
    private MonsterSounds monsterSounds;


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
        monsterSounds = gameObject.GetComponentInChildren<MonsterSounds>();
        monsterSounds.setAudioSource(gameObject.GetComponentInChildren<AudioSource>());

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

        if (gotHit)
        {
            animator.SetTrigger("getsHit");
            gotHit = false;

        }

        //out of player range
        if (distanceToPlayer >= detectionRange)
        {
            if (!idle)
            {
                idleSound();
                idle = true;
            }
            moving = false;
            animator.SetBool("idle", true);
            aiPath.canMove = false;
            return;
        }
        //in attack range
        else if (distanceToPlayer <= attackRange)
        {
            animator.SetBool("attack", true);
            moving = false;
            if (!attack)
            {
                attackSound();
                attack = true;
            }
            

        }
        // wants to move towards the player
        else
        {
            idle = false;
            animator.SetBool("idle", false);
            aiPath.canMove = true;
            if (!moving)
            {
                movingSound();
                moving = true;
            }
             animator.SetBool("moving", true);
             attack = false;
        }




        if (horizontalMovement < 0f && verticalMovement > 0f)
        {
            animator.SetBool("TL", true);
        }
        else if ((horizontalMovement < 0f && verticalMovement == 0f) | (horizontalMovement < 0f && verticalMovement < 0f))
        {
            animator.SetBool("BL", true);
          

        }
        else if ((verticalMovement > 0f && horizontalMovement == 0f) | (horizontalMovement > 0f && verticalMovement > 0f))
        {
            animator.SetBool("TR", true);
            

        }
        else if ((horizontalMovement > 0f && verticalMovement == 0f) | (verticalMovement < 0f && horizontalMovement == 0f) | (horizontalMovement > 0f && verticalMovement < 0f))
        {

            animator.SetBool("BR", true);

        }
    }

    private void EnemyAnimationOLD()
    {
        
        horizontalMovement = aiPath.velocity.x;
        verticalMovement = aiPath.velocity.y;
        float attackRange = 1f;

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer >= detectionRange)
        {
            if (!idle)
            {
                idleSound();
                idle = true;
            }
            moving = false;
            animator.SetBool("idle", true);
            aiPath.canMove = false;
            return;
        }
        else
        {
            idle = false;
            animator.SetBool("idle", false);
            aiPath.canMove = true;
        }
        if (horizontalMovement < 0f && verticalMovement > 0f)
        {
            animator.SetBool("TL", true);
            if (gotHit)
            {
                animator.SetTrigger("getsHit");
                gotHit = false;

            }else  if (distanceToPlayer <= attackRange)
            { 
                if (!attack)
                {
                    attackSound();
                    attack = true;
                }

            animator.SetBool("attack", true);
              
            }
            else
            {
                if (!moving)
                {
                    movingSound();
                    moving = true;
                }
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

            }else  if (distanceToPlayer <= attackRange)
            {
                if (!attack)
                {
                    attackSound();
                    attack = true;
                }
                moving = false;
                animator.SetBool("attack", true);
            }
            else
            {
                if (!moving)
                {
                    Debug.Log("I start moving");
                    movingSound();
                    moving = true;
                }
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

            }else   if (distanceToPlayer <= attackRange)
            {
                if (!attack)
                {
                    attackSound();
                   
                    attack = true;
                }

                animator.SetBool("attack", true);
            }
            else
            {
                animator.SetBool("moving", true);
                attack = false;
                moving = false;
            }

        }
        else if ((horizontalMovement > 0f && verticalMovement == 0f) | (verticalMovement < 0f && horizontalMovement == 0f) | (horizontalMovement > 0f && verticalMovement < 0f))
        {

            animator.SetBool("BR", true);
            if (gotHit)
            {
                animator.SetTrigger("getsHit");
                gotHit = false;
            }else  if (distanceToPlayer <= attackRange)
            {

                if (!attack)
                {
                    attackSound();
                    attack = true;
                }
                animator.SetBool("attack", true);
            }
            else
            {
                animator.SetBool("moving", true);
                attack = false;
                moving = false;
            }

        }
    }
    public void getAttacked(float damage)
    {
        gotHit = true;
        Debug.Log("I got attacked");
        if(monsterType.Equals(MonsterType.Skeleton))
        {
            monsterSounds.playSkeletonHurt();
        }
    }

    public void attackSound()
    {
      /*  if(monsterType.Equals(MonsterType.Skeleton))
        {
            monsterSounds.playSkeletonAttack();
        }else if(monsterType.Equals(MonsterType.Goblin))
        {

        }*/
    }
    public void deathSound()
    {
        /*
        if (monsterType.Equals(MonsterType.Skeleton))
        {
            monsterSounds.playSkeletonDeath();
        }*/
       
    }

    public void idleSound()
    {/*
        if (monsterType.Equals(MonsterType.Skeleton))
        {
            monsterSounds.playSkeletonDeath();
        }*/
    }

    public void movingSound()
    {/*
        if (monsterType.Equals(MonsterType.Skeleton))
        {
            monsterSounds.playSkeletonStep();
        }*/
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
