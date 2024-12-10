using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FollowAI : MonoBehaviour
{
    public Transform target;
    public float speed = 200;
    public float nextWaypointDistance = 3;

    private Path path;
    int currentWaypointIndex = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, Time.fixedDeltaTime);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }
    void OnPathComplete(Path p)
    {
        if (p.error)
            return;

        path = p;
        currentWaypointIndex = 0;
    }

    void Update()
    {
        if (path == null)
            return;

        if(currentWaypointIndex >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }

        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypointIndex] - rb.position).normalized;

        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypointIndex]);

        if (distance < nextWaypointDistance)
        {
            currentWaypointIndex++;
        }
    }
}
