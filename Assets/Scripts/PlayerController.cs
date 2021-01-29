using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 7;
    public float hMovementSpeed = 7;
    public float HurtForce = 3;
    public Transform feet;
    private enum State { idle, running, jumping, falling, hurt }
    private State currentstate = State.running;
    private Collider2D BoxCollider2D;
    Rigidbody2D playerRB;
    Animator playerAnimator;
    [SerializeField] private LayerMask GroundMask;


    void Start()
    {
        BoxCollider2D = GetComponent<Collider2D>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (currentstate != State.hurt)
            MovementController();
        StateController();
        playerAnimator.SetInteger("state", (int)currentstate);
        CheckSlope();
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

    void CheckSlope()
    {
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.D))
            return;

        RaycastHit2D hit = Physics2D.Raycast(feet.position, playerRB.velocity.normalized, 0.4f, GroundMask);
        Debug.DrawRay(feet.position, playerRB.velocity.normalized, Color.blue, 1f);

        if (hit && hit.normal != Vector2.right && hit.normal != Vector2.up && hit.normal != Vector2.left)
        {
            Debug.DrawRay(hit.point, -Vector2.Perpendicular(hit.normal), Color.red, 1f);
            playerRB.velocity = -Vector2.Perpendicular(hit.normal) * hMovementSpeed;
        }
    }
}
