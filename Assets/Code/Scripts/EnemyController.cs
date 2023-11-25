using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public enum MonsterType
    {
        Skeleton,
        Goblin,
        Wolf,
        Troll,
        MotherSlime,
        BlueBigSlime,
        BlueSmallSlime
    }

    //new monster
    public GameObject objectToSpawn;

    private AIPath aiPath;
    private Transform target;
    private float horizontalMovement, verticalMovement;
    private Animator animator;
    private bool gotHit = false;
    private bool attack = false;
    private bool dead = false;
    private MonsterSounds monsterSounds;


    // Settings for the monster
    public MonsterType monsterType;
    public float movementSpeed = 1;
    public float detectionRange = 3f;
    public float health;
    public float damage;
    private PlayerController playerController;


    public RuntimeAnimatorController skeletonAnimator;
    public Sprite skeletonSprite;
    public RuntimeAnimatorController goblinAnimator;
    public Sprite goblinSprite;
    public RuntimeAnimatorController wolfAnimator;
    public Sprite wolfSprite;
    public RuntimeAnimatorController trollAnimator;
    public Sprite trollSprite;
    public RuntimeAnimatorController blueMotherSlimeAnimator;
    public Sprite blueMotherSlimeSprite;
    public RuntimeAnimatorController blueBigSlimeAnimator;
    public Sprite blueBigSlimeSprite;
    public RuntimeAnimatorController blueSmallSlimeAnimator;
    public Sprite blueSmallSlimeSprite;
    void Awake()
    {


        monsterSounds = gameObject.GetComponentInChildren<MonsterSounds>();
        monsterSounds.setAudioSource(gameObject.GetComponentInChildren<AudioSource>());
  
        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = movementSpeed;
        
        GameObject player = GameObject.FindWithTag("Player");
        target = player.transform;
        playerController = player.GetComponent<PlayerController>();

        animator = GetComponentInChildren<Animator>();
        SpriteRenderer monsterSprite = GetComponentInChildren<SpriteRenderer>();

        switch (monsterType) {
            case MonsterType.Skeleton:
                animator.runtimeAnimatorController = skeletonAnimator;
                monsterSprite.sprite = skeletonSprite;
                health = 4;
                break;

            case MonsterType.Wolf:
                animator.runtimeAnimatorController = wolfAnimator;
                monsterSprite.sprite = wolfSprite;
                movementSpeed = 2f;
                health = 5;
                //ToDO more speed, less health
                break;

            case MonsterType.Troll:
                animator.runtimeAnimatorController = trollAnimator;
                monsterSprite.sprite = trollSprite;
                movementSpeed = 0.5f;
                health = 10; 
                //TodDo less speed, more health?, BIGGER!
                break;

            case MonsterType.MotherSlime:
                animator.runtimeAnimatorController = blueMotherSlimeAnimator;
                monsterSprite.sprite = blueMotherSlimeSprite;
                health = 15; //test 15 sonst 25?
                //movementSpeed =?; 
                break;

            case MonsterType.BlueBigSlime:
                animator.runtimeAnimatorController = blueBigSlimeAnimator;
                monsterSprite.sprite = blueBigSlimeSprite;
                health = 10;
                break;

            case MonsterType.BlueSmallSlime:
                animator.runtimeAnimatorController = blueSmallSlimeAnimator;
                monsterSprite.sprite = blueSmallSlimeSprite;
                movementSpeed = 3f;
                health = 3;
                break;

            case MonsterType.Goblin:
                Debug.Log("Sneaky Gobbos");
                break;

            default:
                Debug.Log("No Monster");
                break;

        }

        aiPath.maxSpeed = movementSpeed;

    }

    void Update()
    {
        ResetValuesBeforeFrame();
        EnemyAnimation();


        // Test Method:
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            getAttacked(1);
        }

    }




    private void EnemyAnimation()
    {
        if (dead)
        {
            return;
        }
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

    
    

    void SpawnNewMonsters()
    {
        Instantiate(objectToSpawn, aiPath.position, Quaternion.identity);
    }

    public void hitPLayer()
    {
       // playerController.updateHealth((int) -damage);
    }




    public void getAttacked(float damage)
    {
        
        health = health - damage;
        if (health <= 0f) 
        {
            dead = true;
            aiPath.canMove = false;
            aiPath.enabled = false;
            animator.SetBool("dead", true);

            if (monsterType.Equals(MonsterType.MotherSlime)) 
            {
                //könnte auch immer wieder slimes beschwören nicht erst beim tod
                SpawnNewMonsters();
                SpawnNewMonsters();
            }
            if (monsterType.Equals(MonsterType.BlueBigSlime))
            {
                //spawn two smaller slimes
                SpawnNewMonsters();
                SpawnNewMonsters();
            }
            //else if((monsterType.Equals(MonsterType.GreenBigSlime))
        }
        else
        {
            gotHit = true;
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
