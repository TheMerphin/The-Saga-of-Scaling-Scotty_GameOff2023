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
    private bool gotHit = false;
    private bool attack = false;
    private MonsterSounds monsterSounds;

    public float soundVolume = 1f;


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

        monsterSounds.changeVolume(soundVolume);

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


        // Test Method:
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            getAttacked(0);
        }
        */
    }



    public void changeSoundVolume(float volume)
    {
        soundVolume = volume; 
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

            animator.SetBool("idle", true);
            aiPath.canMove = false;
            aiPath.enabled = false;
            return;
        }
        //in attack range
        else if (distanceToPlayer <= attackRange)
        {
            animator.SetBool("attack", true);
        }
        // wants to move towards the player
        else
        {
            animator.SetBool("idle", false);
            aiPath.canMove = true;
            aiPath.enabled = true;
            
             animator.SetBool("moving", true);
        }



        // which direction do i move?
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

    
    public void getAttacked(float damage)
    {
        gotHit = true;
        if(monsterType.Equals(MonsterType.Skeleton))
        {
            monsterSounds.playSkeletonHurt();
        }
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
