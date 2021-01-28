using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleController : Enemy
{
    private float leftcap;
    private float rightcap;
    private float upcap;
    private float downcap;
    private float speed = 3f;
    private bool facingdirection = true;
    private bool floatingdircetion = true;

    void Start()
    {
        leftcap = transform.position.x - 4;
        rightcap = transform.position.x + 4;
        upcap = transform.position.y + 2;
        downcap = transform.position.y - 2;
    }

    void Update()
    {
        GhostMoveController();
    }

    private void GhostMoveController()
    {
        if (facingdirection)
        {
            if (transform.position.x > leftcap)
            {
                if (transform.localScale.x != 1)
                    transform.localScale = new Vector3(1, 1);

                if (floatingdircetion)
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, speed);
                else
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, -speed);
            }
            else
            {
                facingdirection = false;
            }
        }
        else
        {
            if (transform.position.x < rightcap)
            {
                if (transform.localScale.x != -1)
                    transform.localScale = new Vector3(-1, 1);

                if (floatingdircetion)
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, speed);
                else
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, -speed);


            }
            else
            {
                facingdirection = true;
            }
        }

        if (floatingdircetion)
        {
            if (transform.position.y > upcap)
            {
                floatingdircetion = false;
            }
        }
        else
        {
            if (transform.position.y < downcap)
            {
                floatingdircetion = true;
            }
        }
    }
}
