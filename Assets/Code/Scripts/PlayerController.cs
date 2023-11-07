using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Toolbox;

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
        HandleMovement();
        HandleActions();
    }

    private void HandleMovement()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        Vector2 playerPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mouseDirection = (mousePos - playerPos).normalized;

        switch (GetDiagonalDirection(mouseDirection))
        {
            case DiagonalDirection.UpRight:
                animator.SetBool("isFacingTR", true);
                break;
            case DiagonalDirection.UpLeft:
                animator.SetBool("isFacingTL", true);
                break;
            case DiagonalDirection.DownRight:
                animator.SetBool("isFacingBR", true);
                break;
            case DiagonalDirection.DownLeft:
                animator.SetBool("isFacingBL", true);
                break;
        }

        if (horizontalMovement != 0f | verticalMovement != 0f)
        {
            animator.SetBool("isMoving", true);
        }
    }

    private void HandleActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    private void ResetValuesBeforeFrame()
    {
        animator.SetBool("isFacingTR", false);
        animator.SetBool("isFacingTL", false);
        animator.SetBool("isFacingBR", false);
        animator.SetBool("isFacingBL", false);
        animator.SetBool("isMoving", false);
    }
}
