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
        Slime,
        Wolf,
        Troll
    }

    private AIPath aiPath;
    private Transform target;
    private float horizontalMovement, verticalMovement;
    private Animator animator;
    private bool gotHit = false;
    private bool attack = false;
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
    public RuntimeAnimatorController wolfAnimator;
    public Sprite wolfSprite;
    public RuntimeAnimatorController trollAnimator;
    public Sprite trollSprite;
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

        switch (monsterType) {
            case MonsterType.Skeleton:
                animator.runtimeAnimatorController = skeletonAnimator;
                monsterSprite.sprite = skeletonSprite;
                break;

            case MonsterType.Wolf:
                animator.runtimeAnimatorController = wolfAnimator;
                monsterSprite.sprite = wolfSprite;
                //ToDO more speed, less health
                break;

            case MonsterType.Troll:
                animator.runtimeAnimatorController = trollAnimator;
                monsterSprite.sprite = trollSprite;
                //TodDo less speed, more health?
                break;

            case MonsterType.Slime:
                break;

            case MonsterType.Goblin:
                Debug.Log("Sneaky Gobbos");
                break;

            default:
                Debug.Log("No Monster");
                break;

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

        switch (monsterType)
        {
            case MonsterType.Skeleton:
                monsterSounds.playSkeletonHurt();
                break;

            case MonsterType.Wolf:
                monsterSounds.playWolfHurt();
                break;

            case MonsterType.Troll:
                monsterSounds.playTrollHurt();
                break;

            case MonsterType.Slime:
                break;

            case MonsterType.Goblin:
                Debug.Log("Sneaky Gobbos");
                break;

            default:
                Debug.Log("No Monster");
                break;

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
