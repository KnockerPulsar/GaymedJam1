using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogController : Enemy
{
    private float leftcap;
    private float rightcap;

    private bool facingleft = true;

    private float jumplength = 4;
    private float jumpheight = 4;

    [SerializeField] private LayerMask GroundMask;

    void Start()
    {
        leftcap = transform.position.x - 2;
        rightcap = transform.position.x + 2;

    }

    public void Update()
    {
        if (this.GetComponent<Animator>().GetBool("jumping"))
        {
            if (this.GetComponent<Rigidbody2D>().velocity.y < 0.1)
            {
                this.GetComponent<Animator>().SetBool("falling", true);
                this.GetComponent<Animator>().SetBool("jumping", false);

            }
        }
        else if (this.GetComponent<BoxCollider2D>().IsTouchingLayers(GroundMask) && this.GetComponent<Animator>().GetBool("falling"))
        {
            this.GetComponent<Animator>().SetBool("falling", false);
            this.GetComponent<Animator>().SetBool("jumping", false);
        }
    }

    private void FrogMovementController()
    {
        if (facingleft)
        {
            if (transform.position.x > leftcap)
            {
                if (transform.localScale.x != 1)
                    transform.localScale = new Vector3(1, 1);

                if (this.GetComponent<BoxCollider2D>().IsTouchingLayers(GroundMask))
                {
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(-jumplength, jumpheight);
                    this.GetComponent<Animator>().SetBool("jumping", true);
                }
            }
            else
            {
                facingleft = false;
            }
        }
        else
        {
            if (transform.position.x < rightcap)
            {
                if (transform.localScale.x != -1)
                    transform.localScale = new Vector3(-1, 1);

                if (this.GetComponent<BoxCollider2D>().IsTouchingLayers(GroundMask))
                {
                    this.GetComponent<Rigidbody2D>().velocity = new Vector2(jumplength, jumpheight);
                    this.GetComponent<Animator>().SetBool("jumping", true);
                }
            }
            else
            {
                facingleft = true;
            }

        }
    }



}
