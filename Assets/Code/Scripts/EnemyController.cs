using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyController : MonoBehaviour
{
    private AIPath aiPath;
    private Transform target;
    private float horizontalMovement, verticalMovement;
    private Animator animator;
    private bool attack=false;
    void Awake()
    {
        aiPath = GetComponent<AIPath>();
        GameObject player = GameObject.Find("Player");
        target = player.transform;

        animator = GetComponentInChildren<Animator>();


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

        
        float detectionRange = 0.8f;
        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        
        if (horizontalMovement < -0.01f && verticalMovement > 0.01f)
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
        else if ((horizontalMovement < -0.01f && verticalMovement == 0f) | (horizontalMovement < -0.01f && verticalMovement < -0.01f))
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
        else if ((verticalMovement > 0.01f && horizontalMovement == 0f) | (horizontalMovement > 0.01f && verticalMovement > 0.01f))
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
        else if ((horizontalMovement > 0.01f && verticalMovement == 0f) | (verticalMovement < -0.01f && horizontalMovement == 0f) | (horizontalMovement > 0.01f && verticalMovement < -0.01f))
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
