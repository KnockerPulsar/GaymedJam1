using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Player;
    float PushingForce = 7;
    float SideForce = 7;
    float HurtForce = 3;
    private enum State { idle, running, jumping, falling, hurt }
    private State currentstate = State.running;
    private BoxCollider2D BoxCollider2D;
    [SerializeField] private LayerMask GroundMask;


    void Start()
    {
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if(currentstate!=State.hurt)
            MovementController();
        StateController();
        Player.GetComponent<Animator>().SetInteger("state", (int)currentstate);
        //Debug.Log(currentstate);
    }

    private void MovementController()
    {
        float Hdirection = Input.GetAxis("Horizontal");

        if (Hdirection > 0)
        {
            Player.GetComponent<Transform>().localScale = new Vector2(1, 1);
            Player.velocity = new Vector2(SideForce, Player.velocity.y);
        }
        else if (Hdirection < 0)
        {
            Player.GetComponent<Transform>().localScale = new Vector2(-1, 1);
            Player.velocity = new Vector2(-SideForce, Player.velocity.y);
        }

        if (Input.GetKeyDown("w") && BoxCollider2D.IsTouchingLayers(GroundMask))
        {
            Jump();
        }
    }

    private void Jump()
    {
        Player.velocity = new Vector2(Player.velocity.x, PushingForce);
        currentstate = State.jumping;
    }

    private void StateController()
    {
        if (currentstate == State.jumping)
        {
            if (Player.velocity.y < 0.1f)
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
            if (Mathf.Abs(Player.velocity.x)<0.1f)
            {
                currentstate = State.idle;
            }
        }
        else if (Mathf.Abs(Player.velocity.x) > 2.8f)
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
                    Player.velocity = new Vector2(-HurtForce, Player.velocity.x);
                }
                else
                {
                    Player.velocity = new Vector2(HurtForce, Player.velocity.x);
                }
            }
        }
    }
}
