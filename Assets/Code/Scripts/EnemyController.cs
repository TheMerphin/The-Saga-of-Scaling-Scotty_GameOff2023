using UnityEngine;
using Pathfinding;
using System;

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
        BlueSmallSlime,
        Minotaur
    }

    public RuntimeAnimatorController enemyAnimator;
    public Sprite idleSprite;

    //spawnable Objects
    public GameObject objectToSpawn;
    public GameObject keyToSpawn;
    public bool dropsKey;


    private AIPath aiPath;
    private Transform target;
    private float horizontalMovement, verticalMovement;
    private Animator animator;
    private bool gotHit = false;
    private bool attack = false;
    private bool dead = false;
    private MonsterSounds monsterSounds;

    private ParticleSystem movementParticles;
    private ParticleSystem damageParticles;
    private ParticleSystem deathParticles;

    // Settings for the monster
    public MonsterType monsterType;
    public float movementSpeed = 1;
    public float detectionRange = 3f;
    public float health = -1;
    public float damage = -1;
    private PlayerController playerController;





    void Awake()
    {

        // Connect to AudioSource
        monsterSounds = gameObject.GetComponentInChildren<MonsterSounds>();
        monsterSounds.setAudioSource(gameObject.GetComponentInChildren<AudioSource>());
  
        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = movementSpeed;
        
        // Set Target
        GameObject player = GameObject.FindWithTag("Player");
        target = player.transform;
        playerController = player.GetComponent<PlayerController>();
        CapsuleCollider2D capsuleCollider2D = this.GetComponent<CapsuleCollider2D>();

        animator = GetComponentInChildren<Animator>();
        SpriteRenderer monsterSprite = GetComponentInChildren<SpriteRenderer>();


        // Particle System
        damageParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
        movementParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
        deathParticles = transform.GetChild(2).GetComponent<ParticleSystem>();



        animator.runtimeAnimatorController = enemyAnimator;
        monsterSprite.sprite = idleSprite;

        aiPath.maxSpeed = movementSpeed;

    }

    void Update()
    {
        ResetValuesBeforeFrame();
        EnemyAnimation();

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
            movementParticles.Play();
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

    public void hitPLayer()
    {
       playerController.updateHealth((int) -damage);
    }

    public void getAttacked(float damage)
    {
        damageParticles.Play();
        health = health - damage;
        if (health <= 0f && !dead) 
        {
            dead = true;
            aiPath.canMove = false;
            aiPath.enabled = false;
            animator.SetBool("dead", true);
            deathParticles.Play();

            if (dropsKey)
            {
                Instantiate(keyToSpawn, aiPath.position, Quaternion.identity);
            }

            if (monsterType.Equals(MonsterType.MotherSlime))
            {
                GameObject keyDroppingSlime = Instantiate(objectToSpawn, aiPath.position, Quaternion.identity);
                EnemyController keySlimeController = keyDroppingSlime.GetComponent<EnemyController>();
                keySlimeController.dropsKey = true;
                for (int i = 0; i < 4; i++)
                {
                    Instantiate(objectToSpawn, aiPath.position, Quaternion.identity);
                }
            }
            if (monsterType.Equals(MonsterType.BlueBigSlime))
            {
                for (int i = 0; i < 3; i++)
                {
                    Instantiate(objectToSpawn, aiPath.position, Quaternion.identity);
                }
            }


            
        }
        else if(health >0)
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

        movementParticles.Pause();
        animator.SetBool("moving", false);

        if (!attack)
        {
            animator.SetBool("attack", false);
        }
    }
}
