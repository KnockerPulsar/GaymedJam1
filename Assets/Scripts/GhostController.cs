using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : Enemy
{
    public float speed = 3f;
    public float distanceThreshold = 0.2f;
    public Transform[] waypoints;

    int currPathIndex = 0;
    int nextPathIndex = 1;
    float time = 0;

    void Start()
    {
        currPathIndex = 0;
    }

    void FixedUpdate()
    {
        GhostMoveController();
    }

    private void GhostMoveController()
    {
        float dist = Vector2.Distance(waypoints[currPathIndex].position, waypoints[nextPathIndex].position);
        time += Time.deltaTime * speed / dist;
        transform.position = Vector3.Lerp(waypoints[currPathIndex].position, waypoints[nextPathIndex].position, time);
        if (time >= 1)
        {
            time = 0;
            currPathIndex++;
            nextPathIndex++;
            if (currPathIndex == waypoints.Length) currPathIndex = 0;
            if (nextPathIndex == waypoints.Length) nextPathIndex = 0;
        }
    }

    public override void DeathAnimation()
    {
        // To stop the enemy from moving after death
        speed = 0;
        base.DeathAnimation();
    }
}
