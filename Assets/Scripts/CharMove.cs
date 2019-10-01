﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    public Animator animator;
    public int playerSpeed = 8;
    public int playerJumpPower = 1700;
    public float sprintMult = 2;
    public float totalSpeed;
    private float moveX;
    private float sprintX;
    private bool facingRight = true;
    public bool isGrounded;

    void Update()
    {
        PlayerMove();
        animator.SetFloat("Speed", Mathf.Abs(moveX));
    }
    void PlayerMove()
    {
        //CONTROLS
        moveX = Input.GetAxis("Horizontal");
        sprintX = Input.GetAxis("Sprint");
        if (Input.GetButtonDown("Jump") && isGrounded) {
            Jump();
            animator.SetBool("IsJumping", true);
        }
        //ANIMATIONS
        //PLAYER DIRECTION
        if (moveX > 0.0f && !facingRight)
        {
            FlipPlayer();
        }
        else if (moveX < 0.0f && facingRight)
        {
            FlipPlayer();
        }
        //PHYSICS
        if (Mathf.Abs(moveX) > 0.0f) {
            totalSpeed = playerSpeed;
            if (sprintX > 0.0f)
            {
                totalSpeed = playerSpeed * sprintMult;
                animator.SetBool("IsSprinting", true);
            }
            else {
                animator.SetBool("IsSprinting", false);
            }
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * totalSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }

    void Jump()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * playerJumpPower);
        isGrounded = false;
    }

    void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "ground") {
            isGrounded = true;
            animator.SetBool("IsJumping", false);
        }
    }
}
