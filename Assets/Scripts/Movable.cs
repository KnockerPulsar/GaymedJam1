using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movable : MonoBehaviour
{
    public float maxVelSqr = 225f;
    Rigidbody2D rb;
    
    // Awake is called before Update() and FixedUpdate()
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rb.velocity.sqrMagnitude >= maxVelSqr)
        {
            Vector2 vel = rb.velocity;
            vel.Normalize();
            vel *= maxVelSqr;
            rb.velocity = vel;
        }
    }
}
