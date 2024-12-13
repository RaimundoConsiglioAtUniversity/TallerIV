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

    public Coroutine jumpCor;

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
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * Mathf.Sign(waypointDirection.x), 2f, obstacleLayers);

            // Cast the ray
            RaycastHit2D obstacleHit = Physics2D.Raycast(new Vector2(rb.position.x + Mathf.Sign(waypointDirection.x), 10f), Vector2.down, 11f, obstacleLayers);
            // Make the raycast visible
            Debug.DrawRay(new Vector2(rb.position.x + Mathf.Sign(waypointDirection.x), 10f), Vector2.down * 11f, Color.red);
        
        if (hit.collider != null)
        {

            if (target.position.y - rb.position.y > 0.2f)
            {
                targetJumpHeight =  Mathf.Abs(target.position.y - rb.position.y);
            }
            else if (obstacleHit.collider != null && target.position.y - rb.position.y < 0.1f && Mathf.Abs(target.position.x - rb.position.x) > 1.2f)
            {
                    float frontObstacleHeightDifference = obstacleHit.point.y;
                    targetJumpHeight = Mathf.Abs(frontObstacleHeightDifference - rb.position.y) + 1f;
            }

            print ($"{thisPony.name}'s Target Jump Height: {targetJumpHeight}");
            float holdJumpDuration = CalculateHoldJumpDuration(targetJumpHeight);
            
            

            if (thisPony.groundCheck.isGrounded)
            {
                if (targetJumpHeight < 0.25f)
                {
                    jumpCor = StartCoroutine(TapJump());
                }
                else if (targetJumpHeight >= 0.25f)
                {
                    jumpCor = StartCoroutine(HoldJump(holdJumpDuration));
                }
            }
            
            if (thisPony.groundCheck.isGrounded == false &&  thisPony.charRB.velocity.y < 0.01f)
            {
                if (thisPony.canFly && (thisPony.currentAirJumps < thisPony.maxAirJumps))
                {
                    if (targetJumpHeight < 0.25f)
                    {
                        jumpCor = StartCoroutine(TapJump());
                    }
                    else if (targetJumpHeight >= 0.25f)
                    {
                        jumpCor = StartCoroutine(HoldJump(holdJumpDuration/2f));
                    }
                }

                else
                {
                    tryHoldJump = false;
                    tryTapJump = false;
                    jumpCor = null;
                }
            }
        }
    }
    
    private IEnumerator HoldJump(float duration)
    {
        tryTapJump = true;
        tryHoldJump = true;

        yield return null;

        tryTapJump = false;

        yield return new WaitForSeconds(duration);

        tryHoldJump = false;

        jumpCor = null;
    }
    private IEnumerator TapJump()
    {
        tryTapJump = true;

        yield return null;

        tryTapJump = false;

        jumpCor = null;
    }

    private float CalculateHoldJumpDuration(float targetHeight)
    {
        float f = Mathf.Clamp(targetHeight/4f, 0.01f, 0.59f);
        print($"{thisPony.name}'s Jump Holding Duration: {f}s");
        return f;
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
