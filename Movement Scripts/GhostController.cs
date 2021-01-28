using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : Enemy
{
    private enum Dirctions { Right,Left,Up,Down}
    private float leftcap;
    private float rightcap;
    private float upcap;
    private float downcap;
    private float speed = 3f;
    private Dirctions sidesdirection = Dirctions.Left;
    private Dirctions floatingdircetion = Dirctions.Up;

    void Start()
    {
        leftcap = transform.position.x - 8;
        rightcap = transform.position.x + 8;
        upcap = transform.position.y + 3;
        downcap = transform.position.y - 3;
    }

    void Update()
    {
        GhostMoveController();
    }

    private void GhostMoveController()
    {
        if (sidesdirection==Dirctions.Left)
        {
            if (transform.position.x > leftcap)
            {
                if (transform.localScale.x != 1)
                    transform.localScale = new Vector3(1, 1);

                if (floatingdircetion==Dirctions.Up)
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, speed);
                else
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-speed, -speed);
            }
            else
            {
                sidesdirection = Dirctions.Right;
            }
        }
        else
        {
            if (transform.position.x < rightcap)
            {
                if (transform.localScale.x != -1)
                    transform.localScale = new Vector3(-1, 1);

                if (floatingdircetion == Dirctions.Up)
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, speed);
                else
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, -speed);


            }
            else
            {
                sidesdirection = Dirctions.Left;
            }
        }

        if (floatingdircetion==Dirctions.Up)
        {
            if (transform.position.y > upcap)
            {
                floatingdircetion = Dirctions.Down;
            }
        }
        else
        {
            if (transform.position.y < downcap)
            {
                floatingdircetion = Dirctions.Up;
            }
        }
    }
}
