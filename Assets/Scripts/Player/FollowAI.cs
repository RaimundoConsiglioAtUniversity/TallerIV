using System.Collections;
using UnityEngine;
using Pathfinding;

public class FollowAI : MonoBehaviour
{
    public PlayerController pony;
    public PlayerInput player;
    public float nextWaypointDistance = 3;
    public float waypointDistance = 0f;
    public Vector2 waypointDirection = Vector2.zero;
    public float targetDistance = 2f;
    public Vector2 targetDirection = Vector2.left;
    public Rigidbody2D rb;

    private Path path;
    int currentWaypoint = 0;
    bool reachedPathEnd = false;

    Seeker seeker;

    public float hInput;
    public float vInput;
    public bool tryTapJump = false;
    public bool tryHoldJump = false;
    public bool tryRun = false;

    private float jumpTime = 0f;
    private bool isJumping = false;

    private Transform Target => pony.tribe.target.transform;


    void Awake()
    {
        pony = transform.parent.parent.GetComponent<PlayerController>();
        seeker = GetComponent<Seeker>();
        rb = transform.parent.parent.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = PlayerInput.Instance;

        StartCoroutine(UpdatePath()); // Recursively calls itself
    }

    IEnumerator UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, Target.position, OnPathComplete);

        yield return new WaitForSeconds(Time.fixedDeltaTime);

        StartCoroutine(UpdatePath()); // Recursively calls itself
    }
    
    void OnPathComplete(Path p)
    {
        if (p.error)
            return;

        path = p;
        currentWaypoint = 0;
    }

    void Update()
    {
        if (path == null)
            return;

        if (currentWaypoint < path.vectorPath.Count)
            reachedPathEnd = false;
        
        else
        {
            reachedPathEnd = true;
            return;
        }

        UpdatePathProgress();

        hInput = Mathf.Abs(waypointDirection.x) > 0.2f && Mathf.Abs(targetDistance) > 2f ? Mathf.Sign(waypointDirection.x) : 0f;
        vInput = Mathf.Abs(targetDistance) > 1f ? -1 : 0;
        tryRun = Mathf.Abs(targetDistance) > 5f;

        HandleJumpLogic();
    }

    void HandleJumpLogic()
    {
        if (ShouldJump())
        {
            if (!isJumping)
            {
                tryTapJump = true;
                isJumping = true;
                jumpTime = 0f;
            }
            else
            {
                jumpTime += Time.deltaTime;
                tryHoldJump = jumpTime < pony.stats.maxJumpTime;
            }
        }
        else
        {
            isJumping = false;
            tryTapJump = false;
            tryHoldJump = false;
        }
    }

    bool ShouldJump()
    {
        Vector2 vec = new(rb.position.x + Mathf.Sign(waypointDirection.x), rb.position.y + 10f);

        RaycastHit2D ObstacleHeight = Physics2D.Raycast(vec, Vector2.down, 11f, pony.stats.obstacleLayers);
        Debug.DrawRay(vec, Vector2.down * 12f, Color.red);

        bool canFlap = pony.tribe is PonyPegasus && !pony.groundC.IsGrounded && pony.rb.velocity.y < -0.01f && pony.currentFlaps < pony.stats.maxFlaps;

        return (pony.groundC.IsGrounded || canFlap) && (ObstacleHeight.point.y - rb.position.y) > 0.1f;
    }

    private void UpdatePathProgress()
    {
        Vector2 CurrentWaypointPath = (Vector2)path.vectorPath[currentWaypoint];

        if ((CurrentWaypointPath - rb.position).normalized.magnitude > 0.1f)
            waypointDirection = (CurrentWaypointPath - rb.position).normalized;

        if (((Vector2)Target.position - rb.position).normalized.magnitude > 0.1f)
            targetDirection = ((Vector2)Target.position - rb.position).normalized;

        waypointDistance = Vector2.Distance(rb.position, CurrentWaypointPath);
        targetDistance = Vector2.Distance(rb.position, Target.position);

        if (waypointDistance < 0.1f)
            waypointDistance = 0.1f;

        if (waypointDistance < nextWaypointDistance)
            currentWaypoint++;
        
    }
}
