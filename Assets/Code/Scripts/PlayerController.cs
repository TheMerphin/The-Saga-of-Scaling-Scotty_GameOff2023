using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1f;
    private Rigidbody2D rb;
    private Animator animator;

    private float horizontalMovement, verticalMovement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ResetValuesBeforeFrame();
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (horizontalMovement > 0.01f || horizontalMovement < -0.01f)
        {
            rb.AddForce(new Vector2(horizontalMovement * movementSpeed, 0f), ForceMode2D.Impulse);
        }

        if (verticalMovement > 0.01f || verticalMovement < -0.01f)
        {
            rb.AddForce(new Vector2(0f, verticalMovement * movementSpeed), ForceMode2D.Impulse);
        }
    }

    private void HandleInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        if (horizontalMovement < -0.01f && verticalMovement > 0.01f)
        {
            animator.SetBool("movingTL", true);
        }
        else if ((horizontalMovement < -0.01f && verticalMovement == 0f) | (horizontalMovement < -0.01f && verticalMovement < -0.01f))
        {
            animator.SetBool("movingBL", true);
        }
        else if ((verticalMovement > 0.01f && horizontalMovement == 0f) | (horizontalMovement > 0.01f && verticalMovement > 0.01f))
        {
            animator.SetBool("movingTR", true);
        }
        else if ((horizontalMovement > 0.01f && verticalMovement == 0f) | (verticalMovement < -0.01f && horizontalMovement == 0f) | (horizontalMovement > 0.01f && verticalMovement < -0.01f))
        {
            animator.SetBool("movingBR", true);
        }
    }

    private void ResetValuesBeforeFrame()
    {
        animator.SetBool("movingTR", false);
        animator.SetBool("movingTL", false);
        animator.SetBool("movingBR", false);
        animator.SetBool("movingBL", false);
    }
}
