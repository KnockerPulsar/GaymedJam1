using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject parentGO;
    public virtual void DeathAnimation()
    {
        Animator animator = GetComponent<Animator>();
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D collider2D = GetComponent<Collider2D>();

        if (animator)
            animator.SetTrigger("Death");
        if (rigidbody2D)
            rigidbody2D.velocity = Vector2.zero;
        if (collider2D)
            collider2D.isTrigger = true;
    }

    public void Death()
    {
        // In case we need the enemy to have a parent like the ghost, deleting the parent should delete the child
        if (parentGO)
            Destroy(parentGO);
        else
            Destroy(gameObject);
    }
}
