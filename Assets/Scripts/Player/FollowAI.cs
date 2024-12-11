using System.Collections;
using UnityEngine;
using Pathfinding;

public class FollowAI : MonoBehaviour
{
    public PlayerController thisPony;
    public PlayerInput player;
    public Transform target;
    public float nextWaypointDistance = 3;
    public float waypointDistance = 0f;
    public Vector2 waypointDirection = Vector2.zero;
    public float targetDistance = 2f;
    public Vector2 targetDirection = Vector2.left;

    public LayerMask obstacleLayers;
    private Path path;
    int currentWaypoint = 0;
    bool reachedPathEnd = false;

    Seeker seeker;
    Rigidbody2D rb;


    public float hInput;
    public float vInput;
    public bool tryTapJump = false;
    public bool tryHoldJump = false;
    public bool tryRun = false;

    public float jumpCooldown = 0.1f;
    public float jumpCooldownTimer = 0f;
    float targetJumpHeight;

    void IncrementJumpTimer() => jumpCooldownTimer += Time.deltaTime;
    void ResetJumpTimer() => jumpCooldownTimer = 0f;

    void Awake()
    {
        thisPony = GetComponent<PlayerController>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        player = PlayerInput.Instance;

        StartCoroutine(UpdatePath()); // Recursively calls itself
    }

    IEnumerator UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);

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
        vInput = player.activePony.isDucking ? -1 : 0;
        tryRun = player.activePony.isRunning;

        if (thisPony.groundCheck.isGrounded)
            IncrementJumpTimer();
        else
            ResetJumpTimer();

        // Raycast to check for obstacles in front
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * Mathf.Sign(waypointDirection.x), 1f, obstacleLayers);

        /*
        if (hit.collider != null)
        {
            if (target.position.y - rb.position.y > 0.2f)
            {
                targetJumpHeight = target.position.y - rb.position.y;
            }
            else
            {
                // Cast the ray
                RaycastHit2D obstacleHit = Physics2D.Raycast(new Vector2(rb.position.x + Mathf.Sign(waypointDirection.x), 10f), Vector2.down, 11f, obstacleLayers);
                // Make the raycast visible
                Debug.DrawRay(new Vector2(rb.position.x + Mathf.Sign(waypointDirection.x), 10f), Vector2.down * 11f, Color.red);
                if (obstacleHit.collider != null)
                {
                    float frontObstacleHeightDifference = obstacleHit.point.y;
                    targetJumpHeight = frontObstacleHeightDifference - rb.position.y;
                // Your existing code to handle the raycast hit...
                }
            }

            print ($"{thisPony.name}'s Target Jump Height: {targetJumpHeight}");
            float holdJumpDuration = CalculateHoldJumpDuration(targetJumpHeight);
            

            if (thisPony.groundCheck.isGrounded || (!thisPony.groundCheck.isGrounded && thisPony.canFly && thisPony.charRB.velocity.y < 0.1f && (thisPony.currentAirJumps < thisPony.maxAirJumps)))
            {
                if (targetJumpHeight < 0.8f && rb.velocity.y < 0.01f)
                {
                    tryTapJump = true;
                }
                else if (targetJumpHeight >= 0.8f)
                {
                    tryHoldJump = true;
                    StartCoroutine(HoldJump(holdJumpDuration));
                }
            }
        }
        else
        {
            tryTapJump = false;
        }
        */
    }
    
    private IEnumerator HoldJump(float duration)
    {
        yield return new WaitForSeconds(duration);
        tryHoldJump = false;
    }

    private float CalculateHoldJumpDuration(float targetHeight)
    {
        return Mathf.Clamp(targetHeight / 3.5f, 0.01f, 0.25f);
    }

    private void UpdatePathProgress()
    {
        Vector2 CurrentWaypointPath = (Vector2)path.vectorPath[currentWaypoint];

        if ((CurrentWaypointPath - rb.position).normalized.magnitude > 0.1f)
            waypointDirection = (CurrentWaypointPath - rb.position).normalized;

        if (((Vector2)target.position - rb.position).normalized.magnitude > 0.1f)
            targetDirection = ((Vector2)target.position - rb.position).normalized;

        waypointDistance = Vector2.Distance(rb.position, CurrentWaypointPath);
        targetDistance = Vector2.Distance(rb.position, target.position);

        if (waypointDistance < 0.1f)
            waypointDistance = 0.1f;

        if (waypointDistance < nextWaypointDistance)
            currentWaypoint++;
        
    }
}
