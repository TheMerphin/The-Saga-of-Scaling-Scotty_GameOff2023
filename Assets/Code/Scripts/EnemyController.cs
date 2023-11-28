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


    //new monster
    public GameObject objectToSpawn;
    public GameObject keyToSpawn;
    public Boolean dropsKey;


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
    public RuntimeAnimatorController minotaurAnimator;
    public Sprite minotaurSprite;
    void Awake()
    {


        monsterSounds = gameObject.GetComponentInChildren<MonsterSounds>();
        monsterSounds.setAudioSource(gameObject.GetComponentInChildren<AudioSource>());
  
        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = movementSpeed;
        
        GameObject player = GameObject.FindWithTag("Player");
        target = player.transform;
        playerController = player.GetComponent<PlayerController>();
        CapsuleCollider2D capsuleCollider2D = this.GetComponent<CapsuleCollider2D>();

        animator = GetComponentInChildren<Animator>();
        SpriteRenderer monsterSprite = GetComponentInChildren<SpriteRenderer>();

        damageParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
        movementParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
        deathParticles = transform.GetChild(2).GetComponent<ParticleSystem>();

        switch (monsterType) {
            case MonsterType.Skeleton:
                animator.runtimeAnimatorController = skeletonAnimator;
                monsterSprite.sprite = skeletonSprite;
                capsuleCollider2D.offset = new Vector2(0.06f, -0.07f);
                if (health < 0) health = 8;
                if (damage < 0) damage = 1;
                break;

            case MonsterType.Wolf:
                animator.runtimeAnimatorController = wolfAnimator;
                monsterSprite.sprite = wolfSprite;
                capsuleCollider2D.offset = new Vector2(0.06f, -0.07f);
                movementSpeed = 2f;
                if (health < 0) health = 6;
                if (damage < 0) damage = 1;
                break;

            case MonsterType.Troll:
                animator.runtimeAnimatorController = trollAnimator;
                monsterSprite.sprite = trollSprite;
                capsuleCollider2D.offset = new Vector2(0.06f, -0.07f);
                movementSpeed = 0.5f;
                if (health < 0) health = 24;
                if (damage < 0) damage = 3;
                break;

            case MonsterType.MotherSlime:
                animator.runtimeAnimatorController = blueMotherSlimeAnimator;
                monsterSprite.sprite = blueMotherSlimeSprite;
                if (health < 0) health = 80;
                if (damage < 0) damage = 3; 
                break;

            case MonsterType.BlueBigSlime:
                animator.runtimeAnimatorController = blueBigSlimeAnimator;
                monsterSprite.sprite = blueBigSlimeSprite;
                if (health < 0) health = 8;
                if (damage < 0) damage = 1;
                break;

            case MonsterType.BlueSmallSlime:
                animator.runtimeAnimatorController = blueSmallSlimeAnimator;
                monsterSprite.sprite = blueSmallSlimeSprite;
                movementSpeed = 3f;
                if (health < 0) health = 1;
                if (damage < 0) damage = 1;
                break;

            case MonsterType.Minotaur:
                animator.runtimeAnimatorController = minotaurAnimator;
                monsterSprite.sprite = minotaurSprite;
                movementSpeed = 0.8f;
                capsuleCollider2D.offset = new Vector2(0.06f, -0.07f);
                if (health < 0) health = 20;
                if (damage < 0) damage = 5;
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
