using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void DeathAnimation()
    {
        this.GetComponent<Animator>().SetTrigger("Death");
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        this.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
