using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 7;
    public float hMovementSpeed = 7;
    public float HurtForce = 3;
    private enum State { idle, running, jumping, falling, hurt }
    private State currentstate = State.running;
    private BoxCollider2D BoxCollider2D;
    Rigidbody2D playerRB;
    Animator playerAnimator;
    [SerializeField] private LayerMask GroundMask;


    void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentstate != State.hurt)
            MovementController();
        StateController();
        playerAnimator.SetInteger("state", (int)currentstate);
    }

    private void MovementController()
    {
        // If the player presses A or D, snaps to the direction instead of growing to the specified direction
        // then shrinking down back to zero. Allows snappier movement
        float Hdirection = 0;
        if (Input.GetKey(KeyCode.D))
            Hdirection = 1;
        else if (Input.GetKey(KeyCode.A))
            Hdirection = -1;

        if (Hdirection > 0)
        {
            playerRB.GetComponent<Transform>().localScale = new Vector2(1, 1);
            playerRB.velocity = new Vector2(hMovementSpeed, playerRB.velocity.y);
        }
        else if (Hdirection < 0)
        {
            playerRB.GetComponent<Transform>().localScale = new Vector2(-1, 1);
            playerRB.velocity = new Vector2(-hMovementSpeed, playerRB.velocity.y);
        }

        Jump();
    }

    private void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && BoxCollider2D.IsTouchingLayers(GroundMask))
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            currentstate = State.jumping;
        }

        // If the player releases the space button while jumping, freeze vertical velocity.
        if (!Input.GetKey(KeyCode.Space) && playerRB.velocity.y > 0)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, 0);
        }
    }

    private void StateController()
    {
        if (currentstate == State.jumping)
        {
            if (playerRB.velocity.y < 0.1f)
            {
                currentstate = State.falling;
            }
        }
        else if (currentstate == State.falling)
        {
            if (BoxCollider2D.IsTouchingLayers(GroundMask))
            {
                currentstate = State.idle;
            }
        }
        else if (currentstate == State.hurt)
        {
            if (Mathf.Abs(playerRB.velocity.x) < 0.1f)
            {
                currentstate = State.idle;
            }
        }
        else if (Mathf.Abs(playerRB.velocity.x) > 2.8f)
        {
            currentstate = State.running;
        }
        else
            currentstate = State.idle;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (currentstate == State.falling)
            {
                enemy.DeathAnimation();
                Jump();
            }
            else
            {
                currentstate = State.hurt;
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    playerRB.velocity = new Vector2(-HurtForce, playerRB.velocity.x);
                }
                else
                {
                    playerRB.velocity = new Vector2(HurtForce, playerRB.velocity.x);
                }
            }
        }
    }
}
